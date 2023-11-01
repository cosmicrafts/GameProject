using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    public void Shot(Vector3 targetPosition, GameObjectManager gameObjectManager)
    {
        StartCoroutine(OnShot(targetPosition, gameObjectManager));
    }

    private IEnumerator OnShot(Vector3 targetPosition, GameObjectManager gameObjectManager)
    {
        float time = 0.0f;
       
        while (time < 0.2f)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(targetPosition + 5.0f * Vector3.up, targetPosition,  time / 0.2f);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
