using UnityEngine;

public class CoinRotator : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 100f;
    public CollectibleType type;

    void Update()
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
                CoinCounter.Instance.AddCoin();
            }
            else if (type == CollectibleType.Gem)
            {
                CoinCounter.Instance.AddDiamond();
            }
        }
    }
}
public enum CollectibleType
{
    Coin,
    Gem
}
