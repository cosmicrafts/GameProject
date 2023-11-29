using System.Collections;
using System.Collections.Generic;
using Candid;
using UnityEngine;

public class LogOutButton : MonoBehaviour
{
    public void OnClicked()
    {
        CandidApiManager.Instance.LogOut();
    }
}
