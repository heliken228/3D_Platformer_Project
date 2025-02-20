using UnityEngine;

namespace platformerProject
{
    public class OscillateRotation : MonoBehaviour
    {
        [SerializeField] private Vector3 _rotationAxis;
        [SerializeField] private float _speed;
        [SerializeField] private float _rotationAngle;
        [SerializeField] private bool _loopRotate;
        [SerializeField] private bool _invertLoopRotate;

        private Quaternion _startRotation;
        private bool _rotatingPositive = true;

        private void Start()
        {
            _startRotation = transform.rotation;
        }

        private void Update()
        {
            if (_loopRotate)
            {
                // Режим бесконечного вращения
                float direction = _invertLoopRotate ? -1 : 1; // Определяем направление вращения -- если _invertLoopRotate TRUE, to direction = -1f, в ином случае - 1f
                transform.Rotate(_rotationAxis, direction * _speed * Time.deltaTime); // Вращаем объект
                _rotatingPositive = false;
            }
            else
            {
                // Режим колебательного вращения
                Quaternion targetRotation = _startRotation;
                if (_rotatingPositive)
                {
                    targetRotation = _startRotation * Quaternion.AngleAxis(_rotationAngle, _rotationAxis);
                }
                else
                {
                    targetRotation = _startRotation * Quaternion.AngleAxis(-_rotationAngle, _rotationAxis);
                }

                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _speed * Time.deltaTime);

                if (transform.rotation == targetRotation)
                {
                    _rotatingPositive = !_rotatingPositive;
                }
            }
        }
    }
}
