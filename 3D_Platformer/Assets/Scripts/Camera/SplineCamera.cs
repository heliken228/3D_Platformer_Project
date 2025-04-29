using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Splines;

public class SplineCamera : MonoBehaviour
{
    public SplineContainer _splineContainer;
    public float _speed = 5f;
    public CinemachineCamera _mainCinemachineCamera;
    public CinemachineCamera _splineCinemachineCamera;

    private bool _isFollowingSpline = false;
    private float _splinePosition = 0f;
    private CinemachineBrain _cinemachineBrain;
    private float tValue = 0f;

    private void Start()
    {
        _cinemachineBrain = GetComponentInChildren<CinemachineBrain>();
        
        if (_mainCinemachineCamera != null)
            _mainCinemachineCamera.Priority = 10;

        if (_splineCinemachineCamera != null)
            _splineCinemachineCamera.Priority = 0;
    }

    public void Update()
    {
        if (_isFollowingSpline)
        {
            CameraMove();
        }
    }

    private void CameraMove()
    {
        if (_splineContainer == null) return;

        float splineLength = _splineContainer.CalculateLength();
        float deltaMove = (_speed * Time.deltaTime) / splineLength;
        tValue += deltaMove;
        
        tValue = Mathf.Clamp01(tValue);

        _splineContainer.Evaluate(tValue, out var position, out var tangent, out var up);
        _splineCinemachineCamera.transform.position = (Vector3)position;
        
        Vector3 tangentDir = (Vector3)tangent;
        Vector3 upDir = (Vector3)up;

        if (tangentDir != Vector3.zero && upDir != Vector3.zero)
        {
            _splineCinemachineCamera.transform.rotation = Quaternion.LookRotation(tangentDir, upDir);
        }
        if (Mathf.Approximately(tValue, 1f))
        {
            SwitchBackToMainCamera();
        }
    }

    public void SwitchToSplineCamera()
    {
        _cinemachineBrain.DefaultBlend.Style = CinemachineBlendDefinition.Styles.Cut;
        if (_mainCinemachineCamera != null)
            _mainCinemachineCamera.Priority = 0;

        if (_splineCinemachineCamera != null)
            _splineCinemachineCamera.Priority = 10;

        _isFollowingSpline = true;
    }

    public void SwitchBackToMainCamera()
    {
        _cinemachineBrain.DefaultBlend.Style = CinemachineBlendDefinition.Styles.Cut;
        if (_mainCinemachineCamera != null)
            _mainCinemachineCamera.Priority = 10;

        if (_splineCinemachineCamera != null)
            _splineCinemachineCamera.Priority = 0;

        _isFollowingSpline = false;
    }
    
    public void SetSpline(SplineContainer splineContainer)
    {
        _splineContainer = splineContainer;
        tValue = 0f;
    }
}
