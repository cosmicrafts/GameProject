using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public static GameData GMD;

    private void Awake()
    {
        GMD = new GameData();
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        GMD = null;
    }
}
