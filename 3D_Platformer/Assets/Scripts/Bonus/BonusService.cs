using System;
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

    private int _coinCount = 0;
    private int _gemCount = 0;
    private int _currentHeartIndex = 0;
    
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
        if (_currentHeartIndex < _hearts.Length)
        {
            Image heartImage = _hearts[_currentHeartIndex].GetComponent<Image>();
            if (heartImage != null)
            {
                heartImage.color = Color.black;
            }
            _currentHeartIndex++;
            
            return _currentHeartIndex >= _hearts.Length;
        }
        return true;
        
    }
}
