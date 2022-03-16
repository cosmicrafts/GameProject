using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainStation : MonoBehaviour
{
    Unit MyUnit;

    MeshFilter MyMesh;
    MeshRenderer MyRender;

    private void Start()
    {
        MyUnit = GetComponent<Unit>();
        MyMesh = MyUnit.Mesh.GetComponent<MeshFilter>();
        MyRender = MyUnit.Mesh.GetComponent<MeshRenderer>();

        if (GameData.CurrentMatch != Match.multi && MyUnit.MyTeam == Team.Red)
        {
            MyMesh.mesh = GameMng.GM.SkinStationsMeshes[4];
            MyRender.material = GameMng.GM.SkinStationsMaterials[4];
        }
    }
}
