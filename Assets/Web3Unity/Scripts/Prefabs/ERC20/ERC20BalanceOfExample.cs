using System.Collections;
using System.Numerics;
using System.Collections.Generic;
using UnityEngine;

public class ERC20BalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string chain = "42261";
        string network = "testnet";
        string contract = "0x3De545e93Cb2D6342bb02b381f46EdB7F8b0b8ee";
        string account = "0xf0d056015Bdd86C0EFD07000F75Ea10873A1d0A7";

        BigInteger balanceOf = await ERC20.BalanceOf(chain, network, contract, account);
        print(balanceOf); 
    }
}
