using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class GloveKick : MonoBehaviour
{
    public Vector3 GloveKickDirection;
    
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField] private int _blendShapeIndex = 0;
    [SerializeField] private Transform _gloveTransform;
    [SerializeField] private Vector3 _gloveBasePosition;
    [SerializeField] private float _glovePunchDistance = 4f;

    private float _blendWeight = 100f;

    private void Start()
    {
        if (_gloveTransform != null)
            _gloveBasePosition = _gloveTransform.localPosition;

        PlaySpringCycle();
    }

    private void PlaySpringCycle()
    {
        Sequence springSequence = DOTween.Sequence();

        // --- ПРУЖИНА ---

        // Быстрый сжатие BlendShape 100 → 0
        springSequence.Append(DOTween.To(() => _blendWeight, x =>
        {
            _blendWeight = x;
            _skinnedMeshRenderer.SetBlendShapeWeight(_blendShapeIndex, x);
        }, -100f, 0.2f).SetEase(Ease.OutExpo));

        // Тряска BlendShape 0 ↔ 10
        springSequence.Append(DOTween.To(() => _blendWeight, x =>
        {
            _blendWeight = x;
            _skinnedMeshRenderer.SetBlendShapeWeight(_blendShapeIndex, x);
        }, -80f, 0.1f).SetLoops(4, LoopType.Yoyo).SetEase(Ease.InOutSine));

        // Медленный возврат BlendShape 0 → 100
        springSequence.Append(DOTween.To(() => _blendWeight, x =>
        {
            _blendWeight = x;
            _skinnedMeshRenderer.SetBlendShapeWeight(_blendShapeIndex, x);
        }, 100f, 1.2f).SetEase(Ease.InOutQuad));

        springSequence.SetLoops(-1);

        // --- ПЕРЧАТКА ---

        Sequence gloveSequence = DOTween.Sequence();

        // Быстрый толчок вперёд
        gloveSequence.Append(_gloveTransform.DOLocalMove(_gloveBasePosition + Vector3.forward * _glovePunchDistance, 0.2f).SetEase(Ease.OutExpo));

        // Тряска туда-сюда
        gloveSequence.Append(_gloveTransform.DOLocalMove(_gloveBasePosition + Vector3.forward * (_glovePunchDistance * 0.9f), 0.1f)
            .SetLoops(4, LoopType.Yoyo).SetEase(Ease.InOutSine));

        // Медленное возвращение в исходную позицию
        gloveSequence.Append(_gloveTransform.DOLocalMove(_gloveBasePosition, 1.2f).SetEase(Ease.InOutQuad));

        gloveSequence.SetLoops(-1);
    }
}
