using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPanel : MonoBehaviour
{
    public static LoadingPanel instance;
    public GameObject loadingPanel;
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            DesactiveLoadingPanel();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ActiveLoadingPanel()
    {
        loadingPanel.SetActive(true);
    }
    public void DesactiveLoadingPanel()
    {
        loadingPanel.SetActive(false);
    }
}

