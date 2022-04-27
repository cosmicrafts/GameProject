using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    private Timer timer;  // creamos variable para enlacar el script de timer.cs
    // Start is called before the first frame update
    void Start()
    {
        timer = gameObject.GetComponent<Timer>(); // creamos instancia de Timer

        StartGame(); // ejecutamos la funcion para empezar el timer
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        timer.StartTimer(); // llamamos la funcion para empezar el Timer de timer.cs
    }

    public void EndGame()
    {
        timer.StopTimer(); // paramos el timer desde su funcion del timer.cs
        ExisteObjeto(); // comprobamos si existe el objeto llamando la funcion
    }

    void ExisteObjeto()
    {
        // si un objeto con la etiqueta "player" es encontrado o no se muestra el msg
        if(GameObject.FindGameObjectWithTag("Player"))
        {
            Debug.Log("Encontrado");
        }
        else
        {
            Debug.Log("NO encontrado");
        }
    }
}
