using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

public class BonusCollectEffect : MonoBehaviour
{
    public static BonusCollectEffect Instance { get; private set; }
    
    [SerializeField] private int _poolSize = 5;
    [SerializeField] private float _animationDuration = 1f;
    [SerializeField] private float _spread = 100f;
    [SerializeField] private float _scaleEndValue = 0.5f;

    private Transform _bonusEffectCanvas;
    private Image _coinImage;
    private Transform _coinTarget;
    private GameObject[] _coinPool;

    [Inject]

    public void Construction(BonusEffectProjectContext bonusEffectProjectContext)
    {
        _bonusEffectCanvas = bonusEffectProjectContext.BonusEffectCanvas;
        _coinImage = bonusEffectProjectContext.CoinImage;
        _coinTarget = bonusEffectProjectContext.CoinTarget;
        
        InitializePool();
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Делаем этот объект доступным из других скриптов
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePool()
    {
        _coinPool = new GameObject[_poolSize];
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject coinObj = new GameObject($"Coin_{i}");
            coinObj.transform.SetParent(_bonusEffectCanvas);
            
            Image img = coinObj.AddComponent<Image>();
            img.sprite = _coinImage.sprite;
            img.rectTransform.sizeDelta = _coinImage.rectTransform.sizeDelta;
            img.raycastTarget = false;
            
            coinObj.SetActive(false);
            _coinPool[i] = coinObj;
        }
    }

    public void PlayCoinEffect(Vector3 worldPosition)
    {
        GameObject coin = GetPooledObject();
        if (coin == null) return;

        RectTransform coinRect = coin.GetComponent<RectTransform>();
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        coinRect.position = screenPosition;
        coinRect.localScale = Vector3.one;
        coin.SetActive(true);

        Sequence sequence = DOTween.Sequence();
        
        // 1. Небольшой "прыжок" в случайном направлении
        sequence.Append(coinRect.DOAnchorPos(
            new Vector2(
                Random.Range(-_spread, _spread), 
                Random.Range(-_spread, _spread)
            ), 
            _animationDuration * 0.3f
        ).SetRelative(true));
        
        // 2. Перелет к цели
        sequence.Append(coinRect.DOMove(_coinTarget.position, _animationDuration * 0.7f));
        
        // 3. Уменьшение размера во время перелета
        sequence.Join(coinRect.DOScale(_scaleEndValue, _animationDuration * 0.7f));
        
        sequence.OnComplete(() => {
            coin.SetActive(false);
        });
    }

    private GameObject GetPooledObject() //Получение монетки из пула
    {
        foreach (var coin in _coinPool)
        {
            if (!coin.activeInHierarchy)
                return coin;
        }
        Debug.LogWarning("Coin pool is empty! Consider increasing pool size.");
        return null;
    }
}

