using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public static GameData GMD;

    public static char[] NFTsPrefix = new char[4] { 'C', 'H', 'S', 'U' };

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
