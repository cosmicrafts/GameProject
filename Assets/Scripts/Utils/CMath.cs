using UnityEngine;

public static class CMath
{
    public static float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
    {
        Vector2 diference = vec2 - vec1;
        float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
        float angle = Vector2.Angle(Vector2.right, diference) * sign;
        if (angle < 0) { angle += 360; }
        return angle;
    }

    public static Vector3 GetMouseWorldPos()
    {
        //float wx = (Input.mousePosition.x * GameMng.GM.MapWidth) / Screen.width;
        //float wz = (Input.mousePosition.y * GameMng.GM.MapHeigth) / Screen.height;

        //Vector3 WorldPos = new Vector3(GameMng.GM.MapX + wx, 0f, GameMng.GM.MapZ + wz - 4f);

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f, 1 << 9))
        {
            return hit.point;
        }

        return Vector3.zero;
    }

    public static Vector3 LookToY(Vector3 from, Vector3 to)
    {
        Vector3 rotation = new Vector3(to.x, from.y, to.z);
        return rotation;
    }
}
