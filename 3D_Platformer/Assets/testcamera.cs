using UnityEngine;
using UnityEngine.Splines;

public class testcamera : MonoBehaviour
{
    public SplineContainer _splineContainer;
    public float _speed = 5f;
    private float tValue = 0f;
    
    
    void Update()
    {
        if (_splineContainer == null) return;

        float splineLength = _splineContainer.CalculateLength();
        float deltaMove = (_speed * Time.deltaTime) / splineLength;
        tValue += deltaMove;
        
        tValue = Mathf.Clamp01(tValue);

        _splineContainer.Evaluate(tValue, out var position, out var tangent, out var up);
        transform.position = (Vector3)position;
        
        Vector3 tangentDir = (Vector3)tangent;
        Vector3 upDir = (Vector3)up;

        if (tangentDir != Vector3.zero && upDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(tangentDir, upDir);
        }
    }
}
