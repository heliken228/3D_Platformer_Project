using UnityEngine;
public enum CollectibleType
{
    Coin, 
    Gem
}

public class BonusHandler : MonoBehaviour
{
    public static event OnColliderBonus OnBonusTake;
    
    public CollectibleType type;

    public delegate void OnColliderBonus(CollectibleType type);

    [SerializeField] private float _rotationSpeed = 100f;


    private void Update()
    {
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            if (type == CollectibleType.Coin)
            {
                OnBonusTake?.Invoke(type);
            }
            else if (type == CollectibleType.Gem)
            {
                OnBonusTake?.Invoke(type);
            }
        }
    }
}

