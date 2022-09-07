using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CanvasDamage : MonoBehaviour
{
    [SerializeField]
    TMP_Text damageText;

    [SerializeField]
    float damageCanvasSpeed ;

    float damageValue;
    
    void Start()
    {
       
        float timeScale = Random.Range(2.2f, 6.5f);
        damageCanvasSpeed = timeScale;
        damageText.text = "" + (int)damageValue;
    }

    // Update is called once per frame
    void Update()
    {
     transform.position= new Vector3(transform.position.x, transform.position.y +damageCanvasSpeed *Time.deltaTime, transform.position.z);
       
        Destroy(gameObject, 0.8f);
    }

    public void SetDamage(float newDamage)
    {
        damageValue =newDamage;
    }
}
