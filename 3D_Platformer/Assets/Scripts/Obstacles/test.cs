using UnityEngine;

public class test : MonoBehaviour
{
    public float rotationSpeed = 50f; // Скорость вращения в градусах в секунду
    private Rigidbody rb;

    void Start()
    {
        // Получаем компонент Rigidbody
        rb = GetComponent<Rigidbody>();

        // Убедитесь, что Rigidbody не является кинематическим
        if (rb != null)
        {
            rb.isKinematic = true;
        }
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
