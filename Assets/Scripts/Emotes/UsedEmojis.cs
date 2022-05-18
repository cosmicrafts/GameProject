using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UsedEmojis : MonoBehaviour
{
    [SerializeField]
    List<Sprite> emojis = new List<Sprite>();

    [SerializeField] int avatarIndex;

    [SerializeField]
    Image imageEmoji;
    [SerializeField]
    GameObject emojisPanel;
 
    void Start()
    {
        emojisPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ActiveEmojisPaneles()
    {
        emojisPanel.SetActive(true);
    }
    public void UsedEmoji(int index)
    {
        StartCoroutine(ActivateEmoji(index));
    }
    IEnumerator ActivateEmoji(int index)
    {
        avatarIndex = index;
        imageEmoji.enabled = true;
        emojisPanel.SetActive(false);
        imageEmoji.sprite = emojis[avatarIndex];
        yield return new WaitForSeconds(3);
        imageEmoji.enabled = false;
        avatarIndex = 0;
    }
}
