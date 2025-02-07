using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject nave; // se crea variable nave para adjuntar el objeto 
    public int cantidad; // contador de la cantidad de naves simultaneas
    public int maximo; // el maximo permitido
    int maximoNum = 0;  // inicializa el contador maximo
    int numero = 0; // inicializa el contador simultaneo
    GameObject[] objetos;  // lista creada para saber la cantidad de objetos
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", 2, 1.5f); // invocar funcion y repetirla en unos interbalos
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Spawn()
    {
        // si numero de naves es menos que la cantidad maxima simultanea
        // Y el numero de naves maximas es menor que el maximo total permitido
        if(numero < cantidad && maximoNum < maximo)
        {
            // se creo un nuevo objeto(nave- esfera :D )
            Instantiate(nave, new Vector3(0, 0, 0), nave.transform.rotation);
            numero++; // se suma par el contador simultaneo
            maximoNum++; // para el contador maximo
            Debug.Log(" numero:"+numero);  // muestra numero siltaneo
            if(numero == cantidad) // si numero simultane es igual a cantidad maxima
            {
                // mostramos mensaje en consola
                Debug.Log("Tienes:" + cantidad + "Maximo permitido simultaneo");
            }
        }else  // si numero no es menos que cantidad y maximonum no es menos que maximo 
        {
            // objenemos la cantidad de objetos con la etiqueda de "player" existen
            objetos = GameObject.FindGameObjectsWithTag("Player");
            if(objetos.Length < cantidad) // si la cantidad de naves existentes en menos que la permitida entra
            {
                // si se eliminan objetos entonces tendremos menos objetos simultaneos
                numero = objetos.Length; // se asigna la nueva cantidad de objetos
            }
        }
        // si el numero maximo de objetos permitidos en total es igual a el maximo
        if(maximoNum == maximo)
        {
            // mostramo mensaje que ya se lleno al maximo y no se crearan mas objetos
            Debug.Log("Numero MAXIMO de "+ maximo + " unidades");
        }
        
        //Debug.Log("objetos:" + objetos.Length + "numero:" + numero);
    }
}
