using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Scripting;
using Newtonsoft.Json;

public class TestNft : MonoBehaviour
{
    string urlSelectedNFTX;
   List<Sprite> selectedNFTXImages = new List<Sprite>();  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       

    }
    [Preserve]
    public class RequestAnvilConnectResponse
    {
        [Preserve] public string result;
    }


    [System.Obsolete]
    public IEnumerator GetPlayerNFTX(string u)
    {
        urlSelectedNFTX = u;
        WWW www = new WWW(u);
        yield return www.url;

      //Sprite.Create(www.texture, new Rect(0.0f, 0.0f, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f), 100.0f);
    }

    
}
