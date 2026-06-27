using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStanceManager : MonoBehaviour
{
    [SerializeReference] private StanceDatabase _stanceDatabase;
    [SerializeField] private GameObject _switchPopup;
    [SerializeField] private Image _stanceIcon;
    [SerializeField] private float _switchPopupTime;
    public EStance SelectedStance;
    public StanceStatsScriptable SelectedStanceStats => _stanceDatabase.GetStance(SelectedStance);

    public void CycleStance()
    {
        switch (SelectedStance)
        {
            case EStance.Earth:
                if (ItemManager.Instance.hasUnlocked(AugmentType.Water))
                    SelectedStance = EStance.Water;
                else if (ItemManager.Instance.hasUnlocked(AugmentType.Air))
                    SelectedStance = EStance.Air;
                else if (ItemManager.Instance.hasUnlocked(AugmentType.Fire))
                    SelectedStance = EStance.Fire;
                else
                    SelectedStance = EStance.Earth;
                break;
            case EStance.Water:
                if (ItemManager.Instance.hasUnlocked(AugmentType.Air))
                    SelectedStance = EStance.Air;
                else if (ItemManager.Instance.hasUnlocked(AugmentType.Fire))
                    SelectedStance = EStance.Fire;
                else if (ItemManager.Instance.hasUnlocked(AugmentType.Earth))
                    SelectedStance = EStance.Earth;
                else
                    SelectedStance = EStance.Water;
                break;
            case EStance.Air:
                if (ItemManager.Instance.hasUnlocked(AugmentType.Fire))
                    SelectedStance = EStance.Fire;
                else if (ItemManager.Instance.hasUnlocked(AugmentType.Earth))
                    SelectedStance = EStance.Earth;
                else if (ItemManager.Instance.hasUnlocked(AugmentType.Water))
                    SelectedStance = EStance.Water;
                else
                    SelectedStance = EStance.Air;
                break;
            case EStance.Fire:
                if (ItemManager.Instance.hasUnlocked(AugmentType.Earth))
                    SelectedStance = EStance.Earth;
                else if (ItemManager.Instance.hasUnlocked(AugmentType.Water))
                    SelectedStance = EStance.Water;
                else if (ItemManager.Instance.hasUnlocked(AugmentType.Air))
                    SelectedStance = EStance.Air;
                else
                    SelectedStance = EStance.Fire;
                break;
            default:
                if (ItemManager.Instance.hasUnlocked(AugmentType.Earth))
                    SelectedStance = EStance.Earth;
                else if (ItemManager.Instance.hasUnlocked(AugmentType.Water))
                    SelectedStance = EStance.Water;
                else if (ItemManager.Instance.hasUnlocked(AugmentType.Air))
                    SelectedStance = EStance.Air;
                else if (ItemManager.Instance.hasUnlocked(AugmentType.Fire))
                    SelectedStance = EStance.Fire;
                else
                    SelectedStance = EStance.None;
                break;
        }

        StartCoroutine(StancePopup());
    }

    IEnumerator StancePopup()
    {
        _switchPopup.SetActive(true);
        if(_stanceDatabase.GetStance(SelectedStance) != null)
            _stanceIcon.sprite = _stanceDatabase.GetStance(SelectedStance).StanceIcon;
        yield return new WaitForSeconds(_switchPopupTime);
        _switchPopup.SetActive(false);
    }

    public static PlayerStanceManager Instance;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
}
