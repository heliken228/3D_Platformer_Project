 using UnityEngine;

public class PlayerCylinderInteraction : MonoBehaviour
{
    private bool _isPlayerOnCylinder = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = transform;
            _isPlayerOnCylinder = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = null;
            _isPlayerOnCylinder = false;
        }
    }
}
