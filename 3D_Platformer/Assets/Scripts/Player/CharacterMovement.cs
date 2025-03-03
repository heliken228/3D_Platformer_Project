using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _jumpHeight;
    
    [SerializeField] private AudioSource _jumpSound;
    
    private Rigidbody _rigidbody;
    private Vector3 _velocity;
    private bool _isGrounded;
    private Animator _animator;
    
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public Transform cameraTransform;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckGrounded();
        HandleJump();
    }
    
    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void CheckGrounded()
    {
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        Debug.DrawRay(groundCheck.position, Vector3.down * groundDistance, Color.red);
        _animator.SetBool("IsGrounded", _isGrounded);

        if (_isGrounded)
        {
            _animator.SetBool("Jump", false);
        }
    }

    private void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        Vector3 move = cameraTransform.right * x + cameraTransform.forward * z;  // Вычисление направления движения относительно камеры
        
        move.y = 0;   // Убираем вертикальное смещение, чтобы игрок не взлетал или не опускался
        
        Vector3 targetVelocity = move * _speed;
        _rigidbody.linearVelocity = new Vector3(targetVelocity.x, _rigidbody.linearVelocity.y, targetVelocity.z);
        
        if (move.magnitude > 0)    // Если игрок движется, поворачиваем его в направлении движения
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);    // Целевое вращение
            
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);   // Плавный поворот
        }

        _animator.SetBool("Run", move.magnitude > 0);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
            _rigidbody.linearVelocity = new Vector3(_rigidbody.linearVelocity.x, jumpVelocity, _rigidbody.linearVelocity.z);
            _animator.SetBool("Jump", true);
            _animator.SetBool("IsGrounded", false);
            _jumpSound.Play();
        }
    }
}
