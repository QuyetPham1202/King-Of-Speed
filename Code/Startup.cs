using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startup 
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void InstantiatePrefabs()
    {
        Debug.Log("--- Initializing Prefabs ---");

        // Load all prefabs from the "Resources/InstantiateOnLoad/" directory
        GameObject[] prefabsToInstantiate = Resources.LoadAll<GameObject>("InstantiateOnLoad/");

        // Instantiate each prefab
        foreach (GameObject prefab in prefabsToInstantiate)
        {
            Debug.Log($"Creating {prefab.name}");
            GameObject.Instantiate(prefab);
        }

        Debug.Log("--- Initialization Complete ---");
    }
}
