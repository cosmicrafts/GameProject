using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;

public class Signup : MonoBehaviour
{
    public string user { get; set; }
    public string email { get; set; }
    public string password { get; set; }

    public Text label_status;

    public GameObject Login;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void RequestSignup()
    {
        label_status.text = "Registrando...";
        label_status.color = Color.blue;

        NetResult netResult = await NetUserServices.SignUp(user, password, email);

        if (netResult.Status == EStatus.success)
        {
            label_status.text = "Usuario Registrado!";
            label_status.color = Color.green;
        }
        else
        {
            label_status.text = netResult.Response;
            label_status.color = Color.red;
        }
    }
}
