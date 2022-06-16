using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Scripting;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
public class TestNft : MonoBehaviour
{
    [SerializeField] private Sprite spriteX;
    string urlSelectedNFTX;
   List<Sprite> selectedNFTXImages = new List<Sprite>();
    // Start is called before the first frame update
    [SerializeField]
    Sprite sprite;
    GameObject nftTestPanel;
    [SerializeField]
    Transform instanceImage;
    [SerializeField]
    GameObject imageHolder;
    void Start()
    {
        nftTestPanel.SetActive(false);
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
    public void JSAnvilConnect()
    {
        GameNetwork.JSAnvilConnect();
          nftTestPanel.SetActive(true);
    }
    public void GetNFT(string url)
    {
      
        StartCoroutine(GetPlayerNFTX(url));
        GameNetwork.JSGetAnvilNfts(url);
        Debug.Log(url);
    }
    public IEnumerator GetPlayerNFTX(string u)
    {
        urlSelectedNFTX = u;
        WWW www = new WWW(u);
        yield return www.url;
        foreach (Sprite sprites in selectedNFTXImages)
        {

            selectedNFTXImages.Add(sprites);
         
            break;
        }
        for(int i = 0; i < selectedNFTXImages.Count; i++)
        {
   Image toke=   Instantiate(imageHolder, instanceImage.transform.position, Quaternion.identity).GetComponent<Image>();

            toke.sprite = Sprite.Create(www.texture, new Rect(0.0f, 0.0f, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
      
      
    }

    
}
