using TMPro;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private TextMeshProUGUI _gemText;

    private int _coinCount = 0;
    private int _gemCount = 0;
    
    /*Статическая переменная, которая хранит единственный экземпляр этого класса.
    Это используется для реализации паттерна Singleton,
    чтобы гарантировать, что в сцене будет только один CoinCounter.*/
    public static CoinCounter Instance;

    /*Здесь проверяется, существует ли уже экземпляр CoinCounter.
     Если нет, то текущий объект становится экземпляром. 
     Если экземпляр уже существует, текущий объект уничтожается, 
     чтобы избежать дублирования.*/
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateCoinText();
        UpdateGemText();
    }

    public void AddCoin()
    {
        _coinCount++;
        UpdateCoinText();
    }
    public void AddDiamond()
    {
        _gemCount++;
        UpdateGemText();
    }
    private void UpdateCoinText()
    {
        _coinText.text = _coinCount.ToString();
    }
    private void UpdateGemText()
    {
        _gemText.text = _gemCount.ToString();
    }
}
