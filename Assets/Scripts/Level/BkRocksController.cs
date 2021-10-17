using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BkRocksController : MonoBehaviour
{
    GameObject BaseRock;

    [Range(0,50)]
    public int Rocks = 0;

    [Range(0, 100)]
    public float MinSpeed = 0f;
    [Range(0, 100)]
    public float MaxSpeed = 0f;

    [Range(0, 100)]
    public float MinSize = 0f;
    [Range(0, 100)]
    public float MaxSize = 0f;

    [Range(0, 100)]
    public float MinSpeedRotation = 5f;
    [Range(0, 100)]
    public float MaxSpeedRotation = 10f;

    // Start is called before the first frame update
    void Start()
    {
        BaseRock = transform.GetChild(0).gameObject;
        if (Rocks == 0)
        {
            BaseRock.SetActive(false);
            return;
        }
        else if (Rocks > 1)
        {
            for (int i = 1; i < Rocks; i++)
                Instantiate(BaseRock, transform);
        }

        for(int i = 0; i<transform.childCount; i++)
        {
            BkRock rock = transform.GetChild(i).GetComponent<BkRock>();
            rock.InitSize(MinSize, MaxSize);
            rock.InitSpeed(MinSpeed, MaxSpeed);
            rock.InitRotSpeed(MinSpeedRotation, MaxSpeedRotation);
        }
    }
}
