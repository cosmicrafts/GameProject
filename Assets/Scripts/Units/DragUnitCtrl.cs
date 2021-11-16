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
    GameObject currentPreview;

    public float TargetCost;

    Color DefaultColor;

    Player player;

    private void Start()
    {
        areas = 0;
        SetStatusColor(Color.red);
        target = GameMng.GM.GetFinalTarget(GameMng.P.MyTeam).position;
        DefaultColor = Color.green;
        player = GameMng.P;
    }

    private void Update()
    {
        DefaultColor = TargetCost > player.CurrentEnergy ? Color.blue : Color.green;
        SetStatusColor(areas > 0 ? DefaultColor : Color.red);
    }

    private void FixedUpdate()
    {
        transform.position = CMath.GetMouseWorldPos();
        transform.LookAt(CMath.LookToY(transform.position, target));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spawnarea"))
        {
            areas++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Spawnarea"))
        {
            areas--;
        }
    }

    private void OnDisable()
    {
        areas = 0;
        SetStatusColor(Color.red);
        DefaultColor = Color.green;
        if (currentPreview != null)
        {
            Destroy(currentPreview);
        }
    }

    public bool IsValid()
    {
        return areas > 0;
    }

    void SetStatusColor(Color color)
    {
        Outline.OutlineParameters.Color = color;
    }

    public void SetMeshAndTexture(Mesh mesh, Material mat)
    {
        MyMesh.material = mat;
        MyMeshFilter.mesh = mesh;
    }

    public void setMeshActive(bool active)
    {
        MyMesh.gameObject.SetActive(active);
    }

    public void CreatePreviewObj(GameObject preview)
    {
        currentPreview = Instantiate(preview, transform);
    }
}
