using System;
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
    [SerializeReference] public PlayerCombatStats PlayerStats;
    [SerializeReference] private StanceDatabase _stanceDatabase;

    [SerializeField] private MeleeController _attackHitbox;
    [SerializeField] private GameObject _hitboxPointer;
    [SerializeField] private ParticleSystem _walkParticles;
    [SerializeField] private ParticleSystem _dashParticles;
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
    private Rigidbody _rigidbody;
    private Vector3 _lastMoveInput = Vector3.zero;
    public float CurrentSpeed;
    private float _timeOfLastDash = 0.0f;
    private bool _isDashing = false;
    #endregion

    void Update()
    {
        ProcessUpdatePointerDirection(_playerMap.FindAction("MousePosition").ReadValue<Vector2>());

        if (_playerMap.FindAction("Attack").WasPressedThisFrame() && IsMouseOverGameWindow)
            ProcessAttack(false);
        if (_playerMap.FindAction("SpecialAttack").WasPressedThisFrame() && IsMouseOverGameWindow)
            ProcessAttack(true);
        if (_playerMap.FindAction("StanceSwitch").WasPressedThisFrame())
            ProcessStanceSwitch();
        if (_playerMap.FindAction("OpenAugmentMenu").WasPressedThisFrame())
            ProcessOpenAugmentMenu(true);
        if (_playerMap.FindAction("OpenAugmentMenu").WasReleasedThisFrame())
            ProcessOpenAugmentMenu(false);

        TryExecuteNextAttack(Time.time - _timeOfLastAttack);
    }

    void FixedUpdate()
    {
        if (_playerMap.FindAction("Dash").WasPressedThisFrame())
            ProcessDash();

        ProcessMovement(_playerMap.FindAction("MoveInput").ReadValue<Vector2>());
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
        _attackHitbox.SetAttackStats(
            _queuedAttack.BaseDamage + PlayerStats.BaseDamage,
            _queuedAttack.BasePoiseDamage + PlayerStats.BasePoiseDamage,
            _queuedAttack.BaseKnockback + PlayerStats.BaseKnockback
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

    #region Movement
    void ProcessMovement(Vector2 input)
    {
        // ESCAPE IF SHOULDNT MOVE
        if (LevelTrigger.HudCheck) return;

        ParticleSystem.EmissionModule particleEmission = _walkParticles.emission;
        if (input.sqrMagnitude <= 0 || !gameObject.CompareTag("Player"))
        {
            _animator.SetMovement(EntityMovement.Idle);
            _rigidbody.drag = 1000.0f;
            particleEmission.enabled = false;
            return;
        }

        // UPDATE MOVE DIRECTION
        _lastMoveInput = new(input.x, 0.0f, input.y);

        if (_isDashing) return;

        // SET ANIMATOR
        _animator.SetMovement(EntityMovement.Strafing);
        _animator.SetDirection(input.x >= 0 ? LookDirection.Right : LookDirection.Left);
        particleEmission.enabled = true;

        // MOVE, SPEED CHANGES BASED ON IF ATTACKING OR NOT
        ResetSpeed();
        _rigidbody.drag = 0.0f;
        if (RevampPlayerStateHandler.Instance.CurrentState != EntityState.Attack)
            _rigidbody.velocity = 100.0f * CurrentSpeed * Time.fixedDeltaTime * ((Vector3)_lastMoveInput).ToIso();
            // _rigidbody.AddForce(, ForceMode.Impulse);
        else
        {
            _rigidbody.velocity = 100.0f * Math.Min(LastUsedAttack.MaxMoveSpeed, CurrentSpeed) * Time.fixedDeltaTime * ((Vector3)_lastMoveInput).ToIso();
        }
    }
    void ProcessDash()
    {
        if (Time.time - _timeOfLastDash < PlayerStats.DashCooldown) return;

        Debug.Log("Tried to dash!");

        _isDashing = true;
        _rigidbody.drag = 0.0f;
        _timeOfLastDash = Time.time;
        _dashParticles.Play();

        _rigidbody.AddForce(100.0f * PlayerStats.DashSpeed * Time.fixedDeltaTime * ((Vector3)_lastMoveInput).ToIso(), ForceMode.VelocityChange);

        Invoke(nameof(ResetDashing), PlayerStats.DashTime);
    }
    void ResetDashing()
    {
        _isDashing = false;
        _rigidbody.drag = 1000.0f;
    }
    #endregion

    #region StanceSwiching
    private StanceStatsScriptable CurrentStance
    {
        get { return _stanceDatabase.Stances[_activeStanceIndex]; }
        set { _activeStanceIndex = _stanceDatabase.Stances.IndexOf(value); }
    }
    void ProcessStanceSwitch()
    {
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

    #region UpdatingPointerInfo
    private float _hitboxPointerOriginalXRotation = 0.0f;
    void ProcessUpdatePointerDirection(Vector2 position)
    {
        _hitboxPointer.transform.position = new Vector3(this.transform.position.x, 0.05f, this.transform.position.z);
        float angle = ToIsoRotation(position);
        Quaternion rot = Quaternion.Euler(_hitboxPointerOriginalXRotation, -angle - 45, 0.0f);
        _hitboxPointer.transform.rotation = rot;
        
        _animator.SetAtkDir(angle >= -90 && angle <= 90 ? LookDirection.Right : LookDirection.Left);
    }
    float ToIsoRotation(Vector2 position) {
        Vector3 tempVector = Camera.main.WorldToScreenPoint(_hitboxPointer.transform.position);
        tempVector = (Vector3)position - tempVector;
        return Mathf.Atan2(tempVector.y, tempVector.x) * Mathf.Rad2Deg;
    }
    
    void UpdateAnimatorControllerStates()
    {
        // _animator.SetMovement(entityMovement);
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
        _timeOfLastDash = Time.time;
        _queuedAttack = null;
        _queuedAttackIsSpecial = false;
        _lastPerformedAttackIsSpecial = false;
        _lastMoveInput = Vector2.zero;
        _isDashing = false;
        ResetSpeed();

        _walkParticles.Play();
    }
    public void ResetSpeed()
    {
        CurrentSpeed = PlayerStats.MoveSpeed;
    }

    void OnEnable()
    {
        _inputActions.Enable();
        _playerMap = _inputActions.FindActionMap("Player");
        _animator = GetComponent<PlayerAnimatorController>();
        _hitboxPointerOriginalXRotation = _hitboxPointer.transform.rotation.eulerAngles.x;
        _rigidbody = GetComponent<Rigidbody>();
        ResetState();
    }

    void OnDisable()
    {
        _inputActions.Disable();
    }

}
