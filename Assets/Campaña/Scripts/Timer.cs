using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public int minutes; // variable para minutos
    public int seconds; // variable para segundos
    private int m, s; // varaible adicionales para minutos y segundos

    //public Text timerText;
    public TextMeshProUGUI timerText; // varaible para el timer text
    private GameControl gameControl;  // varaible para el GameControl.cs
    // Start is called before the first frame update
    void Start()
    {
        // instanaciamos el script GameControl.cs
        gameControl = gameObject.GetComponent<GameControl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTimer() // funcion para iniciar el timer
    {
        m = minutes;
        s = seconds;
        WriteTimer(m, s); // escribe los minutos y segundo
        Invoke("UpdateTimer", 1f); // inicia la funcion "UpdateTimer", cada 1 segundo
        
    }

    public void StopTimer() // paramos el timer
    {
        CancelInvoke(); // cancelar el ciclo de la funcion
    }

    private void UpdateTimer() // actualiar el segundero del timer
    {
        s--;
        if(s < 0)
        {
            if(m == 0)
            {
                // EndGAme
                gameControl.EndGame(); // se ejecuta la funcion de GameControl.cs
                return;
            }
            else
            {
                m--;
                s = 59;
            }
        }

        WriteTimer(m, s); // se escribe el texto

        Invoke("UpdateTimer", 1f); // se ejecuta el funcion cada 1 segundo
    }

    

    private void WriteTimer(int m, int s) // mostrar el reloj en el texto
    {
        if(s < 10)
        {
            timerText.text = m.ToString() + ":0" + s.ToString();
        }
        else
        {
            timerText.text = m.ToString() + ":" + s.ToString();
        }
    }
}
