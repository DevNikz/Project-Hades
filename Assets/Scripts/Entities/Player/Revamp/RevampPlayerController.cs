using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Replaces the old Combat, Movement, and Player Controller script (partially, player state is in the new RevampPlayerStateHandler)
/// </summary>
[RequireComponent(typeof(PlayerAnimatorController))]
public class RevampPlayerController : MonoBehaviour
{
    [SerializeReference] private InputActionAsset _inputActions;
    [SerializeReference] private PlayerCombatStats _playerStats;
    [SerializeReference] private StanceDatabase _stanceDatabase;

    [SerializeField] private MeleeController _hitbox;
    [SerializeField] private GameObject _hitboxPointer;
    [SerializeField] private GameObject _stanceSwitchPopup;
    [SerializeField] private Image _stanceSwitchIcon;
    [SerializeField] private float _stanceSwitchPopupTime = 0.5f;
    [SerializeField] private GameObject _augmentMenu;

    #region HelperProperties
    private InputActionMap _playerMap;
    private PlayerAnimatorController _animator;
    private int _activeStanceIndex = 0;
    private int _comboCount = 0;
    private RevampPlayerAttackStatsScriptable _queuedAttack = null;
    private bool _queuedAttackIsSpecial = false;
    private bool _lastPerformedAttackIsSpecial = false;
    private float _timeOfLastAttack;
    #endregion

    void Update()
    {
        if (_playerMap.FindAction("Attack").WasPressedThisFrame() && IsMouseOverGameWindow)
            ProcessAttack(false);
        if (_playerMap.FindAction("SpecialAttack").WasPressedThisFrame() && IsMouseOverGameWindow)
            ProcessAttack(true);
        if (_playerMap.FindAction("Dash").WasPressedThisFrame())
            ProcessDash();
        if (_playerMap.FindAction("StanceSwitch").WasPressedThisFrame())
            ProcessStanceSwitch();
        if (_playerMap.FindAction("OpenAugmentMenu").WasPressedThisFrame())
            ProcessOpenAugmentMenu(true);
        if (_playerMap.FindAction("OpenAugmentMenu").WasReleasedThisFrame())
            ProcessOpenAugmentMenu(false);

        TryExecuteNextAttack(Time.time - _timeOfLastAttack);

        ProcessMovement(_playerMap.FindAction("MoveInput").ReadValue<Vector2>());
        ProcessUpdatePointerDirection(_playerMap.FindAction("MousePosition").ReadValue<Vector2>());
    }

    #region Attacks
    bool IsMouseOverGameWindow {
        get {
            Vector3 mp = Input.mousePosition;
            return !( 0>mp.x || 0>mp.y || Screen.width<mp.x || Screen.height<mp.y );
        }
    }
    private RevampPlayerAttackStatsScriptable LastUsedAttack
    {
        get
        {
            if (_lastPerformedAttackIsSpecial)
            {
                if (CurrentStance.SpecialAttacks.Count <= _comboCount) return null;
                return CurrentStance.SpecialAttacks[_comboCount];
            }
            else
            {

                if (CurrentStance.NormalAttacks.Count <= _comboCount) return null;
                return CurrentStance.NormalAttacks[_comboCount];
            }
        }
    }
    private RevampPlayerAttackStatsScriptable NextNormalAttack
    {
        get
        {
            if (CurrentStance.NormalAttacks.Count <= _comboCount + 1) return null;
            return CurrentStance.NormalAttacks[_comboCount + 1];
        }
    }
    private RevampPlayerAttackStatsScriptable NextSpecialAttack
    {
        get
        {
            if (CurrentStance.SpecialAttacks.Count <= _comboCount + 1) return null;
            return CurrentStance.SpecialAttacks[_comboCount + 1];
        }
    }

    void ProcessAttack(bool isSpecialAttack)
    {
        /** CHECK IF CLICK IS AT A VALID TIME **/

        // FAIL IF NO VALID NEXT ATTACK CAN BE MADE
        if ((isSpecialAttack && NextSpecialAttack == null) ||
            (!isSpecialAttack && NextNormalAttack == null))
        {
            AttackFail(LastUsedAttack.ComboMissPunishTime);
            return;
        }

        // FAIL IF PRESSED AFTER CURRENT ATTACK'S WINDOW BUT NOT IF PAST ITS FORGET
        if (Time.time - _timeOfLastAttack > LastUsedAttack.ComboInputWindowMaxTime &&
            Time.time - _timeOfLastAttack < LastUsedAttack.AttackForgottenTime)
        {
            AttackFail(LastUsedAttack.ComboMissPunishTime);
            return;
        }

        // TODO : FAIL IF MANA CANT BE SPENT

        /* SUCCESS */
        // WILL SET NEXT ATTACK TO THE LAST BUTTON PRESS OF THE PLAYER
        if (isSpecialAttack) _queuedAttack = NextSpecialAttack;
        else _queuedAttack = NextNormalAttack;
        _queuedAttackIsSpecial = isSpecialAttack;
    }
    void TryExecuteNextAttack(float timeSinceLastAttack)
    {
        // FAIL IF NO NEXT ATTACK EXISTS
        if (_queuedAttack == null)
            return;

        // FAIL TO EXECUTE IF ELAPSED TIME IS NOT ENOUGH
        if (timeSinceLastAttack < LastUsedAttack.EarliestTimeForNextAttack)
            return;

        /* EXECUTE ATTACK */

        // SET HITBOX VALUES
        _hitbox.SetAttackStats(
            _queuedAttack.BaseDamage + _playerStats.BaseDamage,
            _queuedAttack.BasePoiseDamage + _playerStats.BasePoiseDamage,
            _queuedAttack.BaseKnockback + _playerStats.BaseKnockback
        );

        // SET ANIMATION
        _animator.RevampedPlayAttackAnim(_queuedAttack.AnimationClipName);

        // RESET STATE FOR PERFORMED ATTACK
        _timeOfLastAttack = Time.time;
        _lastPerformedAttackIsSpecial = _queuedAttackIsSpecial;
        _queuedAttack = null;
        _queuedAttackIsSpecial = false;
    }
    void AttackFail(float lagtime)
    {
        _timeOfLastAttack = Time.time + lagtime;
        _comboCount = 0;
        _queuedAttack = null;
        _queuedAttackIsSpecial = false;
        _lastPerformedAttackIsSpecial = false;
    }
    #endregion

