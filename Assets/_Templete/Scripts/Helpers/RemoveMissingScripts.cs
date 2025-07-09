using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
public class RemoveMissingScripts : MonoBehaviour
{

    [MenuItem("Tools/Remove Missing Scripts from Open Prefab")]
    static void RemoveMissingScriptsFromOpenPrefab()
    {
        // Check if a prefab is open in Prefab Mode
        PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
        if (prefabStage == null)
        {
            Debug.LogWarning("No prefab is currently open in Prefab Mode.");
            return;
        }

        // Get the root GameObject of the open prefab
        GameObject prefabRoot = prefabStage.prefabContentsRoot;

        int count = 0;

        // Queue of GameObjects to process
        Queue<GameObject> toProcess = new Queue<GameObject>();
        toProcess.Enqueue(prefabRoot);

        // Process the queue
        while (toProcess.Count > 0)
        {
            GameObject go = toProcess.Dequeue();
            SerializedObject serializedObject = new SerializedObject(go);
            SerializedProperty prop = serializedObject.FindProperty("m_Component");

            // Track the original size of the component array
            int componentCount = prop.arraySize;

            // Iterate through each component and check if it's null
            for (int i = componentCount - 1; i >= 0; i--)
            {
                SerializedProperty component = prop.GetArrayElementAtIndex(i);
                if (component.objectReferenceValue == null)
                {
                    // Remove the missing script
                    prop.DeleteArrayElementAtIndex(i);
                    count++;
                }
            }

            serializedObject.ApplyModifiedProperties();

            // Enqueue child GameObjects for processing
            foreach (Transform child in go.transform)
            {
                toProcess.Enqueue(child.gameObject);
            }
        }

        // Save changes to the prefab
        PrefabUtility.SaveAsPrefabAsset(prefabRoot, prefabStage.prefabAssetPath);
        EditorSceneManager.MarkSceneDirty(prefabRoot.scene);

        Debug.Log($"Removed {count} missing scripts from the open prefab.");
    }

    [MenuItem("Tools/Remove Missing Scripts")]
    static void RemoveMissingScriptsFromScene()
    {
        // Get all GameObjects in the scene
        GameObject[] gameObjects = FindObjectsOfType<GameObject>();

        int count = 0;

        // Iterate through each GameObject
        foreach (GameObject go in gameObjects)
        {
            // Get all components attached to the GameObject
            Component[] components = go.GetComponents<Component>();
            SerializedObject serializedObject = new SerializedObject(go);
            SerializedProperty prop = serializedObject.FindProperty("m_Component");

            // Iterate through each component and check if it's null
            for (int i = components.Length - 1; i >= 0; i--)
            {
                if (components[i] == null)
                {
                    // Remove the missing script
                    Debug.LogWarning($"Removing missing script from GameObject: {go.name}");
                    prop.DeleteArrayElementAtIndex(i);
                    count++;
                }
            }

            // Apply changes
            serializedObject.ApplyModifiedProperties();
        }

        Debug.Log($"Removed {count} missing scripts.");
    }
}
#endif