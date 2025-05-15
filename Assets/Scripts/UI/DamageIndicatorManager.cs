using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageIndicatorManager : MonoBehaviour
{
    public enum DamageType {
        Normal, Critical, Burn
    }

    private ObjectPool _objectPool;
    [SerializeField] private Color _normalTextColor = Color.white;
    [SerializeField] private Color _criticalTextColor = Color.yellow;
    [SerializeField] private Color _burnTextColor = Color.red;
    [SerializeField] private float _indicatorDecayTime = 0.5f;

    public void PlayIndicator(Vector3 position, float damage, DamageType damageType, float duration = -1){
        GameObject indicator = _objectPool.ReleaseObjectAt(position);
        if(!indicator.TryGetComponent<TMP_Text>(out var text)){
            Debug.LogWarning("[WARN]: Damage Indicator missing TMP Text.");
            _objectPool.ReturnObject(indicator);
            return;
        }

        switch(damageType){
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

        if(duration < 0)
            duration = _indicatorDecayTime;

        StartCoroutine(IndicatorDecay(indicator, Vector3.up, duration));
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

        if(!gameObject.TryGetComponent<Canvas>(out var canvas)){
            Debug.LogWarning("[WARN]: Damage Indicator Manager missing Canvas.");
            return;
        }       

        Camera mainCamera = Camera.main;
        if(!mainCamera){
            Debug.LogWarning("[WARN]: Main camera not found in Damage Indicator Manager!");
            return;
        }

        canvas.worldCamera = mainCamera;

        if(!TryGetComponent<ObjectPool>(out var _objectPool)){
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
