using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class HelpCircleWay : MonoBehaviour
{
    [SerializeField] private float _speed = 0.5f;
    [SerializeField] private float _rotationAmplitude = 10f;
    [SerializeField] private float _rotationFrequency = 0.5f;
    [SerializeField] private Vector3 _movementDirection = Vector3.back;
    
    private Rigidbody rb;
    private float time;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Движение в заданном направлении
        rb.linearVelocity = _movementDirection * _speed;

        time += Time.fixedDeltaTime;
        float rotation = Mathf.Sin(time * _rotationFrequency) * _rotationAmplitude * Time.fixedDeltaTime;
        transform.Rotate(0, rotation, 0);
    }
}
