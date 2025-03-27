using UnityEngine;
using Zenject;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;

    void Start()
    {
        // Вычисляем смещение камеры относительно игрока
        _offset = transform.position - transform.parent.position;
    }

    void LateUpdate()
    {
        // Обновляем позицию камеры, чтобы она следовала за игроком
        transform.position = transform.parent.position + _offset;

        // Отключаем вращение камеры (оставляем только вращение по оси Z, если нужно)
        transform.rotation = Quaternion.Euler(30f, 0, transform.rotation.eulerAngles.z);
    }
}
