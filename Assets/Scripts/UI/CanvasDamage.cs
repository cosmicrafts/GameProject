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
    
    Camera mainCamera;
    
    void Start()
    {
        float timeScale = Random.Range(1.1f, 4.5f);
        damageCanvasSpeed = timeScale;
        damageText.text = "" + (int)damageValue;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position= new Vector3(transform.position.x, transform.position.y +damageCanvasSpeed *Time.deltaTime, transform.position.z);
        Destroy(gameObject, 2.5f);
    }

    public void SetDamage(float newDamage)
    {
        mainCamera = Camera.main;
        damageValue =newDamage;
       
        //The UI always look at the camera
        if (mainCamera) { transform.LookAt(mainCamera.transform); }

    }
}
