using UnityEditor;
using UnityEngine;

public class CreatePrefabs : MonoBehaviour
{
    [MenuItem("Tools/Create Prefabs from Meshes")]
    static void CreatePrefabsFromMeshes()
    {
        string meshFolderPath = "Assets/ithappy/Platformer_2_Obstacles/Meshes/Walls"; // Укажите путь к вашей папке с мешами
        string prefabFolderPath = "Assets/Prefabs"; // Укажите путь к папке Prefabs

        if (!AssetDatabase.IsValidFolder(prefabFolderPath))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }

        string[] meshGuids = AssetDatabase.FindAssets("t:Mesh", new[] { meshFolderPath });

        foreach (string meshGuid in meshGuids)
        {
            string meshPath = AssetDatabase.GUIDToAssetPath(meshGuid);
            GameObject meshObject = AssetDatabase.LoadAssetAtPath<GameObject>(meshPath);

            if (meshObject != null)
            {
                string prefabPath = prefabFolderPath + "/" + meshObject.name + ".prefab";
                PrefabUtility.SaveAsPrefabAsset(meshObject, prefabPath);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