    void ProcessDash()
    {
        Debug.Log("Dashed!!");
    }

    #region StanceSwiching
    private StanceStatsScriptable CurrentStance
    {
        get { return _stanceDatabase.Stances[_activeStanceIndex]; }
        set { _activeStanceIndex = _stanceDatabase.Stances.IndexOf(value); }
    }
    void ProcessStanceSwitch()
    {
        Debug.Log("Stance Switched!");
        bool leaveLoop = false;
        for (int i = 0; i < 4 && !leaveLoop; i++)
        {
            _activeStanceIndex += (_activeStanceIndex + 1) % _stanceDatabase.Stances.Count;
            switch (CurrentStance.StanceType)
            {
                case EStance.Earth:
                    if (ItemManager.Instance.hasUnlocked(AugmentType.Earth)) leaveLoop = true;
                    break;
                case EStance.Water:
                    if (ItemManager.Instance.hasUnlocked(AugmentType.Water)) leaveLoop = true;
                    break;
                case EStance.Air:
                    if (ItemManager.Instance.hasUnlocked(AugmentType.Air)) leaveLoop = true;
                    break;
                case EStance.Fire:
                    if (ItemManager.Instance.hasUnlocked(AugmentType.Fire)) leaveLoop = true;
                    break;
            }
        }

        StartCoroutine(StancePopup());
    }
    IEnumerator StancePopup()
    {
        _stanceSwitchPopup.SetActive(true);
        _stanceSwitchIcon.sprite = CurrentStance.StanceIcon;
        yield return new WaitForSeconds(_stanceSwitchPopupTime);
        _stanceSwitchPopup.SetActive(false);
    }
    #endregion

    void ProcessOpenAugmentMenu(bool open)
    {
        _augmentMenu.SetActive(open);
    }

    void ProcessMovement(Vector2 input)
    {
        Debug.Log($"Move Input: {input}");

    }

    #region UpdatingPointerInfo
    private float _hitboxPointerOriginalXRotation = 0.0f;
    void ProcessUpdatePointerDirection(Vector2 position)
    {
        _hitboxPointer.transform.position = new Vector3(this.transform.position.x, 0.05f, this.transform.position.z);
        float angle = ToIsoRotation(position);
        Quaternion rot = Quaternion.Euler(_hitboxPointerOriginalXRotation, -angle-45, 0.0f);
        _hitboxPointer.transform.rotation = rot;
    }
    float ToIsoRotation(Vector2 position) {
        Vector3 tempVector = Camera.main.WorldToScreenPoint(_hitboxPointer.transform.position);
        tempVector = (Vector3)position - tempVector;
        return Mathf.Atan2(tempVector.y, tempVector.x) * Mathf.Rad2Deg;
    }
    
    void UpdateAnimatorControllerStates()
    {
        _animator.SetMovement(entityMovement);
        _animator.SetDirection(lookDirection);
        _animator.SetAtkDir(attackDir);
        if (RevampPlayerStateHandler.Instance.IsDead)
        {
            RevampPlayerStateHandler.Instance.CurrentState = EntityState.Dead;
        }
        _animator.SetState(RevampPlayerStateHandler.Instance.CurrentState);
    }
    #endregion
    void ResetState()
    {
        _comboCount = 0;
        _timeOfLastAttack = Time.time;
        _queuedAttack = null;
        _queuedAttackIsSpecial = false;
        _lastPerformedAttackIsSpecial = false;
    }

    void OnEnable()
    {
        _inputActions.Enable();
        _playerMap = _inputActions.FindActionMap("Player");
        _animator = GetComponent<PlayerAnimatorController>();
        _hitboxPointerOriginalXRotation = _hitboxPointer.transform.rotation.eulerAngles.x;
        ResetState();
    }

    void OnDisable()
    {
        _inputActions.Disable();
    }

}
