using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    
{
    [SerializeField] private AudioSource _audioSource;
    
    public float scaleFactor = 1.1f;
    public float animationDuration = 0.2f;

    private Vector3 _originalScale;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _audioSource.Play();
        transform.DOScale(_originalScale * scaleFactor, animationDuration)
            .SetEase(Ease.OutBack)
            .SetUpdate(true); // Игнорируем Time.timeScale
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(_originalScale, animationDuration)
            .SetEase(Ease.OutBack)
            .SetUpdate(true); // Игнорируем Time.timeScale
    }
}
