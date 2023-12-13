using UnityEngine;

public class ListSceneHierarchy : MonoBehaviour
{
    void Start()
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            Debug.Log("Found GameObject: " + obj.name);

            Component[] components = obj.GetComponents<Component>();

            foreach (Component component in components)
            {
                Debug.Log("  - Component: " + component.GetType().ToString());
            }
        }
    }
}
