using System;
using UnityEngine;

public class EmissionColorChanger : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private float _speed = 1f;
    
    private Material _emissionMaterial;
    private float _lerpValue = 0f;
    private bool _transitionToBlack = true;

    private void Start()
    {
        Material[] materials = _renderer.materials;
        _emissionMaterial = materials[1];
        _emissionMaterial.EnableKeyword("_EMISSION");
    }

    private void Update()
    {
        if (_transitionToBlack)
        {
            _lerpValue += _speed * Time.deltaTime;
            if (_lerpValue >= 1f)
            {
                _lerpValue = 1f;
                _transitionToBlack = false;
            }
        }
        else
        {
            _lerpValue -= _speed * Time.deltaTime;
            if (_lerpValue <= 0f)
            {
                _lerpValue = 0f;
                _transitionToBlack = true;
            }
        }
        
        Color newEmissionColor = Color.Lerp(Color.white, Color.black, _lerpValue);
        _emissionMaterial.SetColor("_EmissionColor", newEmissionColor);
    }
}
