using UnityEngine;

public class BonusHandler : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 100f;
    public CollectibleType type;

    public delegate void OnColliderBonus(CollectibleType type);
    public static event OnColliderBonus OnBonusTake;

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
public enum CollectibleType
{
    Coin,
    Gem
}
