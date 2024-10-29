using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour {

    public static Movement Instance;
    
    [PropertySpace] [TitleGroup("Properties", "General Movement Properties", TitleAlignments.Centered)]
    [AssetSelector]
    public PlayerStatsScriptable movement;

    [HorizontalGroup("Properties/Group")]
    [VerticalGroup("Properties/Group/Left")]
    [BoxGroup("Properties/Group/Left/Box1", ShowLabel = false)]
    [LabelWidth(125)]
    [ReadOnly, SerializeReference] public float strafeSpeed;

    [BoxGroup("Properties/Group/Left/Box1", ShowLabel = false)]
    [LabelWidth(125)]
    [ReadOnly, SerializeReference] public float currentSpeed;

    [BoxGroup("Properties/Group/Left/Box1", ShowLabel = false)]
    [LabelWidth(125)]
    [SerializeReference, HideLabel] private Vector3 currentInput;

    
    [BoxGroup("Properties/Group/Left/Box1", ShowLabel = false)]
    [LabelWidth(125)]
    [ReadOnly, SerializeReference] private bool dashing;

    [BoxGroup("Properties/Group/Left/Box1", ShowLabel = false)]
    [LabelWidth(125)]
    [ReadOnly, SerializeReference] private float dashCDTimer;

    [BoxGroup("Properties/Group/Left/Box1", ShowLabel = false)]
    [LabelWidth(125)]
    [ReadOnly, SerializeReference, HideLabel] private Vector3 delayedForce;

    [BoxGroup("Properties/Group/Left/Box1", ShowLabel = false)]
    [LabelWidth(125)]
    [ReadOnly, SerializeReference, HideLabel] private Vector3 isoInput;

    [VerticalGroup("Properties/Group/Right")]
    [BoxGroup("Properties/Group/Right/Box1", ShowLabel = false)]
    [LabelWidth(125)]
    [ReadOnly, SerializeReference] public EntityMovement move = EntityMovement.Idle;

    [BoxGroup("Properties/Group/Right/Box1", ShowLabel = false)]
    [LabelWidth(125)]
    [ReadOnly, SerializeReference] public EntityState state;

    [BoxGroup("Properties/Group/Right/Box1", ShowLabel = false)]
    [LabelWidth(125)]
    [ReadOnly, SerializeReference] public EntityDirection direction;

    [BoxGroup("Properties/Group/Right/Box1", ShowLabel = false)]
    [LabelWidth(125)]
    [ReadOnly, SerializeReference] public LookDirection lookDirection;

    [PropertySpace] [TitleGroup("References", "General Movement References", TitleAlignments.Centered)]
    [HorizontalGroup("References/Group")]
    [VerticalGroup("References/Group/Left")]
    [BoxGroup("References/Group/Left/Box", ShowLabel = false)]
    [LabelWidth(125)]
    [ReadOnly, SerializeReference] private Rigidbody rigidBody;

    [BoxGroup("References/Group/Left/Box", ShowLabel = false)]
    [LabelWidth(125)]
    [ReadOnly, SerializeReference] private Transform model;

    [BoxGroup("References/Group/Left/Box", ShowLabel = false)]
    [LabelWidth(125)]
    [ReadOnly, SerializeReference] private Combat combat;

    [VerticalGroup("References/Group/Right")]
    [BoxGroup("References/Group/Right/Box", ShowLabel = false)]
    [LabelWidth(125)]
    [ReadOnly, SerializeReference] private ParticleSystem dust;

    [BoxGroup("References/Group/Right/Box", ShowLabel = false)]
    [LabelWidth(125)]
    [ReadOnly, SerializeReference] protected GameObject pointerUI;

    [BoxGroup("References/Group/Right/Box", ShowLabel = false)]
    [LabelWidth(125)]
    [ReadOnly, SerializeReference] private ParticleSystem dashParticle;

    [BoxGroup("References/Group/Right/Box", ShowLabel = false)]
    [LabelWidth(125)]
    [ReadOnly, SerializeReference] private Vector3 moveInput;

    [BoxGroup("References/Group/Right/Box", ShowLabel = false)]
    [LabelWidth(125)]
    [ReadOnly, SerializeReference, HideLabel] private float moveInput_normalized;

    [BoxGroup("References/Group/Right/Box", ShowLabel = false)]
    [LabelWidth(125)]
    [ReadOnly, SerializeReference] private bool dashInput;

    private Vector3 tempVector;
    Quaternion rot;
    float angle;

    //Broadcaster
    public const string KEY_MOVE = "KEY_MOVE";
    
    public const string KEY_DASH = "KEY_DASH";

    public const string KEY_MOVE_HELD = "KEY_MOVE_HELD";
    public const string RIGHT_CLICK = "RIGHT_CLICK";

    void Awake() {
        if(Instance == null) {
            Instance = this;
        }
        else Destroy(this);
    }

    void LoadData() {
        LoadUI();
        LoadComponents();
        LoadParticles();
        LoadParams();
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.KeyboardInput.KEY_INPUTS);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) { 
        EventBroadcaster.Instance.AddObserver(EventNames.KeyboardInput.KEY_INPUTS, this.stateHandlerEvent);
        LoadData(); 
    }

    void LoadUI() {
        pointerUI = transform.Find("AttackColliders").gameObject;
    }

    void LoadComponents() {
        if(!combat) combat = this.GetComponent<Combat>();
        if(!movement) movement = Resources.Load<PlayerStatsScriptable>("Player/General/PlayerMovement");
        if(!rigidBody) rigidBody = this.GetComponent<Rigidbody>();
        if(!model) model = this.GetComponent<Transform>();
        GetComponent<CapsuleCollider>().material = Resources.Load<PhysicMaterial>("Player/Player");
    }

    void LoadParticles() {
        if(!dust) dust = transform.Find("GroundDust").gameObject.GetComponent<ParticleSystem>();
        dust.Play();
    }

    void LoadParams() {
        strafeSpeed = movement.strafeSpeed;
    }

    private void Update() {
        UpdatePointer();
        UpdateLookDirection();

        //Checks
        CheckDrag();
        CheckMove();
        CheckDash();

        //Init Dash Funcs
        Cooldown();

        PlayerController.Instance.SetMovement(move);
        PlayerController.Instance.SetDirection(lookDirection);
        PlayerController.Instance.SetDashing(dashing);
    }

    void Move(Vector3 input) {
        moveInput_normalized = input.normalized.magnitude;
        currentInput = transform.localPosition + input.ToIso() * moveInput_normalized * currentSpeed * Time.fixedDeltaTime;
        rigidBody.MovePosition(transform.localPosition + input.ToIso() * moveInput_normalized * currentSpeed * Time.fixedDeltaTime);
    }

    private void CheckMove() {
        ParticleSystem.EmissionModule temp = dust.emission;
        if(move == EntityMovement.Strafing) temp.enabled = true;
        else temp.enabled = false;
    }

    private void CheckDash() {
        if(dashing) dashParticle.Play();
    }

    private void stateHandlerEvent(Parameters parameters) {
        moveInput = parameters.GetVector3Extra(KEY_MOVE, Vector3.zero);
        dashInput = parameters.GetBoolExtra(KEY_DASH, false);

        if(moveInput.x != 0 || moveInput.z != 0) {
            if(dashInput) {
                Dash();
            }
            else {
                //Set To Strafing
                move = EntityMovement.Strafing;

                //Set To Strafing Speed
                currentSpeed = strafeSpeed;

                //Debug Direction
                direction = IsoCompass(moveInput.x, moveInput.z);

                Move(moveInput);
            }
        }

        else {
            move = EntityMovement.Idle;
        }
    }

    bool IsHurt() { return PlayerController.Instance.IsHurt(); }

    private void CheckDrag() {
        if(move == EntityMovement.Strafing) {
            rigidBody.drag = movement.groundDrag;
        }
        else rigidBody.drag = 10f;
    }

    private EntityDirection IsoCompass(float x, float z) {
        //North
        if(x == 0 && (z <= 1 && z > 0)) {
            return EntityDirection.North;
        }

        //North East
        else if((x <= 1 && x > 0) && (z <= 1 && z > 0)) {
            return EntityDirection.NorthEast;
        }

        //East
        else if((x <= 1 && x > 0) && z == 0) {
            return EntityDirection.East;
        }

        //South East
        else if((x <= 1 && x > 0) && (z >= -1 && z < 0)) {
            return EntityDirection.SouthEast;
        }

        //South
        else if(x == 0 && (z >= -1 && z < 0)) {
            return EntityDirection.South;
        }

        //South West
        else if((x >= -1 && x < 0) && (z >= -1 && z < 0)) {
            return EntityDirection.SouthWest;
        }

        //West
        else if((x >= -1 && x < 0) && z == 0) {
            return EntityDirection.West;
        }

        //North West
        else if((x >= -1 && x < 0) && (z <= 1 && z > 0)) {
            return EntityDirection.NorthWest;
        }

        else return direction;
    }

    private void Dash() {
        if(dashCDTimer > 0 ) return;
        else dashCDTimer = movement.dashCooldown;

        //Set Dash To True
        dashing = true;

        //Convert World View Coords To Iso Coords
        isoInput = this.ConvertToIso(moveInput.x, moveInput.z);

        //Apply Dash Based On KeyInput
        delayedForce = isoInput * movement.dashForce; 

        //Duration
        DelayedDashForce();
        //Invoke(nameof(DelayedDashForce), movement.dashDuration);

        //Cooldown
        Invoke(nameof(ResetDash), movement.dashDuration);
    }

    private Vector3 ConvertToIso(float x, float z) {

        //North
        if(x == 0 && (z <= 1 && z > 0)) return new Vector3(1f, 0f, 1f);

        //North East
        else if((x <= 1 && x > 0) && (z <= 1 && z > 0)) return new Vector3(1f, 0f, 0f);

        //East
        else if((x <= 1 && x > 0) && z == 0) return new Vector3(1f, 0f, -1f);

        //South East
        else if((x <= 1 && x > 0) && (z >= -1 && z < 0)) return new Vector3(0f, 0f, -1f);

        //South
        else if(x == 0 && (z >= -1 && z < 0)) return new Vector3(-1f, 0f, -1f);

        //South West
        else if((x >= -1 && x < 0) && (z >= -1 && z < 0)) return new Vector3(-1f, 0f, 0f);

        //West
        else if((x >= -1 && x < 0) && z == 0) return new Vector3(-1f, 0f, 1f);

        //North West
        else if((x >= -1 && x < 0) && (z <= 1 && z > 0)) return new Vector3(0f, 0f, 1f);

        else {
            Vector3 zero = Vector3.zero;
            return zero;
        }
    }

    void UpdateLookDirection() {
        if(angle >= 0 && angle <= 90) lookDirection = LookDirection.Right;
        else if(angle <= 0 && angle >= -90) lookDirection = LookDirection.Right;
        else lookDirection = LookDirection.Left;
    }

    void UpdatePointer() {
        pointerUI.transform.position = new Vector3(this.transform.position.x, 0.05f, this.transform.position.z);
        toIsoRotation();
        rot = Quaternion.Euler(pointerUI.transform.rotation.eulerAngles.x, -angle-45, 0.0f);
        pointerUI.transform.rotation = rot;
    }

    void toIsoRotation() {
        tempVector = Camera.main.WorldToScreenPoint(pointerUI.transform.position);
        tempVector = Input.mousePosition - tempVector;
        angle = Mathf.Atan2(tempVector.y, tempVector.x) * Mathf.Rad2Deg;
    }


    private void DelayedDashForce() {
        rigidBody.AddForce(delayedForce * 100 * Time.fixedDeltaTime, ForceMode.Impulse);
    }

    private void ResetDash() {
        dashing = false;
    }

    private void Cooldown() {
        if(dashCDTimer > 0) dashCDTimer -= Time.fixedDeltaTime;
    }

    public void SetSpeed(float value) {
        strafeSpeed = value;
    }

    public float GetSpeed() { 
        if(movement != null) return movement.strafeSpeed; 
        return 0;
    }
}

