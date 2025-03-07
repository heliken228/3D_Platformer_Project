using UnityEngine;
using Zenject;

public class InputController : MonoBehaviour
{
    private CharacterMovement _characterMovement;
    public Transform cameraTransform;

    private void Start()
    {
        _characterMovement = GetComponent<CharacterMovement>();
        cameraTransform = GameObject.Find("FreeLook Camera").transform;
    }
    
    private void Update()
    {
        HandleMovementInput();
        HandleJumpInput();
    }
    
    /*[Inject]
    private void Construct(CharacterMovement characterMovement)
    {
        _characterMovement = characterMovement;
    }*/

    private void HandleMovementInput()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = cameraTransform.right * x + cameraTransform.forward * z;
        move.y = 0;

        _characterMovement.Move(move);
    }

    private void HandleJumpInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _characterMovement.Jump();
        }
    }
}
