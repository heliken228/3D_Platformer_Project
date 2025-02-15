using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _jumpHeight;
    
    private CharacterController _characterController;
    private Vector3 _velocity;
    private bool _isGrounded;
    
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        CheckGrounded();
        HandleMovement();
        HandleJump();
        ApplyGravity();
    }

    private void CheckGrounded()
    {
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        Debug.DrawRay(groundCheck.position, Vector3.down * groundDistance, Color.red);
    }

    private void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Вычисление направления движения
        Vector3 move = transform.right * x + transform.forward * z;

        // Применение движения
        _characterController.Move(move * _speed * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }
    }

    private void ApplyGravity()
    {
        _velocity.y += _gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }
}
