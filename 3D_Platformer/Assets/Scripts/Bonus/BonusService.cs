using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonusService : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private TextMeshProUGUI _gemText;
    [SerializeField] private AudioSource _coinSound;
    [SerializeField] private AudioSource _gemSound;
    [SerializeField] private GameObject[] _hearts;
    [SerializeField] private Sprite _halfHeart;
    [SerializeField] private Sprite _emptyHeart;

    private int _coinCount = 0;
    private int _gemCount = 0;
    private int _currentHeartIndex = 0;
    private bool _hasHalfHeart = false;
    
    public static BonusService Instance;
    
    void Awake()
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

    private void OnEnable()
    {
        BonusHandler.OnBonusTake += HandleCollider;
    }

    private void OnDisable()
    {
        BonusHandler.OnBonusTake -= HandleCollider;
    }

    private void Start()
    {
        UpdateCoinText();
        UpdateGemText();
    }

    public void HandleCollider(CollectibleType collectibleType)
    {
        if (collectibleType == CollectibleType.Coin)
        {
            _coinCount++;
            UpdateCoinText();
            _coinSound.Play();
        }

        if (collectibleType == CollectibleType.Gem)
        {
            _gemCount++;
            UpdateGemText();
            _gemSound.Play();
        }
        if (collectibleType == CollectibleType.Star)
        {
            _gemSound.Play();
        }
        
    }
    private void UpdateCoinText()
    {
        _coinText.text = _coinCount.ToString();
    }
    private void UpdateGemText()
    {
        _gemText.text = _gemCount.ToString();
    }
    
    public bool LoseHeart()
    {
        if (_hasHalfHeart)
        {
            // Если уже есть половинка сердца, превращаем ее в пустое
            Image heartImage = _hearts[_currentHeartIndex].GetComponent<Image>();
            if (heartImage != null)
            {
                heartImage.transform.DOPunchScale(Vector3.one * 1.3f, 0.3f, vibrato: 1, elasticity: 0.5f).OnComplete(() => 
                {
                    heartImage.sprite = _emptyHeart;
                });
            }
            _hasHalfHeart = false;
            _currentHeartIndex++;
        }
        else if (_currentHeartIndex < _hearts.Length)
        {
            // отнимаем сердце
            Image heartImage = _hearts[_currentHeartIndex].GetComponent<Image>();
            if (heartImage != null)
            {
                heartImage.transform.DOPunchScale(Vector3.one * 1.3f, 0.3f, vibrato: 1, elasticity: 0.5f).OnComplete(() => 
                {
                    heartImage.sprite = _emptyHeart;
                });
            }
            _currentHeartIndex++;
        }
        return IsGameOver();
    }
    
    public bool LoseHalfHeart()
    {
        if (_hasHalfHeart)
        {
            // Если уже есть половинка, превращаем ее в пустое
            Image heartImage = _hearts[_currentHeartIndex].GetComponent<Image>();
            if (heartImage != null)
            {
                heartImage.transform.DOPunchScale(Vector3.one * 1.3f, 0.3f, vibrato: 1, elasticity: 0.5f).OnComplete(() =>
                {
                    heartImage.sprite = _emptyHeart;;
                });
            }
            _hasHalfHeart = false;
            _currentHeartIndex++;
        }
        else if (_currentHeartIndex < _hearts.Length)
        {
            // Создаем половинку сердца
            Image heartImage = _hearts[_currentHeartIndex].GetComponent<Image>();
            if (heartImage != null)
            {
                heartImage.transform.DOPunchScale(Vector3.one * 1.3f, 0.3f, vibrato: 1, elasticity: 0.5f).OnComplete(() =>
                    {
                        heartImage.sprite = _halfHeart;
                    });
            }
            _hasHalfHeart = true;
        }
        return IsGameOver();
    }
    
    private bool IsGameOver()
    {
        return _currentHeartIndex >= _hearts.Length && !_hasHalfHeart;
    }
}
