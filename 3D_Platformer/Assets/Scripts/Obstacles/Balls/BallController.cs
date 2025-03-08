using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private Transform _spawnPoint1;
    [SerializeField] private Transform _spawnPoint2;
    [SerializeField] private float _forceMagnitude = 2000f;
    [SerializeField] private Vector3 _forceDirection = Vector3.back;
    [SerializeField] private float _spawnInterval = 5f;

    private BallPoolObject _poolObject;
    private float _timeSinceLastSpawn = 0f;

    private void Start()
    {
        _poolObject = new BallPoolObject(_ballPrefab, 10);
    }

    private void Update()
    {
        _timeSinceLastSpawn += Time.deltaTime;

        if (_timeSinceLastSpawn >= _spawnInterval)
        {
            SpawnBall();
            _timeSinceLastSpawn = 0f;
        }
    }

    private void SpawnBall()
    {
        GameObject ball = _poolObject.GetObject();

        // Выбираем случайную точку спавна
        Transform spawnPoint = Random.value < 0.5f ? _spawnPoint1 : _spawnPoint2;
        ball.transform.position = spawnPoint.position;
        ball.SetActive(true);

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
