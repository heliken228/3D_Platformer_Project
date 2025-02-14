using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Rooms.Scripts
{
    public static class RoomBuilder
    {
        private const string RootObject = "Platformer_2_Obstacles";
        private const string PrefabsFolder = "Assets/Prefabs";

        [MenuItem("Tools/Build Rooms")]
        public static void Build()
        {
            var rootObject = GameObject.Find(RootObject).transform;

            var guids = AssetDatabase.FindAssets("t:prefab", new[] { PrefabsFolder });
            var paths = guids.Select(AssetDatabase.GUIDToAssetPath).ToList();
            var prefabsRoot = new GameObject(RootObject).transform;
            prefabsRoot.position += Vector3.right * 120;

            foreach (Transform child in rootObject)
            {
                if (!TryGetPrefab(child.name, out var prefab))
                {
                    continue;
                }

                var sceneObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab, prefabsRoot);
                sceneObject.transform.localPosition = child.position;
                sceneObject.transform.rotation = child.rotation;
                sceneObject.transform.localScale = new Vector3(child.localScale.x, Mathf.Abs(child.localScale.z), Mathf.Abs(child.localScale.y));
            }

            bool TryGetPrefab(string name, out GameObject prefab)
            {
                var source = paths.Where(path =>
                {
                    var p = path.Split('/').Last();
                    var prefabName = p[..p.LastIndexOf('.')];

                    return name.StartsWith(prefabName);
                }).FirstOrDefault();

                if (source == null)
                {
                    Debug.LogWarning($"Can't find prefab for: {name}");

                    prefab = null;
                    return false;
                }

                prefab = AssetDatabase.LoadAssetAtPath<GameObject>(source);
                return true;
            }
        }
    }
}