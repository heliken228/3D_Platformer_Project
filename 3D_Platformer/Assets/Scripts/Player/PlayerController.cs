using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

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

    [SerializeField] private AudioSource _jumpSound;
    [SerializeField] private AudioSource _screamSound;
    [SerializeField] private AudioSource _hitSound;
    [SerializeField] private AudioSource _kickSound;
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private float _boostSpeed;
    [SerializeField] private float _animationSpeedMultiplier = 1.5f;
    
    private Vector3 _startPosition;
    private Rigidbody _rigidbody;
    private Vector3 _velocity;
    private bool _isGrounded;
    private Animator _animator;
    private Camera _camera;
    private bool _isRagdollActive = false;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public Transform CameraTarget => _cameraTarget;
    
    private RagdollController _ragdollController;
    
    [Inject]
    public void Construct(Camera camera)
    {
        _camera = camera;
    }
    
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _inputSystem = new InputSystem_Actions();
        _ragdollController = GetComponentInChildren<RagdollController>();
        _startPosition = transform.position;
        _movement = _inputSystem.Player.Move;
    }

    private void OnEnable()
    {
        _inputSystem.Player.Enable(); // Включаем всю схему Player
        _inputSystem.Player.Jump.performed += Jump; // Подписываемся на событие
    }

    private void OnDisable()
    {
        _inputSystem.Player.Jump.performed -= Jump; // Отписываемся от события
        _inputSystem.Player.Disable(); // Отключаем всю схему Player (включая Move, Jump, Sprint)
    }

    private void OnDestroy()
    {
        _inputSystem.Disable();
        _inputSystem.Dispose();
    }

    private void FixedUpdate()
    {
        if (_ragdollController != null && _ragdollController.IsRagdollActive)
        {
            return;
        }
        
        CheckGrounded();
        _movementVector = _movement.ReadValue<Vector2>();
        Move(_movementVector);
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

        // Определяем текущую скорость
        float currentSpeed = _inputSystem.Player.Sprint.IsPressed() ? _boostSpeed : _speed;

        Vector3 targetVelocity = desiredMoveDirection * currentSpeed;
        _rigidbody.linearVelocity = new Vector3(targetVelocity.x, _rigidbody.linearVelocity.y, targetVelocity.z);

        if (desiredMoveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(desiredMoveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }

        // Устанавливаем параметры анимации
        _animator.SetBool("Run", moveDirection.magnitude > 0.1f);
        _animator.SetBool("IsBoosting", _inputSystem.Player.Sprint.IsPressed());

        // Увеличиваем скорость анимации при ускорении
        if (_inputSystem.Player.Sprint.IsPressed())
        {
            _animator.speed = _animationSpeedMultiplier;
        }
        else
        {
            _animator.speed = 1.0f;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (_ragdollController != null && _ragdollController.IsRagdollActive)
        {
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathZone"))
        {
            Die();
        }

        if (other.CompareTag("ScreamZone"))
        {
            _screamSound.Play();
            _animator.SetTrigger("Fall");
        }

        if (other.CompareTag("Hammer"))
        {
            _kickSound.Play();
            _hitSound.Play();
            Hit();
        }

        if (other.CompareTag("Glove") && !_isRagdollActive)
        {
            GloveKick gloveKick = other.GetComponentInParent<GloveKick>();
            
            if (gloveKick != null)
            {
                Vector3 kickDirection = gloveKick.GloveKickDirection;

                _kickSound.Play();
                _hitSound.Play();
                _animator.SetTrigger("Fall");
                _rigidbody.AddForce(kickDirection * 5000f, ForceMode.Impulse);
            }
            else
            {
                Debug.LogWarning("GloveKick не найден в родителях объекта с тегом 'Glove'");
            }
            
            
            
            /*if (_ragdollController != null)
            {
                _ragdollController.SetRagdoll(true);
                _inputSystem.Disable();
                _isRagdollActive = true;
                StartCoroutine(RespawnAfterDelay(2f));
            }*/
            
        }
    }
    private IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Respawn();
    }

    private void Hit()
    {
        _animator.SetTrigger("Hit");
        
        bool isGameOver = BonusService.Instance.LoseHalfHeart();

        if (isGameOver)
        {
            SceneService.Instance.ShowGameOver();
        }
    }

    private void Die()
    {
        bool IsDie = BonusService.Instance.LoseHeart();
        
        Respawn();

        if (IsDie)
        {
            SceneService.Instance.ShowGameOver();
        }
    }

    private void Respawn()
    {
        _ragdollController.SetRagdoll(false);
        _isRagdollActive = false;
        _inputSystem.Enable();
        transform.position = _startPosition;
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }
}
