using UnityEngine;

namespace platformerProject
{
    public class OscillatePosition : MonoBehaviour
    {
        [SerializeField] private Vector3 _moveAxis;
        [SerializeField] private float _speed;
        [SerializeField] private float _moveDistance;

        private Vector3 _startPosition;
        private bool _movingPositive = true;

        private void Start()
        {
            _startPosition = transform.position;
        }

        private void Update()
        {
            // Вычисляем направление
            Vector3 targetPosition = _startPosition;
            if (_movingPositive)
            {
                targetPosition += _moveAxis.normalized * _moveDistance;
            }
            else
            {
                targetPosition -= _moveAxis.normalized * _moveDistance;
            }
            
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
            
            if (transform.position == targetPosition)
            {
                _movingPositive = !_movingPositive;
            }
        }
    }
}
