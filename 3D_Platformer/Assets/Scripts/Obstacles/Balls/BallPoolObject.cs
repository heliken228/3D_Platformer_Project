using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPoolObject : MonoBehaviour
{
    [SerializeField] private GameObject _pooledObjectPrefab;
    [SerializeField] private Transform _spawnPoint1;
    [SerializeField] private Transform _spawnPoint2;
    private List<GameObject> _pooledObjects;
    private int _poolSize = 10;

    private void Awake()
    {
        _pooledObjects = new List<GameObject>();

        for (int i = 0; i < _poolSize; i++)
        {
            GameObject ball = Instantiate(_pooledObjectPrefab);
            ball.SetActive(false);
            _pooledObjects.Add(ball);
        }
    }

    public GameObject GetObject()
    {
        foreach (GameObject obj in _pooledObjects)
        {
            if (!obj.activeInHierarchy)
            {
                // Выбираем случайную точку спавна
                Transform spawnPoint = Random.value < 0.5f ? _spawnPoint1 : _spawnPoint2;
                obj.transform.position = spawnPoint.position;
                obj.SetActive(true);
                StartCoroutine(ReturnToPoolAfterDelay(obj, 10f));
                return obj;
            }
        }

        // Если все объекты активны, создаем новый
        return CreateNewObject();
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.rotation = Quaternion.identity;
        
    }

    private GameObject CreateNewObject()
    {
        GameObject newObj = GameObject.Instantiate(_pooledObjectPrefab);
        newObj.SetActive(false);
        _pooledObjects.Add(newObj);
        return newObj;
    }
    
    private IEnumerator ReturnToPoolAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnObject(obj);
    }
}
