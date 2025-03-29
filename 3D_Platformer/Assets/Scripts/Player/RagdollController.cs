using UnityEngine;
using UnityEngine.InputSystem;

public class RagdollController : MonoBehaviour
{
    private InputSystem_Actions _inputSystem;
    
    private Animator _animator;
    private Rigidbody[] _rigidbodies;
    private Collider[] _colliders;
    private bool _isRagdollActive = false;
    
    public bool IsRagdollActive => _isRagdollActive; // Публичное свойство для доступа к состоянию ragdoll

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        _colliders = GetComponentsInChildren<Collider>();
        _inputSystem = new InputSystem_Actions();
        
        SetRagdoll(false);
    }

    private void OnEnable()
    {
        if (_inputSystem == null)
            _inputSystem = new InputSystem_Actions();

        _inputSystem.Player.Ragdoll.performed += ToggleRagdoll;
        _inputSystem.Player.Ragdoll.Enable();
    }

    private void OnDisable()
    {
        if (_inputSystem != null)
        {
            _inputSystem.Player.Ragdoll.performed -= ToggleRagdoll;
            _inputSystem.Player.Ragdoll.Disable();
        }
    }

    private void OnDestroy()
    {
        if (_inputSystem != null)
        {
            _inputSystem.Player.Disable();
            _inputSystem.Disable();
            _inputSystem = null;
        }
    }

    private void ToggleRagdoll(InputAction.CallbackContext context)
    {
        _isRagdollActive = !_isRagdollActive;
        SetRagdoll(_isRagdollActive);
    }

    private void SetRagdoll(bool active)
    {
        _animator.enabled = !active;

        foreach (var rb in _rigidbodies)
        {
            rb.isKinematic = !active;
        }

        foreach (var col in _colliders)
        {
            col.enabled = active;
        }
    }
}
