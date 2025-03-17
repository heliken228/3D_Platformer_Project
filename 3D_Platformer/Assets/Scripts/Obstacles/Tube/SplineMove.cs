using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;
using Zenject;

public class SplineMove : MonoBehaviour
{
    [SerializeField] private SplineContainer _splineContainer;
    [SerializeField] private float _speed;
    [SerializeField] private float _distancePercent;
    [SerializeField] private Vector3 _playerScaleInTube = new Vector3(0.5f, 0.5f, 0.5f);
    
    private float _splineLength;
    private bool _isMoving = false;
    private Vector3 _initialScale;
    
    private PlayerController _playerController;
    private Animator _playerAnimator;
    
    [Inject]
    public void Construct(PlayerController playerController)
    {
        _playerController = playerController;
        _playerAnimator = playerController.GetComponentInChildren<Animator>();
        _initialScale = playerController.transform.localScale;
    }

    private void Start()
    {
        _splineLength = _splineContainer.CalculateLength();
    }

    private void Update()
    {
        if (_isMoving && _playerController != null)
        {
            _distancePercent += _speed * Time.deltaTime / _splineLength;
            _distancePercent = Mathf.Clamp01(_distancePercent);

            Vector3 currentPosition = _splineContainer.EvaluatePosition(_distancePercent);
            _playerController.transform.position = currentPosition;

            if (_distancePercent >= 1f)
            {
                _isMoving = false;
                _playerAnimator.SetBool("Fall", false);
                _playerController.transform.localScale = _initialScale;
            }
            else
            {
                _playerAnimator.SetBool("Fall", true);
            }

            /*Vector3 newPosition = _splineContainer.EvaluatePosition(_distancePercent + 0.05f);
            Vector3 direction = newPosition - currentPosition;
            _playerController.transform.rotation = Quaternion.LookRotation(direction, _playerController.transform.up);*/
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isMoving = true;
            _playerController.transform.localScale = _playerScaleInTube;
        }
    }

}

