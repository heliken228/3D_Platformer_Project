using System.Collections;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private float _forceMagnitude = 2000f;
    [SerializeField] private Vector3 _forceDirection = Vector3.back;
    [SerializeField] private float _spawnInterval = 5f;

    private BallPoolObject _poolObject;

    private void Start()
    {
        _poolObject = GetComponent<BallPoolObject>();
        StartCoroutine(SpawnBallsRoutine());
    }

    private IEnumerator SpawnBallsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnInterval);
            SpawnBall();
        }
    }

    private void SpawnBall()
    {
        GameObject ball = _poolObject.GetObject();
        
        // Применяем силу к шару
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(_forceDirection * _forceMagnitude, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("Rigidbody component is missing from the ball prefab.");
        }
    }

    private void HandleBallCollision(GameObject ball)
    {
        // Возвращаем шар в пул или уничтожаем его
        _poolObject.ReturnObject(ball);
    }
}
