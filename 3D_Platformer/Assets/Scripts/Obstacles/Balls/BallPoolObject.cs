using System.Collections.Generic;
using UnityEngine;

public class BallPoolObject : MonoBehaviour
{
    [SerializeField] private GameObject _pooledObjectPrefab;
    private List<GameObject> _pooledObjects;
    private int _poolSize = 10;

    public BallPoolObject(GameObject prefab, int size)
    {
        _pooledObjectPrefab = prefab;
        _poolSize = size;
        _pooledObjects = new List<GameObject>();

        for (int i = 0; i < _poolSize; i++)
        {
            CreateNewObject();
        }
    }

    public GameObject GetObject()
    {
        foreach (GameObject obj in _pooledObjects)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        // Если все объекты активны, создаем новый
        return CreateNewObject();
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    private GameObject CreateNewObject()
    {
        GameObject newObj = GameObject.Instantiate(_pooledObjectPrefab);
        newObj.SetActive(false);
        _pooledObjects.Add(newObj);
        return newObj;
    }
}
