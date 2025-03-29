using UnityEngine;
using Zenject;

public class RotateWithCylinder : MonoBehaviour
{
    [Inject] private PlayerController _player;
    private Rigidbody _cylinderRigidbody;
    private bool _isPlayerTouching;

    private void Start()
    {
        _cylinderRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_isPlayerTouching)
        {
            float angularSpeed = _cylinderRigidbody.angularVelocity.y;
            _player.transform.RotateAround(
                transform.position,
                transform.up,
                angularSpeed * Mathf.Rad2Deg * Time.fixedDeltaTime
            );
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == _player.gameObject) //проверяет, является ли объект, с которым столкнулся цилиндр, игроком
        {
            _isPlayerTouching = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == _player.gameObject)
        {
            _isPlayerTouching = false;
        }
    }
}
