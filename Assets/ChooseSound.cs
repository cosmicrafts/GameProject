using UnityEngine;

public class ChooseSound : MonoBehaviour
{
    public void PlaySound(AudioClip audioClip)
    {
        Debug.Log("1");
        SoundManager.Instance.Play(audioClip);
    }
    public void PlayMusic(AudioClip audioClip)
    {
        SoundManager.Instance.PlayMusic(audioClip);
    }
    
}