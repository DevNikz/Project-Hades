using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Takes over the health and mana side of player controller as well as death
/// </summary>
public class RevampPlayerStateHandler : MonoBehaviour
{
    [SerializeField] private PlayerAttackAnimCallback _attackAnimCallback;
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Slider _manaBar;
    [SerializeReference] private PlayerCombatStats _playerCombatStats;
    [SerializeField] private float _hurtTime;

    public EntityState CurrentState;
    public float CurrentHealth { get; private set; }
    public float CurrentCharge { get; private set; }
    private float _maxHealth;
    private float _maxCharge;
    public bool IsHurt { get; private set; }
    private float _invincibilitySetTime = 0.0f;
    private bool IsInvincible
    {
        get { return _invincibilitySetTime >= Time.time; }
    }

    public bool IsDead
    {
        get { return CurrentHealth <= 0; }
    }

    public bool godMode;

    public bool IsGod
    {
        get { return godMode; }
        set { godMode = value;  }
    }

    public static RevampPlayerStateHandler Instance { get; private set; }
    private PlayerAnimatorController _animator;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        //Components
        _healthBar = GameObject.Find("PlayerHealth")?.GetComponent<Slider>();
        _manaBar = GameObject.Find("PlayerMana")?.GetComponent<Slider>();
        _animator = GetComponent<PlayerAnimatorController>();

        ResetStatus();

        SceneManager.sceneLoaded += OnSceneLoad;
    }

    public void ResetStatus()
    {
        SetBonusHealth(0);
        _maxCharge = _playerCombatStats.BaseMaxCharge;
        _maxCharge = _playerCombatStats.BaseMaxCharge;

        CurrentHealth = _maxHealth;
        CurrentCharge = 0;

        UpdateHealthbar();
        UpdateManabar();
    }

    public void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        UpdateHealthbar();
        UpdateManabar();

    }

    public void SetInvincibility(float amount)
    {
        if (_invincibilitySetTime < Time.time + amount)
            _invincibilitySetTime = Time.time + amount;
    }

    public void ReceiveDamage(DamageType damageType, float damage)
    {
        if (CurrentState == EntityState.Dead) return;
        if (IsInvincible || _attackAnimCallback._isInvulnerable) return;
        if (godMode) return;
 
        // HURT DAMAGE CALCULATION
        float actualDamage = damage;
        float damageReduction = 0.0f;
        damageReduction += ItemManager.Instance.getAugmentCount(AugmentType.Steel) * ItemManager.Instance.getAugment(AugmentType.Steel).augmentPower;
        if (ItemManager.Instance.getAugment(AugmentType.Fog_Gear).IsActive)
            damageReduction += ItemManager.Instance.getAugment(AugmentType.Fog_Gear).augmentPower;

        actualDamage *= 1.0f - damageReduction;
        CurrentHealth -= actualDamage;

        if (TryGetComponent<FlashSpriteScript>(out var flashScript))
            flashScript.TriggerFlash(actualDamage, true);

        // SFX SETTINGS
        TriggerRandomHurtSFX();
        TriggerHurt();

        // UPDATE HEALTHBAR
        UpdateHealthbar();
        DeathCheck();

        IsHurt = true;
        SetInvincibility(_hurtTime);
        Invoke(nameof(ResetHurt), _hurtTime);
    }

    private void ResetHurt()
    {
        IsHurt = false;
    }

    void TriggerRandomHurtSFX()
    {
        if(SFXManager.Instance != null)
            SFXManager.Instance.PlaySFXAtPosition($"Player_Hurt_{Random.Range(1, 3)}", transform.position);
        //SFXManager.Instance.Play($"PlayerHurt{Random.Range(1, 3)}");
    }

    public void ResetHealth()
    {
        _animator.RevampPlayIdle();
        CurrentState = EntityState.None;
        gameObject.tag = "Player";
        CurrentHealth = _maxHealth;
        UpdateHealthbar();
    }
    public void ResetCharge()
    {
        CurrentCharge = _maxHealth;
        UpdateManabar();
    }

    void UpdateHealthbar()
    {
        _healthBar.value = CurrentHealth / _maxHealth;
    }

    void UpdateManabar()
    {
        _manaBar.value = CurrentCharge / _maxCharge;
    }

    void DeathCheck()
    {
        if (IsDead)
        {
            CurrentState = EntityState.Dead;
            TriggerDeath();
        }
    }

    public void GiveCharge(float amount)
    {
        if (amount < 0) return;

        if (CurrentCharge + amount > _maxCharge)
            amount = _maxCharge - CurrentCharge;

        CurrentCharge += amount;

        UpdateManabar();
    }

    public void UseCharge(float amount)
    {
        if (amount < 0) return;

        if (CurrentCharge - amount < 0)
            amount = CurrentCharge;

        CurrentCharge -= amount;

        UpdateManabar();
    }

    public void HealHealth(float amount)
    {
        if (amount < 0) return;

        if (CurrentHealth + amount > _maxHealth)
            amount = _maxHealth - CurrentHealth;

        CurrentHealth += amount;

        UpdateHealthbar();
        DeathCheck();
    }

    public void SetBonusHealth(float value)
    {
        //Set Total Health
        _maxHealth = _playerCombatStats.BaseMaxHealth;
        _maxHealth += value;

        if (CurrentHealth > _maxHealth)
            CurrentHealth = _maxHealth;

    }

    void TriggerHurt()
    {
        _animator.RevampTriggerHurt();
    }

    void TriggerDeath()
    {
        gameObject.tag = "Player(Dead)";
        _animator.RevampTriggerDeath();
    }
}
