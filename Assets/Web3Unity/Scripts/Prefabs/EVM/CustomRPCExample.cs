using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomRPCExample : MonoBehaviour
{
    async void Start()
    {
        string chain = "42261";
        string network = "testnet"; 
        string account = "0x6224394Deb3DEd876c20e1039409cb6CdDD64dD6";
        string rpc = "https://testnet.emerald.oasis.dev";

        string balance = await EVM.BalanceOf(chain, network, account, rpc);
        print(balance);
    }
}
