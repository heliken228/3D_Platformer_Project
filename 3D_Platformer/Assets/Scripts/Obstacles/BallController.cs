using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _forceMagnitude = 10f;
    [SerializeField] private Vector3 _forceDirection = Vector3.back;

    private BallPoolObject _poolObject;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _poolObject = new BallPoolObject(_ballPrefab, 10);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnBall();
        }
    }

    private void SpawnBall()
    {
        GameObject ball = _poolObject.GetObject();
        ball.transform.position = _spawnPoint.position;
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
