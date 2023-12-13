using UnityEngine;

public class MonoDataLoader : MonoBehaviour
{
    // Function to load any MonoBehaviour from a JSON file
    public T LoadMonoBehaviour<T>(string jsonPath) where T : MonoBehaviour
    {
        // Load JSON data from the file
        string jsonData = System.IO.File.ReadAllText(jsonPath);

        // Create an empty GameObject
        GameObject newObj = new GameObject();

        // Add the component of type T to the new GameObject
        T loadedMonoBehaviour = newObj.AddComponent<T>();

        // Deserialize JSON data into the MonoBehaviour
        JsonUtility.FromJsonOverwrite(jsonData, loadedMonoBehaviour);

        return loadedMonoBehaviour;
    }

    // Function to instantiate a GameObject and assign a loaded MonoBehaviour
    public GameObject InstantiateMonoBehaviour<T>(T loadedMonoBehaviour) where T : MonoBehaviour
    {
        // Instantiate a new GameObject
        GameObject newObj = Instantiate(new GameObject(loadedMonoBehaviour.name));

        // Copy the loaded MonoBehaviour's components to the new GameObject
        CopyComponent(loadedMonoBehaviour, newObj.AddComponent<T>());

        return newObj;
    }

    // Helper method to copy components from one MonoBehaviour to another
    private void CopyComponent<T>(T original, T destination) where T : Component
    {
        // Copy the public properties or fields as needed
        // Example: destination.Property = original.Property;
    }
}
