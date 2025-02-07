using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeteccionObjetos : MonoBehaviour
{
    public List<Collider> listaObjetos = new List<Collider>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            listaObjetos.Add(col);
            Debug.Log("La zona tiene "+ listaObjetos.Count +" objetos");
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            listaObjetos.Remove(col);
            Debug.Log("La zona tiene "+ listaObjetos.Count +" objetos");
        }
    }
}
