using UnityEngine;
using UnityEditor;

public class SODataLoader : MonoBehaviour
{
    // Function to load any MonoBehaviour from a JSON file
    public T LoadMonoBehaviour<T>(string jsonPath) where T : MonoBehaviour
    {
        // Load JSON data from the file
        string jsonData = System.IO.File.ReadAllText(jsonPath);

        // Deserialize JSON data into the MonoBehaviour
        T loadedMonoBehaviour = JsonUtility.FromJson<T>(jsonData);

        return loadedMonoBehaviour;
    }

    // Function to instantiate a GameObject and assign a loaded MonoBehaviour
    public GameObject InstantiateMonoBehaviour<T>(T loadedMonoBehaviour) where T : MonoBehaviour
    {
        // Load the prefab associated with the MonoBehaviour
        GameObject prefab = UnityEditor.PrefabUtility.LoadPrefabContents(UnityEditor.AssetDatabase.GetAssetPath(loadedMonoBehaviour));
        
        // Instantiate the prefab
        GameObject newObj = Instantiate(prefab);

        return newObj;
    }
}
