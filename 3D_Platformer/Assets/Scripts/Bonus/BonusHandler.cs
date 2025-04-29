using UnityEngine;
public enum CollectibleType
{
    Coin, 
    Gem,
    Star
}

public class BonusHandler : MonoBehaviour
{
    public static event OnColliderBonus OnBonusTake; //Событие, которое срабатывает, когда игрок подбирает бонус. 
    
    public CollectibleType type; //Переменная, которая хранит тип бонуса для данного объекта
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
            if (type == CollectibleType.Coin)
            {
                BonusCollectEffect.Instance.PlayBonusEffect(BonusType.Coin, transform.position); //вызов анимации эффектов
            }
            else if (type == CollectibleType.Gem)
            {
                BonusCollectEffect.Instance.PlayBonusEffect(BonusType.Gem, transform.position);
            }
            else if (type == CollectibleType.Star)
            {
                BonusCollectEffect.Instance.PlayBonusEffect(BonusType.Star, transform.position);
            }

            // Отключаем объект после подбора
            gameObject.SetActive(false);

            // Сигнализируем о сборе бонуса
            OnBonusTake?.Invoke(type);
        }
    }
}

