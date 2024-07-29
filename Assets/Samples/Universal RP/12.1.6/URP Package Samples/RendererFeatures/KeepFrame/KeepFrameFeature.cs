using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Experimental.Rendering;

public class KeepFrameFeature : ScriptableRendererFeature
{
    //This pass is responsible for copying color to a specified destination
    class CopyFramePass : ScriptableRenderPass
    {
        private RTHandle source { get; set; }
        private RTHandle destination { get; set; }

        public void Setup(RTHandle source, RTHandle destination)
        {
            this.source = source;
            this.destination = destination;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (renderingData.cameraData.camera.cameraType != CameraType.Game)
                return;

            CommandBuffer cmd = CommandBufferPool.Get("CopyFramePass");
            Blit(cmd, source, destination);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            if (destination != null)
            {
                RTHandles.Release(destination);
                destination = null;
            }
        }
    }

    //This pass is responsible for drawing the old color to a full screen quad
    class DrawOldFramePass : ScriptableRenderPass
    {
        private Material m_DrawOldFrameMaterial;
        private RTHandle m_handle;
        private string m_textureName;

        public void Setup(Material drawOldFrameMaterial, RTHandle handle, string textureName)
        {
            m_DrawOldFrameMaterial = drawOldFrameMaterial;
            m_handle = handle;
            m_textureName = textureName;
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            RenderTextureDescriptor descriptor = cameraTextureDescriptor;
            descriptor.msaaSamples = 1;
            descriptor.depthBufferBits = 0;

            if (m_handle == null)
            {
                m_handle = RTHandles.Alloc(descriptor.width, descriptor.height, 1, DepthBits.None,
                                           GraphicsFormatUtility.GetGraphicsFormat(descriptor.colorFormat, RenderTextureReadWrite.Default),
                                           FilterMode.Bilinear, TextureWrapMode.Clamp, TextureDimension.Tex2D, false, false,
                                           false, false, 1, 0, MSAASamples.None, false, false, RenderTextureMemoryless.None, "_OldFrameRenderTarget");
            }
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (m_DrawOldFrameMaterial != null)
            {
                CommandBuffer cmd = CommandBufferPool.Get("DrawOldFramePass");
                cmd.SetGlobalTexture(m_textureName, m_handle);
                cmd.SetViewProjectionMatrices(Matrix4x4.identity, Matrix4x4.identity);
                cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, m_DrawOldFrameMaterial, 0, 0);
                cmd.SetViewProjectionMatrices(renderingData.cameraData.camera.worldToCameraMatrix, renderingData.cameraData.camera.projectionMatrix);
                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);
            }
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            if (m_handle != null)
            {
                RTHandles.Release(m_handle);
                m_handle = null;
            }
        }
    }

    [Serializable]
    public class Settings
    {
        [Tooltip("The material that is used when the old frame is redrawn at the start of the new frame (before opaques).")]
        public Material displayMaterial;
        [Tooltip("The name of the texture used for referencing the copied frame. (Defaults to _FrameCopyTex if empty)")]
        public string textureName = "_FrameCopyTex";
    }

    private CopyFramePass m_CopyFrame;
    private DrawOldFramePass m_DrawOldFrame;

    private RTHandle m_OldFrameHandle;

    public Settings settings = new Settings();

    // In this function the passes are created and their point of injection is set
    public override void Create()
    {
        m_CopyFrame = new CopyFramePass
        {
            renderPassEvent = RenderPassEvent.AfterRenderingTransparents // Frame color is copied late in the frame
        };

        m_DrawOldFrame = new DrawOldFramePass
        {
            renderPassEvent = RenderPassEvent.BeforeRenderingOpaques // Old frame is drawn early in the frame
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (m_OldFrameHandle == null)
        {
            m_OldFrameHandle = RTHandles.Alloc("_OldFrameRenderTarget", name: "_OldFrameRenderTarget");
        }

        m_CopyFrame.Setup(renderer.cameraColorTargetHandle, m_OldFrameHandle);
        renderer.EnqueuePass(m_CopyFrame);

        m_DrawOldFrame.Setup(settings.displayMaterial, m_OldFrameHandle, string.IsNullOrEmpty(settings.textureName) ? "_FrameCopyTex" : settings.textureName);
        renderer.EnqueuePass(m_DrawOldFrame);
    }

    protected override void Dispose(bool disposing)
    {
        if (m_OldFrameHandle != null)
        {
            RTHandles.Release(m_OldFrameHandle);
            m_OldFrameHandle = null;
        }
    }
}
