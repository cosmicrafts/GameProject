using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContadorFPS : MonoBehaviour
{
    public TMP_Text tmp;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CalcularFPS());
    }

    // Update is called once per frame
    IEnumerator CalcularFPS()
    {
        while (true)
        {
            tmp.text = (1f / Time.deltaTime).ToString("00");
            yield return new WaitForSeconds(1f);
        }
       
    }
}
