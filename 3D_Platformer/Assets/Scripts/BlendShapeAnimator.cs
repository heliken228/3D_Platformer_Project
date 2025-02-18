using UnityEngine;

public class BlendShapeAnimator : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField] private int _blendShapeIndex = 0;
    [SerializeField] private float _animationDuration = 0.5f;

    private float _timer = 0.0f;
    private bool _isAnimatingForward = true;

    void Update()
    {
        _timer += _isAnimatingForward ? Time.deltaTime : -Time.deltaTime;
        /*
        if (_isAnimatingForward)
        {
            _timer += Time.deltaTime;
        }
        else
        {
            _timer -= Time.deltaTime;
        }*/

        float blendShapeValue = Mathf.PingPong(_timer, _animationDuration) / _animationDuration;
        _skinnedMeshRenderer.SetBlendShapeWeight(_blendShapeIndex, blendShapeValue * 100);

        if (_timer >= _animationDuration)
        {
            _isAnimatingForward = false;
        }
        else if (_timer <= 0)
        {
            _isAnimatingForward = true;
        }
    }
}
