using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class JiricButton : MonoBehaviour
{
    [SerializeField] private float _buttonMoveDistance = 0.1f;
    [SerializeField] private float _buttonMoveTime = 0.1f;
    [SerializeField] private AudioSource _jirircAudio;
    [SerializeField] private GameObject _pressFText;
    [SerializeField] private float _lookDistance = 3f; //Максимальная дистанция, на которую камера «видит» кнопку

    private bool _playerInTrigger = false;
    private bool _isMoving = false;
    private Transform _playerCamera;
    private Vector3 _initialButtonPosition;

    private void Start()
    {
        _initialButtonPosition = transform.localPosition;
        _jirircAudio = GetComponent<AudioSource>();
        _pressFText.SetActive(false);
        
        _playerCamera = Camera.main?.transform;
    }

    private void Update()
    {
        if (_playerInTrigger && IsPlayerLookingAtButton())
        {
            _pressFText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F) && !_isMoving)
            {
                StartCoroutine(PressButton());
            }
        }
        else
        {
            _pressFText.SetActive(false);
        }
    }

    private bool IsPlayerLookingAtButton() //Игрок смотрит на кнопку
    {
        if (_playerCamera == null) return false;

        Ray ray = new Ray(_playerCamera.position, _playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _lookDistance))
        {
            // Вернёт true, если игрок  смотрит на кнопку
            return hit.transform != transform;
        }
        
        return true;
    }

    private IEnumerator PressButton()
    {
        _isMoving = true;
        _jirircAudio.Play();

        Vector3 pressedPos = _initialButtonPosition - transform.right * _buttonMoveDistance;

        // НАЖАТИЕ НА КНОПКУ
        float elapsed = 0f;
        while (elapsed < _buttonMoveTime)
        {
            transform.localPosition = Vector3.Lerp(_initialButtonPosition, pressedPos, elapsed / _buttonMoveTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = pressedPos;
        
        // ВОЗВРАЩЕНИЕ КНОПКИ
        elapsed = 0f;
        while (elapsed < _buttonMoveTime)
        {
            transform.localPosition = Vector3.Lerp(pressedPos, _initialButtonPosition, elapsed / _buttonMoveTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = _initialButtonPosition;

        _isMoving = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInTrigger = false;
        }
    }
}
    

