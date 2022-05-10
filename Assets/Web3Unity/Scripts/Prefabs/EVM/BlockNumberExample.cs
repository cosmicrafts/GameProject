using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockNumberExample : MonoBehaviour
{
    async void Start()
    {
        string chain = "Oasis Emerald";
        string network = "testnet"; // mainnet ropsten kovan rinkeby goerli
        int blockNumber = await EVM.BlockNumber(chain, network);
        print(blockNumber);
    }
}
