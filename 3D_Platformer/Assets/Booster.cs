using System;
using UnityEngine;

public class Booster : MonoBehaviour
{
    [SerializeField] private Vector3 _boostDirection = Vector3.forward;
    [SerializeField] private float _boostForce = 10f;
    [SerializeField] private AudioSource _BoostAudioSource;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Transform playerTransform = other.transform;
            if (playerTransform != null)
            {
                Vector3 newPosition = playerTransform.position + _boostDirection * _boostForce * Time.deltaTime;
                playerTransform.position = newPosition;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _BoostAudioSource.Play();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _BoostAudioSource.Stop();
        }
    }
}
