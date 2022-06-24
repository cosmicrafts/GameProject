using UnityEngine;

/*
 * This is the basic game card data used on the units and spells prefabs
 */

public class GameCard : MonoBehaviour
{
    public string NftsKey;

    public string Alias;

    NFTsCard Data;

    public NFTsCard GetData()
    {
        if (Data == null)
            Data = GlobalManager.GMD.GetUserCollection().FindCard(NftsKey);

        return Data;
    }
}
