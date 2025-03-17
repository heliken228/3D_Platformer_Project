using System;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private float _jumpForce = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                playerRigidbody.linearVelocity = new Vector3(playerRigidbody.linearVelocity.x, 0, playerRigidbody.linearVelocity.z);
                playerRigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
            }
        }
    }
}
