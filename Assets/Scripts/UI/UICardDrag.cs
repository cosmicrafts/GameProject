using UnityEngine;
using UnityEngine.UI;

public class UICardDrag : MonoBehaviour
{
    /* This class represents the current draging card
     * for the deck collection menu
     */

    //The icon of the current draging card
    public Image Icon;

    // Update is called once per frame
    void Update()
    {
        //Always follow the mouse
        transform.position = Input.mousePosition;
    }
}
