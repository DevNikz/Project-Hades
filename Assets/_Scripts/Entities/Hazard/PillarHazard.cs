using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarHazard : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _destroyDelay;
    [SerializeField] private GameObject _visualBox;
    [SerializeField] private GameObject _hitbox;
    [SerializeField] private ParticleSystem _hitEffect;
    [SerializeField] private ParticleSystem _shakeEffect;
    [SerializeField] private ParticleSystem _debrisObjects;
    [SerializeField] private GameObject _fullState;
    [SerializeField] private GameObject _zeroHealthState;
    [SerializeField] private GameObject _destroyedState;
    [SerializeField] private List<GameObject> _pillarStateObjects = new();
    [SerializeField] private Animator _animator;

    private Collider _collider;
    private FlashSpriteScript _flashScript;
    private bool _wasDestroyed = true;

    private float _currentHealth;
    void OnEnable()
    {
        _currentHealth = _maxHealth;
        if (_maxHealth > 0) _wasDestroyed = false;
        UpdateLook();
        _flashScript = GetComponent<FlashSpriteScript>();
        _collider = GetComponentInChildren<Collider>();
        if(_collider != null) _collider.enabled = true;
    }

    private void UpdateLook()
    {
        float healthPercent = 0f;
        if (_maxHealth > 0)
            healthPercent = _currentHealth / _maxHealth;
        if(healthPercent > 0)
            _destroyedState.SetActive(false);


        if (!_wasDestroyed)
        {   
            _fullState.SetActive(healthPercent >= 1f);
            _zeroHealthState.SetActive(healthPercent <= 0f);
            foreach (var state in _pillarStateObjects)
                state.SetActive(false);
            for (int i = 0; i < _pillarStateObjects.Count; i++)
            {
                if (healthPercent >= 1f) break;
                if (healthPercent <= 0) break;
                
                if (((float)(_pillarStateObjects.Count - i) / (float)(_pillarStateObjects.Count)) <= healthPercent || i == _pillarStateObjects.Count - 1)
                {
                    _pillarStateObjects[i].SetActive(true);
                    break;
                }
            }
        }
        if (healthPercent <= 0f && !_wasDestroyed) StartCoroutine(DestroyPillar(_destroyDelay));
    }

    public void TakeDamage(float amount, bool isCrit)
    {
        if (_wasDestroyed) return;
        if (amount <= 0) return;

        DamageIndicatorManager.Instance.PlayIndicator(gameObject.transform.position, amount, isCrit ? DamageIndicatorManager.DamageType.Critical : DamageIndicatorManager.DamageType.Normal);

        if (amount > _currentHealth) amount = _currentHealth;
        _currentHealth -= amount;

        if (_currentHealth > 0)
            _wasDestroyed = false;

        //Play Effect
        _hitEffect.Play();
        if(_flashScript != null) _flashScript.TriggerFlash(amount, true);
        UpdateLook();
    }

    private IEnumerator DestroyPillar(float delay)
    {
        if (_wasDestroyed) yield break;
        Debug.Log("Shaking!");
        _zeroHealthState.SetActive(true);
        _wasDestroyed = true;
        _shakeEffect.Play();
        //_visualBox.SetActive(true);
        SFXManager.Instance.PlaySFXAtPosition("Falling_Debris", transform.position);

        yield return new WaitForSeconds(delay);

        //_visualBox.SetActive(false);


        _animator.Play("CollapseVFX");
        _hitbox.SetActive(true);

        _debrisObjects.Play();

        foreach (var state in _pillarStateObjects)
            state.SetActive(false);
        _fullState.SetActive(false);
        if(_collider != null) _collider.enabled = false;
        _zeroHealthState.SetActive(false);
        _destroyedState.SetActive(true);
        _shakeEffect.Stop();

        yield return new WaitForSeconds(0.5f);
        _hitbox.SetActive(false);
        
    }
}
