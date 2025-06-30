using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageIndicatorManager : MonoBehaviour
{
    public enum DamageType {
        Normal, Critical, Burn
    }

    [SerializeField] private ObjectPool _objectPool;
    [SerializeField] private Color _normalTextColor = Color.white;
    [SerializeField] private Color _criticalTextColor = Color.yellow;
    [SerializeField] private Color _burnTextColor = Color.red;
    [SerializeField] private Vector3 _maxSpawnDisplacement;
    [SerializeField] private Vector3 _decayDirection = Vector3.up;
    [SerializeField] private float _indicatorDecayTime = 0.5f;

    [SerializeField] private float _biggestDamageThreshold;
    [SerializeField] private Vector2 _fontSizeRange;

    public void PlayIndicator(Vector3 position, float damage, DamageType damageType, float duration = -1)
    {

        float randomDispX = (Random.Range(0f, 2f) - 1f) * _maxSpawnDisplacement.x;
        float randomDispY = Random.Range(0f, 1f) * _maxSpawnDisplacement.y;
        float randomDispZ = (Random.Range(0f, 2f) - 1f) * _maxSpawnDisplacement.z;
        position += new Vector3(randomDispX, randomDispY, randomDispZ);

        GameObject indicator = _objectPool.ReleaseObjectAt(position);
        TMP_Text text = indicator.GetComponentInChildren<TMP_Text>();
        if (!text)
        {
            Debug.LogWarning("[WARN]: Damage Indicator missing TMP Text.");
            _objectPool.ReturnObject(indicator);
            return;
        }

        switch (damageType)
        {
            case DamageType.Normal:
                text.text = $"{damage}";
                text.color = _normalTextColor;
                break;
            case DamageType.Critical:
                text.text = $"{damage}!";
                text.color = _criticalTextColor;
                break;
            case DamageType.Burn:
                text.text = $"{damage}";
                text.color = _burnTextColor;
                break;
        }

        float fontSize = Mathf.Clamp(damage, 0, _biggestDamageThreshold) / _biggestDamageThreshold * (_fontSizeRange.y - _fontSizeRange.x) + _fontSizeRange.x;
        text.fontSize = fontSize;

        if (duration < 0)
            duration = _indicatorDecayTime;

        StartCoroutine(IndicatorDecay(indicator, _decayDirection, duration));
    }

    private IEnumerator IndicatorDecay(GameObject indicator, Vector3 floatDisplacement, float duration){
        float elapsedTime = 0.0f;
        Vector3 originalPosition = indicator.transform.position;
        Vector3 targetPosition = originalPosition + floatDisplacement;
        while(elapsedTime < duration){
            elapsedTime += Time.deltaTime;

            indicator.transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / duration);
            yield return new WaitForEndOfFrame();
        }

        _objectPool.ReturnObject(indicator);
    }


    void Awake()
    {
        if(Instance != null){
            Destroy(this);
            return;
        }

        Instance = this;

        if(!TryGetComponent<ObjectPool>(out _objectPool)){
            Debug.LogWarning("[WARN]: Object Pool not found in Damage Indicator Manager");
        }
    }

    public static DamageIndicatorManager Instance { get; private set; }
    void OnDestroy(){
        if(Instance == this){
            Instance = null;
        }
    }
}
