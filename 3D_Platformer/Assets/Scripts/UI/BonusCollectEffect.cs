using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

public enum BonusType
{
    Coin,
    Gem,
    Star
}
public class BonusCollectEffect : MonoBehaviour
{
    public static BonusCollectEffect Instance { get; private set; }
    
    [SerializeField] private int _poolSize = 5;
    [SerializeField] private float _animationDuration = 1f;
    [SerializeField] private float _spread = 100f;
    [SerializeField] private float _scaleEndValue = 0.5f;

    private Transform _bonusEffectCanvas;
    
    private Dictionary<BonusType, Sprite> _bonusSprites; //Словарь, хранящий спрайты для каждого типа бонуса
    private Dictionary<BonusType, Transform> _bonusTargets; //Словарь, хранящий цели для каждого типа бонуса
    private Dictionary<BonusType, GameObject[]> _bonusPools; //Словарь, содержащий пулы объектов для каждого типа бонуса

    [Inject]
    public void Construction(BonusEffectProjectContext bonusEffectProjectContext)
    {
        _bonusEffectCanvas = bonusEffectProjectContext.BonusEffectCanvas;

        _bonusSprites = new Dictionary<BonusType, Sprite>
        {
            { BonusType.Coin, bonusEffectProjectContext.CoinImage.sprite },
            { BonusType.Gem, bonusEffectProjectContext.GemImage.sprite },
            { BonusType.Star, bonusEffectProjectContext.StarImage.sprite }
        };

        _bonusTargets = new Dictionary<BonusType, Transform>
        {
            { BonusType.Coin, bonusEffectProjectContext.CoinTarget },
            { BonusType.Gem, bonusEffectProjectContext.GemTarget },
            { BonusType.Star, bonusEffectProjectContext.StarTarget }
        };

        InitializePools(bonusEffectProjectContext);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePools(BonusEffectProjectContext context)
    {
        _bonusPools = new Dictionary<BonusType, GameObject[]>();

        foreach (BonusType bonusType in System.Enum.GetValues(typeof(BonusType)))
        {
            GameObject[] pool = new GameObject[_poolSize];
            for (int i = 0; i < _poolSize; i++)
            {
                GameObject bonusObj = new GameObject($"{bonusType}_{i}");
                bonusObj.transform.SetParent(_bonusEffectCanvas);

                Image img = bonusObj.AddComponent<Image>();
                img.sprite = _bonusSprites[bonusType];

                // Настроим размер под каждый бонус, если нужно
                if (bonusType == BonusType.Coin)
                    img.rectTransform.sizeDelta = context.CoinImage.rectTransform.sizeDelta;
                else if (bonusType == BonusType.Gem)
                    img.rectTransform.sizeDelta = context.GemImage.rectTransform.sizeDelta;
                else if (bonusType == BonusType.Star)
                    img.rectTransform.sizeDelta = context.StarImage.rectTransform.sizeDelta;

                img.raycastTarget = false;
                bonusObj.SetActive(false);
                pool[i] = bonusObj;
            }
            _bonusPools[bonusType] = pool;
        }
    }

    public void PlayBonusEffect(BonusType bonusType, Vector3 worldPosition)
    {
        GameObject bonus = GetPooledObject(bonusType);
        if (bonus == null) return;

        RectTransform bonusRect = bonus.GetComponent<RectTransform>();
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        bonusRect.position = screenPosition;
        bonusRect.localScale = Vector3.one;
        bonus.SetActive(true);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(bonusRect.DOAnchorPos(
            new Vector2(
                Random.Range(-_spread, _spread),
                Random.Range(-_spread, _spread)
            ),
            _animationDuration * 0.3f
        ).SetRelative(true));

        sequence.Append(bonusRect.DOMove(_bonusTargets[bonusType].position, _animationDuration * 0.7f));
        sequence.Join(bonusRect.DOScale(_scaleEndValue, _animationDuration * 0.7f));

        sequence.OnComplete(() => {
            bonus.SetActive(false);
        });
    }

    private GameObject GetPooledObject(BonusType bonusType)
    {
        foreach (var bonus in _bonusPools[bonusType])
        {
            if (!bonus.activeInHierarchy)
                return bonus;
        }
        Debug.LogWarning($"{bonusType} pool is empty! Consider increasing pool size.");
        return null;
    }
}

