using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;

public class DragUnitCtrl : MonoBehaviour
{
    int areas;
    Vector3 target;

    public MeshRenderer MyMesh;
    public MeshFilter MyMeshFilter;
    public Outlinable Outline;

    private void Start()
    {
        areas = 0;
        Outline.OutlineParameters.Color = Color.red;
        target = GameMng.GM.GetFinalTarget(GameMng.P.MyTeam).position;
    }

    private void Update()
    {
        //transform.LookAt(CMath.LookToY(transform.position, target));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spawnarea"))
        {
            areas++;
            Outline.OutlineParameters.Color = Color.green;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Spawnarea"))
        {
            areas--;
            if (areas == 0)
            {
                Outline.OutlineParameters.Color = Color.red;
            }
        }
    }

    private void OnDisable()
    {
        areas = 0;
        Outline.OutlineParameters.Color = Color.red;
    }

    public bool IsValid()
    {
        return areas > 0;
    }

    public void SetMeshAndTexture(Mesh mesh, Material mat)
    {
        MyMesh.material = mat;
        MyMeshFilter.mesh = mesh;
    }
}
