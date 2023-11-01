using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroButton : MonoBehaviour, IPointerDownHandler
{
    public System.Action<HeroButton> OnDown;
    public int Index = 0;
    public Image SpIcon;
    public int EnergyCost = 99;
    public Text TextCost;
    public GameObject Selection;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDown?.Invoke(this);
    }
    
    public void SetSelection(bool selected)
    {
        Selection.SetActive(selected);
    }
}
