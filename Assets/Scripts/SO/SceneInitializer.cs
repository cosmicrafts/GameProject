using UnityEngine;
using UnityEditor;

public class SceneInitializer : MonoBehaviour
{
    // Function to instantiate a GameObject and assign a loaded MonoBehaviour
    public GameObject InstantiateMonoBehaviour<T>(T loadedMonoBehaviour) where T : MonoBehaviour
    {
        // Instantiate the prefab associated with the MonoBehaviour
        GameObject prefab = UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource(loadedMonoBehaviour.gameObject);

        if (prefab != null)
        {
            // Instantiate the prefab
            GameObject newObj = Instantiate(prefab);
            return newObj;
        }
        else
        {
            Debug.LogError("Prefab not found for MonoBehaviour: " + typeof(T).Name);
            return null;
        }
    }
}
