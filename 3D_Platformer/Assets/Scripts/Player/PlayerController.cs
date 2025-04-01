using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputSystem_Actions _inputSystem;
    private InputAction _movement;
    private InputAction _jump;
    private Vector2 _movementVector;
    
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _jumpHeight;
    [SerializeField] Camera _camera;

    [SerializeField] private AudioSource _jumpSound;

    private Rigidbody _rigidbody;
    private Vector3 _velocity;
    private bool _isGrounded;
    private Animator _animator;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    
    private RagdollController _ragdollController; 
    

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _inputSystem = new InputSystem_Actions();
        _ragdollController = GetComponentInChildren<RagdollController>();
    }

    private void OnEnable()
    {
        _inputSystem.Player.Enable();
        _movement = _inputSystem.Player.Move;
        _movement.Enable();
        
        _inputSystem.Player.Jump.performed += Jump;
        _inputSystem.Player.Jump.Enable();
    }

    private void OnDisable()
    {
        _inputSystem.Player.Jump.performed -= Jump;
        _movement.Disable();
        _inputSystem.Player.Jump.Disable();
    }

    private void OnDestroy()
    {
        _inputSystem.Player.Disable();
    }

    private void FixedUpdate()
    {
        if (_ragdollController != null && _ragdollController.IsRagdollActive)
        {
            // Если ragdoll активен, не разрешаем движение
            Debug.Log("Ragdoll активен, движение отключено.");
            return;
        }
        
        CheckGrounded();
        _movementVector = _movement.ReadValue<Vector2>();
        Move(_movementVector);
        Debug.Log(_isGrounded);
    }

    private void CheckGrounded()
    {
        _isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, groundDistance, groundMask);
        Debug.DrawRay(groundCheck.position, Vector3.down * groundDistance, Color.red);
        _animator.SetBool("IsGrounded", _isGrounded);
    }

    public void Move(Vector3 moveDirection)
    {
        // Получаем направление вперед и вправо камеры
        Vector3 forward = _camera.transform.forward;
        Vector3 right = _camera.transform.right;

        // Убираем компонент Y, чтобы движение было только по плоскости XZ
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // Вычисляем направление движения относительно камеры
        Vector3 desiredMoveDirection = forward * moveDirection.y + right * moveDirection.x;

        Vector3 targetVelocity = desiredMoveDirection * _speed;
        _rigidbody.linearVelocity = new Vector3(targetVelocity.x, _rigidbody.linearVelocity.y, targetVelocity.z);

        if (desiredMoveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(desiredMoveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
        _animator.SetBool("Run", moveDirection.magnitude > 0.1f);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (_ragdollController != null && _ragdollController.IsRagdollActive)
        {
            // Если ragdoll активен, не разрешаем прыжок
            Debug.Log("Ragdoll активен, прыжок отключен.");
            return;
        }
        
        if (_isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
            _rigidbody.linearVelocity = new Vector3(_rigidbody.linearVelocity.x, jumpVelocity, _rigidbody.linearVelocity.z);
            _animator.SetTrigger("Jump");
            _jumpSound.Play();
        }
    }
}
