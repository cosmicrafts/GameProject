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

        switch(GameData.CurrentMatch)
        {
            case Match.bots:
                {
                    if (GameMng.P.MyTeam == MyUnit.MyTeam)
                    {
                        MyMesh.mesh = GameMng.GM.SkinStationsMeshes[GameMng.PlayerCharacter.CharacterId];
                        MyRender.material = GameMng.GM.SkinStationsMaterials[GameMng.PlayerCharacter.CharacterId];
                    } else
                    {
                        MyMesh.mesh = GameMng.GM.SkinStationsMeshes[4];
                        MyRender.material = GameMng.GM.SkinStationsMaterials[4];
                    }
                }
                break;
            case Match.tutorial:
                {
                    if (MyUnit.MyTeam != GameMng.P.MyTeam)
                    {
                        MyMesh.mesh = GameMng.GM.SkinStationsMeshes[4];
                        MyRender.material = GameMng.GM.SkinStationsMaterials[4];
                    }
                }
                break;
            case Match.multi:
                {
                    if (GameMng.P.MyTeam == MyUnit.MyTeam)
                    {
                        MyMesh.mesh = GameMng.GM.SkinStationsMeshes[GameMng.PlayerCharacter.CharacterId];
                        MyRender.material = GameMng.GM.SkinStationsMaterials[GameMng.PlayerCharacter.CharacterId];
                    }
                    else
                    {
                        UserGeneral vs = GameData.GetVsUser();
                        int IdStation;
                        int.TryParse(vs.Icon.Substring(10), out IdStation);
                        MyMesh.mesh = GameMng.GM.SkinStationsMeshes[IdStation];
                        MyRender.material = GameMng.GM.SkinStationsMaterials[IdStation];
                    }
                }
                break;
        }
    }
}
