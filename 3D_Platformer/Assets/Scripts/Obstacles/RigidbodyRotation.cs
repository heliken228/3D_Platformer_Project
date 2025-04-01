using UnityEngine;

public class RigidbodyRotation : MonoBehaviour
{
    public float rotationSpeed = 50f; // Скорость вращения в градусах в секунду
    private Rigidbody rb;

    void Start()
    {
        // Получаем компонент Rigidbody
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            // Вычисляем новую ориентацию
            Quaternion deltaRotation = Quaternion.Euler(0, rotationSpeed * Time.fixedDeltaTime, 0);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
    }
}
