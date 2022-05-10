using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ERC20NameExample : MonoBehaviour
{
    async void Start()
    {
        string chain = "Oasis Emerald";
        string network = "testnet";
        string contract = "0x6224394Deb3DEd876c20e1039409cb6CdDD64dD6";

        string _name = await ERC20.Name(chain, network, contract);
       PlayerPrefs.SetString("Account",_name);
        print(name); 
    }


}
