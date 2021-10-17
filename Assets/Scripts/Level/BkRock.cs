using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BkRock : MonoBehaviour
{
    float starty = 0f;
    float size = 1f;
    float speed = 0f;
    float speedrotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        starty = transform.position.y;
        float xpos = Random.Range(GameMng.GM.MapX, GameMng.GM.MapX + GameMng.GM.MapWidth);
        float zpos = Random.Range(GameMng.GM.MapZ -10f, GameMng.GM.MapZ + (GameMng.GM.MapHeigth * 1.5f));
        transform.position = new Vector3(xpos, starty, zpos);
        transform.Rotate(Vector3.up, Random.Range(0f,360f), Space.World);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-speed*Time.deltaTime, 0f, -speed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.up, -speed * Time.deltaTime * speedrotation, Space.World);
        if (transform.position.x < GameMng.GM.MapX - 10f || transform.position.z < GameMng.GM.MapZ - 20f)
        {
            speed = 5f / size;
            transform.localScale = new Vector2(size, size);
            float xpos = Random.Range(GameMng.GM.MapX, GameMng.GM.MapX + (GameMng.GM.MapWidth * 1f));
            float zpos = GameMng.GM.MapZ + GameMng.GM.MapHeigth + 1f;
            transform.position = new Vector3(xpos, starty, zpos);
        }
    }

    public void InitSize(float min, float max)
    {
        transform.localScale = new Vector2(size, size);
    }

    public void InitSpeed(float min, float max)
    {
        speed = Random.Range(min, max) / size;
    }

    public void InitRotSpeed(float min, float max)
    {
        speedrotation = Random.Range(min, max);
    }
}
