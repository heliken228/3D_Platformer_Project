using System;
using System.Collections;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private float _jumpForce = 12f;
    [SerializeField] private AudioSource _otskokAudioSource;
    [SerializeField] private float _scaleMultiplierY = 3f;
    [SerializeField] private float _scaleDuration = 1f;
    
    private Vector3 _originalScale;
    
    private void Start()
    {
        _originalScale = transform.localScale;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _otskokAudioSource.Play();
            Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                playerRigidbody.linearVelocity = new Vector3(playerRigidbody.linearVelocity.x, 0, playerRigidbody.linearVelocity.z);
                playerRigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
            }
            StopAllCoroutines();
            StartCoroutine(ScaleYAnimation());
        }
    }
    private IEnumerator ScaleYAnimation()
    {
        float halfDuration = _scaleDuration / 2f;
        Vector3 targetScale = new Vector3(_originalScale.x, _originalScale.y * _scaleMultiplierY, _originalScale.z);

        // Увеличение
        float time = 0;
        while (time < halfDuration) // Пока время не достигло половины длительности
        {
            transform.localScale = Vector3.Lerp(_originalScale, targetScale, time / halfDuration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;

        // Уменьшение
        time = 0;
        while (time < halfDuration)
        {
            transform.localScale = Vector3.Lerp(targetScale, _originalScale, time / halfDuration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localScale = _originalScale;
    }
}
