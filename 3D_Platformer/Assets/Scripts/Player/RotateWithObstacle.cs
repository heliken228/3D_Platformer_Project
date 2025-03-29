using UnityEngine;
using Zenject;

public class RotateWithCylinder : MonoBehaviour
{
    [Inject] private PlayerController _player;
    private Rigidbody _cylinderRigidbody;
    private bool _isPlayerTouching;
    private Rigidbody _playerRigidbody;

    private void Start()
    {
        _cylinderRigidbody = GetComponent<Rigidbody>();
        _playerRigidbody = _player.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_isPlayerTouching && _playerRigidbody != null)
        {
            // Получаем угловую скорость цилиндра
            Vector3 angularVelocity = _cylinderRigidbody.angularVelocity;
            
            // Вычисляем тангенциальную скорость для игрока
            Vector3 playerOffset = _playerRigidbody.position - _cylinderRigidbody.position;
            Vector3 tangentialVelocity = Vector3.Cross(angularVelocity, playerOffset);
            
            // Добавляем эту скорость к игроку
            _playerRigidbody.linearVelocity += tangentialVelocity;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == _player.gameObject)
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
