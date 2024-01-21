using UnityEngine;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using System;
using System.Text;

public class KeyGenerator : MonoBehaviour
{
    public void GenerateAndPrintKeys()
    {
        var (publicKey, privateKey) = GenerateRSAKeys();
        string json = CreateJsonString(publicKey, privateKey);
        Debug.Log(json);
    }

    private (string PublicKey, string PrivateKey) GenerateRSAKeys()
    {
        using (var rsa = new RSACryptoServiceProvider(2048))
        {
            return (Convert.ToBase64String(rsa.ExportParameters(false).Modulus),
                    Convert.ToBase64String(rsa.ExportParameters(true).Modulus));
        }
    }

    private string CreateJsonString(string publicKey, string privateKey)
    {
        var json = new JObject(
            new JProperty("_inner", new JArray(publicKey, privateKey)),
            new JProperty("_delegation", new JObject(
                new JProperty("delegations", new JArray(
                    new JObject(
                        new JProperty("delegation", new JObject(
                            new JProperty("expiration", "0000000000000000"), // Valor que representa 'no expira'
                            new JProperty("pubkey", publicKey)
                        )),
                        new JProperty("signature", Convert.ToBase64String(Encoding.UTF8.GetBytes("Firma de ejemplo")))
                    )
                )),
                new JProperty("publicKey", publicKey)
            ))
        );

        return json.ToString();
    }
}