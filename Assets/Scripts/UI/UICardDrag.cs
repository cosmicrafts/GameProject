using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICardDrag : MonoBehaviour
{
    public Image Icon;

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
    }
}
