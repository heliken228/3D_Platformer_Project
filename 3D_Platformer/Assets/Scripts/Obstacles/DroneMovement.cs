using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    [SerializeField] private Transform _drone;
    [SerializeField] private float _rotationSpeed = 2.0f;
    [SerializeField] private float _maxRotationAngle = 5.0f;
    [SerializeField] private float _verticalSpeed = 0.5f;
    [SerializeField] private float _verticalAmplitude = 0.5f;

    private Vector3 _initialPosition;
    private Vector3 _initialRotation;

    private void Start()
    {
        // Сохраняем начальную позицию дрона
        _initialPosition = _drone.position;
        _initialRotation = _drone.eulerAngles;
    }

    private void Update()
    {
        float rotationX = _initialRotation.x + Mathf.Sin(Time.time * _rotationSpeed) * _maxRotationAngle;
        float rotationY = _initialRotation.y + Mathf.Sin(Time.time * _rotationSpeed) * _maxRotationAngle;
        float rotationZ = _initialRotation.z + Mathf.Sin(Time.time * _rotationSpeed) * _maxRotationAngle;
        _drone.eulerAngles = new Vector3(rotationX, rotationY, rotationZ);

        // Движение вверх-вниз
        float newY = _initialPosition.y + Mathf.Sin(Time.time * _verticalSpeed) * _verticalAmplitude;
        _drone.position = new Vector3(_drone.position.x, newY, _drone.position.z);
    }
}
