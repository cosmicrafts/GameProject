using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Web3PrivateKeySignMessageExample : MonoBehaviour
{
    void Start()
    {
        string privateKey = "46a0c839ac4465538ab22ae5b709853d2ba64a604c6ddb06cbab681e7a70bc78";
        string message = "hello";
        string response = Web3PrivateKey.Sign(privateKey, message);
        print(response);
    }
}
