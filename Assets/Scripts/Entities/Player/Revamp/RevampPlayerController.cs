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

    [SerializeField] private Camera _mainCamera;
    [SerializeField] private LayerMask _targetableMask;
    [SerializeField] private MeleeController _attackHitbox;
    [SerializeField] private PlayerAttackAnimCallback _attackAnimCallback;
    [SerializeField] private GameObject _hitboxPointer;
    [SerializeField] private List<GameObject> _stanceAttackIndicators = new();
    [SerializeField] private bool _useAutohitboxSpawn = false;
    [SerializeField] private ParticleSystem _walkParticles;
    [SerializeField] private ParticleSystem _dashParticles;
    [SerializeField] private GameObject _stanceSwitchPopup;
    [SerializeField] private Image _stanceSwitchIcon;
    [SerializeField] private float _stanceSwitchPopupTime = 0.5f;
    [SerializeField] private GameObject _augmentMenu;

    #region HelperProperties
    private InputActionMap _playerMap;
    private PlayerAnimatorController _animator;
    private RevampPlayerStateHandler _stateHandler;
    private int _activeStanceIndex = 0;
    private int _comboCount = -1;
    private RevampPlayerAttackStatsScriptable _queuedAttack = null;
    private bool _queuedAttackIsSpecial = false;
    private bool _lastPerformedAttackIsSpecial = false;
    private float _timeOfLastAttack;
    private Rigidbody _rigidbody;
    private Vector3 _lastMoveInput = Vector3.zero;
    public float CurrentSpeed;
    private float _timeOfLastDash = 0.0f;
    private bool _isDashing = false;
    private bool _holdingAnAttack = false;
    private float _chargingStartTime = 0.0f;
    private float _chargeTime = 0.0f;
    #endregion

    void Update()
    {
        UpdatePointerInfo();

        if (IsMouseOverGameWindow && gameObject.tag == "Player")
        {
            InputAction attack = _playerMap.FindAction("Attack");
            InputAction specialAttack = _playerMap.FindAction("SpecialAttack");
            if (attack.IsPressed())                               ProcessAttack(false);
            if (specialAttack.IsPressed())                        Debug.Log("Press R");
            if (attack.IsPressed())                               Debug.Log("Press L");
            if (specialAttack.IsPressed())                        ProcessAttack(true);
            if (attack.WasReleasedThisFrame())                              Debug.Log("Release L");
            if (specialAttack.WasReleasedThisFrame())                       Debug.Log("Release R");
            if (attack.WasReleasedThisFrame() && _holdingAnAttack)          SubmitAttack(false);
            if (specialAttack.WasReleasedThisFrame() && _holdingAnAttack)   SubmitAttack(true);
        }

        if (_playerMap.FindAction("StanceSwitch").WasPressedThisFrame())
            ProcessStanceSwitch();
        if (_playerMap.FindAction("OpenAugmentMenu").WasPressedThisFrame())
            ProcessOpenAugmentMenu(true);
        if (_playerMap.FindAction("OpenAugmentMenu").WasReleasedThisFrame())
            ProcessOpenAugmentMenu(false);

        // Reset Attack Anims and state if attack is over and nothing is queued
        if (_stateHandler.CurrentState == EntityState.Attack && !_attackAnimCallback._isAttacking && _queuedAttack == null)
        {
            _stateHandler.CurrentState = EntityState.None;
            _animator.ResetAttack();
        }

        // Forget last attack if the current attack is over and no new attack is queued
        if (LastUsedAttack != null && _queuedAttack == null && !_attackAnimCallback._isAttacking && !_holdingAnAttack)
        {
            _comboCount = -1;
            _stateHandler.CurrentState = EntityState.None;
        }

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
            if (_comboCount < 0) return null;
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
        // Cancel invoke if already being charged
        if (_holdingAnAttack) return;

        /** CHECK IF CLICK IS AT A VALID TIME **/

        // NOTHIN IS REGISTERED IF THE ATTACK INDICATOR IS DEACTIVATED
        if (_hitboxPointer.activeInHierarchy == false) return;

        // NOTHIN IS REGISTERED IF THERE IS NO NEXT VALID ATTACK
        if ((isSpecialAttack && NextSpecialAttack == null) ||
            (!isSpecialAttack && NextNormalAttack == null))
        {
            return;
        }

        /* SUCCESS, WILL ACTIVATE MOVE SUBMISSION ON BUTTON RELEASE */
        _holdingAnAttack = true;
        _chargingStartTime = Time.time;
        _chargeTime = 0.0f;
    }

    void SubmitAttack(bool isSpecialAttack)
    {
        _holdingAnAttack = false;

        // WILL SET NEXT ATTACK TO THE LAST BUTTON PRESS OF THE PLAYER
        if (isSpecialAttack) _queuedAttack = NextSpecialAttack;
        else _queuedAttack = NextNormalAttack;
        _queuedAttackIsSpecial = isSpecialAttack;
        _chargeTime = Time.time - _chargingStartTime;
    }

    void TryExecuteNextAttack(float timeSinceLastAttack)
    {
        // FAIL IF NO NEXT ATTACK EXISTS
        if (_queuedAttack == null)
            return;

        // FAIL TO EXECUTE IF AN ATTACK IS STILL PLAYING
        if (LastUsedAttack != null && _attackAnimCallback._isAttacking)
            return;

        // FAIL IF MANA CANT BE SPENT
        if (_stateHandler.CurrentCharge < _queuedAttack.ManaCost)
        {
            _queuedAttack = null;
            return;
        }

        /* EXECUTE ATTACK */

        // USE MANA (Reward is on hit)
        _stateHandler.UseCharge(_queuedAttack.ManaCost);

        // SET HITBOX VALUES
        _attackHitbox.SetAttackStats(
            _queuedAttack,
            CurrentStance,
            PlayerStats,
            transform.position,
            _chargeTime
        );

        // TP Player if they are in Wind Stance
        if(CurrentStance.StanceType == EStance.Air)
            transform.position = _hitboxPointer.transform.position;

        // SET ANIMATION
        _animator.RevampedPlayAttackAnim(_queuedAttack.AnimationClipName, 0, _queuedAttack.VFXAnimClipName, _queuedAttack.SFXClipName);

        // SET STATE FOR PERFORMED ATTACK
        _stateHandler.CurrentState = EntityState.Attack;
        _comboCount++;
        _timeOfLastAttack = Time.time;
        _lastPerformedAttackIsSpecial = _queuedAttackIsSpecial;
        _queuedAttack = null;
        _queuedAttackIsSpecial = false;
    }
    void AttackFail(float lagtime)
    {
        _stateHandler.CurrentState = EntityState.None;
        Debug.Log("Attack Failed");
        _timeOfLastAttack = Time.time + lagtime;
        _queuedAttack = null;
        _comboCount = -1;
        _queuedAttackIsSpecial = false;
        _lastPerformedAttackIsSpecial = false;
    }
    private float _lastHitboxEndTime = 0.0f;
    IEnumerator SpawnHitbox(float spawnDelay, float lingerTime)
    {
        yield return new WaitForSeconds(spawnDelay);
        _lastHitboxEndTime = Time.time + lingerTime;
        _attackHitbox.gameObject.SetActive(true);
    }
    void DeactivateHitbox()
    {
        // Debug.Log($"Attack Hitbox: {_lastHitboxEndTime}, {Time.time}");
        if(_lastHitboxEndTime < Time.time)
            _attackHitbox.gameObject.SetActive(false);
    }
    void ResetAttackingAnim()
    {
        DeactivateHitbox();
        _animator.DelayedResetAttack();
    }

    #endregion

    #region Movement
    void ProcessMovement(Vector2 input)
    {
        // ESCAPE IF SHOULDNT MOVE

        if (LevelTrigger.AtEndOfLevel) return;

        ParticleSystem.EmissionModule particleEmission = _walkParticles.emission;
        if ((input.sqrMagnitude <= 0 && !_isDashing) || !gameObject.CompareTag("Player"))
        {

            if (_stateHandler.CurrentState == EntityState.None)
            {
                _animator.SetMovement(EntityMovement.Idle);
                _animator.RevampSetMoving(false);   
            }

            _rigidbody.drag = 1000.0f;
            particleEmission.enabled = false;
            return;
        }

        // UPDATE MOVE DIRECTION
        if(input.sqrMagnitude > 0)
            _lastMoveInput = new(input.x, 0.0f, input.y);

        // Debug.Log("Move Input: " + _lastMoveInput);

        if (_isDashing) return;

        // SET ANIMATOR
        if (_stateHandler.CurrentState == EntityState.None)
        {
            _animator.SetMovement(EntityMovement.Strafing);
            _animator.RevampSetMoving(true);
        }
        if(_stateHandler.CurrentState != EntityState.Attack)
            _animator.SetDirection(input.x >= 0 ? LookDirection.Right : LookDirection.Left);
        particleEmission.enabled = true;

        // MOVE, SPEED CHANGES BASED ON IF ATTACKING OR NOT
        // Debug.Log("Current Speed: " + CurrentSpeed);
        _rigidbody.drag = 0.0f;
        if (_stateHandler.CurrentState != EntityState.Attack)
            _rigidbody.velocity = 100.0f * CurrentSpeed * Time.fixedDeltaTime * ((Vector3)_lastMoveInput).ToIso();
        // _rigidbody.AddForce(, ForceMode.Impulse);
        else
        {
            // If the attack anim callback is less than 0, disables custom per frame speed, using the attack's max movespeed, otherwise, prefers the per frame speed
            if (_attackAnimCallback._attackMoveSpeed < 0)
                _rigidbody.velocity = 100.0f * Math.Min(LastUsedAttack.MaxMoveSpeed, CurrentSpeed) * Time.fixedDeltaTime * ((Vector3)_lastMoveInput).ToIso();
            else
                _rigidbody.velocity = 100.0f * Math.Min(_attackAnimCallback._attackMoveSpeed, CurrentSpeed) * Time.fixedDeltaTime * ((Vector3)_lastMoveInput).ToIso();
        }
    }
    void ProcessDash()
    {
        if (Time.time - _timeOfLastDash < PlayerStats.DashCooldown) return;

        _isDashing = true;
        _timeOfLastDash = Time.time;
        _rigidbody.drag = 0.0f;
        _dashParticles.Play();

        if(_stateHandler.CurrentState == EntityState.None)
            _animator.RevampDashAnim(_isDashing);

        _rigidbody.velocity = 100.0f * PlayerStats.DashSpeed * Time.fixedDeltaTime * ((Vector3)_lastMoveInput).ToIso();

        _stateHandler.SetInvincibility(PlayerStats.DashTime);

        Invoke(nameof(ResetDashing), PlayerStats.DashTime);
    }
    void ResetDashing()
    {
        _isDashing = false;
        _animator.RevampDashAnim(_isDashing);
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
            _activeStanceIndex = (_activeStanceIndex + 1) % _stanceDatabase.Stances.Count;
            switch (CurrentStance.StanceType)
            {
                case EStance.Earth:
                    if (ItemManager.Instance.hasUnlocked(AugmentType.Earth)) leaveLoop = true;
                    SFXManager.Instance.PlaySFX("Earth_Select");
                    break;
                case EStance.Water:
                    if (ItemManager.Instance.hasUnlocked(AugmentType.Water)) leaveLoop = true;
                    SFXManager.Instance.PlaySFX("Water_Select");
                    break;
                case EStance.Air:
                    if (ItemManager.Instance.hasUnlocked(AugmentType.Air)) leaveLoop = true;
                    SFXManager.Instance.PlaySFX("Wind_Select");
                    break;
                case EStance.Fire:
                    if (ItemManager.Instance.hasUnlocked(AugmentType.Fire)) leaveLoop = true;
                    SFXManager.Instance.PlaySFX("Fire_Select");
                    break;
            }
        }
        
        UpdateAttackIndicator();
        StartCoroutine(StancePopup());
    }
    IEnumerator StancePopup()
    {
        _stanceSwitchPopup.SetActive(true);
        _stanceSwitchIcon.sprite = CurrentStance.StanceIcon;
        yield return new WaitForSeconds(_stanceSwitchPopupTime);
        _stanceSwitchPopup.SetActive(false);
    }

    void UpdateAttackIndicator()
    {
        foreach (var item in _stanceAttackIndicators)
            item.SetActive(false);

        switch (CurrentStance.StanceType)
            {
                case EStance.Earth:
                    _stanceAttackIndicators[0].SetActive(true);
                    break;
                case EStance.Water:
                    _stanceAttackIndicators[1].SetActive(true);
                    break;
                case EStance.Air:
                    _stanceAttackIndicators[2].SetActive(true);
                    break;
                case EStance.Fire:
                    _stanceAttackIndicators[3].SetActive(true);
                    break;
            }
    }
    #endregion

    void ProcessOpenAugmentMenu(bool open)
    {
        // _augmentMenu.SetActive(open);
    }

    #region UpdatingPointerInfo
    public void UpdatePointerInfo()
    {
        if (CurrentStance.StanceType == EStance.Air)
            ProcessUpdatePointerPosition(_playerMap.FindAction("MousePosition").ReadValue<Vector2>());
        else
            ProcessUpdatePointerDirection(_playerMap.FindAction("MousePosition").ReadValue<Vector2>());
        UpdateAnimatorControllerStates();
    }

    private float _hitboxPointerOriginalXRotation = 0.0f;
    void ProcessUpdatePointerDirection(Vector2 position)
    {
        _hitboxPointer.transform.localPosition = new(0.0f, 0.0f, 0.0f);
        _hitboxPointer.SetActive(true);

        float angle = ToIsoRotation(position);
        Quaternion rot = Quaternion.Euler(_hitboxPointerOriginalXRotation, - angle - 45, 0.0f);
        _hitboxPointer.transform.rotation = rot;
        
        if(_stateHandler.CurrentState == EntityState.Attack)
            _animator.SetDirection(angle >= -90 && angle <= 90 ? LookDirection.Right : LookDirection.Left);
    }
    float ToIsoRotation(Vector2 position) {
        Vector3 tempVector = Camera.main.WorldToScreenPoint(transform.position);
        tempVector = (Vector3)position - tempVector;
        return Mathf.Atan2(tempVector.y, tempVector.x) * Mathf.Rad2Deg;
    }

    void ProcessUpdatePointerPosition(Vector2 position)
    {
        if (_attackAnimCallback._isAttacking)
        {
            _hitboxPointer.transform.localPosition = new(0.0f, 0.0f, 0.0f);
            _hitboxPointer.SetActive(true);
            return;
        }

        float angle = ToIsoRotation(position);
        if(_stateHandler.CurrentState == EntityState.Attack)
            _animator.SetDirection(angle >= -90 && angle <= 90 ? LookDirection.Right : LookDirection.Left);

        RaycastHit raycastHit;
        Ray ray = _mainCamera.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity, _targetableMask))
        {
            Vector3 targetPosition = raycastHit.point;
            targetPosition.y = _hitboxPointer.transform.position.y;
            _hitboxPointer.transform.position = targetPosition;
            _hitboxPointer.SetActive(true);

        }
        else
            _hitboxPointer.SetActive(false);
            
    }

    void UpdateAnimatorControllerStates()
    {
        // _animator.SetMovement(entityMovement);
        // if (_stateHandler.IsDead)
        // {
        //     _stateHandler.CurrentState = EntityState.Dead;
        // }
        // _animator.SetState(_stateHandler.CurrentState);

        // _animator.RevampUpdatePlayerState(_stateHandler.CurrentState, _stateHandler.IsHurt);
    }
    #endregion
    void ResetState()
    {
        _comboCount = -1;
        _timeOfLastAttack = Time.time;
        _timeOfLastDash = Time.time;
        _chargingStartTime = Time.time;
        _chargeTime = 0.0f;
        _queuedAttack = null;
        _queuedAttackIsSpecial = false;
        _lastPerformedAttackIsSpecial = false;
        _lastMoveInput = Vector3.zero;
        _isDashing = false;
        _holdingAnAttack = false;
        ResetSpeed();
        UpdateAttackIndicator();

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
        _stateHandler = GetComponent<RevampPlayerStateHandler>();
        _hitboxPointerOriginalXRotation = _hitboxPointer.transform.rotation.eulerAngles.x;
        _rigidbody = GetComponent<Rigidbody>();
        ResetState();

        if (ItemManager.Instance != null && ItemManager.Instance.UnlockedStanceCount <= 0)
        {
            switch (CurrentStance.StanceType)
            {
                case EStance.Earth:
                    ItemManager.Instance.AddAugment(AugmentType.Earth);
                    ProcessStanceSwitch();
                    break;
                case EStance.Water:
                    ItemManager.Instance.AddAugment(AugmentType.Water);
                    ProcessStanceSwitch();
                    break;
                case EStance.Air:
                    ItemManager.Instance.AddAugment(AugmentType.Air);
                    ProcessStanceSwitch();
                    break;
                case EStance.Fire:
                    ItemManager.Instance.AddAugment(AugmentType.Fire);
                    ProcessStanceSwitch();
                    break;
                case EStance.None:
                    ItemManager.Instance.AddAugment(AugmentType.Earth);
                    ProcessStanceSwitch();
                    break;
            }
        }
    }

    void OnDisable()
    {
        _inputActions.Disable();
    }

}
