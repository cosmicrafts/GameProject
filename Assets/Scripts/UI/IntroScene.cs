using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScene : MonoBehaviour
{

    [SerializeField]
    Animator topAnimator;
    [SerializeField]
    Animator botAnimator;
    [SerializeField]
    GameObject topPanel;
    [SerializeField]
    GameObject botPanel;
    [SerializeField]
    AudioSource BackGroundSound;
    private void Awake()
    {
        StartCoroutine(InitialiazeIntro());
    }

    IEnumerator InitialiazeIntro()
    {
        topPanel.SetActive(true);
        botPanel.SetActive(true);
        topAnimator.SetTrigger("MovePanel");
        botAnimator.SetTrigger("MovePanel");

        yield return new WaitForSeconds(2);
        topPanel.SetActive(false);
        botPanel.SetActive(false);
        BackGroundSound.Play();
    }
}
