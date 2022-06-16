using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Scripting;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
public class TestNft : MonoBehaviour
{

    string urlSelectedNFTX;
   List<Sprite> selectedNFTXImages = new List<Sprite>();
    // Start is called before the first frame update
    [SerializeField]
    GameObject nftTestPanel;
    [SerializeField]
    Image spriteX;

    void Start()
    {
        nftTestPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       

    }
   
    public void JSAnvilConnect()
    {
        GameNetwork.JSAnvilConnect();
          nftTestPanel.SetActive(true);
    }
    public void GetNFT(string url)
    {
      
        StartCoroutine(GetPlayerNFTX(url));
        GameNetwork.JSGetAnvilNfts(url);
        
    }
    public IEnumerator GetPlayerNFTX(string u)
    {
        urlSelectedNFTX = u;
        WWW www = new WWW(u);
        yield return www;
        spriteX.sprite = Sprite.Create(www.texture, new Rect(0.0f, 0.0f, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f), 100.0f);
    }

}
