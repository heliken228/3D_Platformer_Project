using System;
using TMPro;
using UnityEngine;

public class BonusService : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private TextMeshProUGUI _gemText;
    [SerializeField] private AudioSource _coinSound;
    [SerializeField] private AudioSource _gemSound;

    private int _coinCount = 0;
    private int _gemCount = 0;
    
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
