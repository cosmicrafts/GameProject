using UnityEngine;
using System.IO;

public class ListFilesInResources : MonoBehaviour
{
    void Start()
    {
        string[] files = Directory.GetFiles(Application.dataPath + "/Resources", "*.asset", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            Debug.Log("Found file: " + file);
        }
    }
}