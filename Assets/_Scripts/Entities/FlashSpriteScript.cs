using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashSpriteScript : MonoBehaviour
{
    [SerializeReference] private Material _flashMaterial;
    [SerializeField] private float _maxDamageThreshold;
    [SerializeField] private int _maxFlashCount;
    [SerializeField] private List<SpriteRenderer> _targetSprites = new();
    private float _flashFrequency = 0.3f;
    private List<Material> _originalMaterials = new();
    private int _currentFlashCount;
    private bool _isFlashing;
    private bool _isTimeIndependent;
    private bool _isFlashColor;
    private float _elapsedTime;
    private int _flashCount;

    public void TriggerFlash(float damage, bool isTimeIndependent)
    {
        _isTimeIndependent = isTimeIndependent;
        _flashCount = (int)((_maxFlashCount - 1) * damage / _maxDamageThreshold) + 1;

        if (_isFlashing) return;
        _isFlashing = true;
        _elapsedTime = 0f;
        _currentFlashCount = 0;

        _isFlashColor = true;
        for (int i = 0; i < _targetSprites.Count; i++)
            _targetSprites[i].material = _flashMaterial;
    }

    private void CheckFlash()
    {
        if (_currentFlashCount >= _flashCount)
        {
            _isFlashing = false;
            for (int i = 0; i < _targetSprites.Count; i++)
                _targetSprites[i].material = _originalMaterials[i];
            return;
        }

        if (_isTimeIndependent)
            _elapsedTime += Time.unscaledDeltaTime;
        else
            _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _flashFrequency / 2f)
        {
            _elapsedTime = 0f;

            if (!_isFlashColor) {
                _isFlashColor = true;
                for (int i = 0; i < _targetSprites.Count; i++)
                    _targetSprites[i].material = _flashMaterial;
            }
            else
            {
                _isFlashColor = false;
                for (int i = 0; i < _targetSprites.Count; i++)
                    _targetSprites[i].material = _originalMaterials[i];
                _currentFlashCount++;
            }
        }
    }

    void Update()
    {
        if (_isFlashing) CheckFlash();
    }

    void Start()
    {
        foreach (var sprite in _targetSprites)
            _originalMaterials.Add(sprite.material);
    }
}
