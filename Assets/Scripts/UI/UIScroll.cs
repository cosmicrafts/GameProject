using System.Collections;
using UnityEngine;

public class UIScroll : MonoBehaviour
{
    private float h = Screen.height;
    private float w = Screen.width; 
    private float Ref = 768f;
    public float timeAnimation = 0.5f;

    public void MoveTo(string item = "0,0")
    {
        string[] split = item.Split(",");
        int column = int.Parse(split[0]);
        int row = int.Parse(split[1]);
        Debug.Log("Column: "+column+"  Row: "+ row);
           
        h = Screen.height;
        w = Screen.width;
        StopAllCoroutines();
        StartCoroutine(AnimationCoRoutine(column,row));
    }
       
    private IEnumerator AnimationCoRoutine(int column, int row)
    {
        Vector3 start = transform.localPosition;
        float targetX = (w / h) * Ref * column;
        float targetY =  Ref * row;
        Vector3 targetPosition = new Vector3(targetX, targetY, transform.localPosition.z);
   
        // animate over 1/2 second
        for (float accumTime = Time.deltaTime; accumTime <= timeAnimation; accumTime += Time.deltaTime)
        {
            transform.localPosition = Vector3.Lerp(start, targetPosition, accumTime / timeAnimation);
            yield return null;
        }
           
        transform.localPosition = targetPosition;
    }
}
