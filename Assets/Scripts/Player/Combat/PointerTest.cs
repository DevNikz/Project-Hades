using UnityEngine;

public class PointerTest : MonoBehaviour
{
    //Player Ref
    private GameObject player;   
    private GameObject bullet;
    [SerializeField] private GameObjectPool bulletPool;
    
    //Pointer
    private Vector3 orbVector;
    private float angle;
    public Quaternion rot;
    public Quaternion tempRot;
    private float _xRotation;

    //Input
    private bool leftPress;
    private bool rightPress;

    //Broadcaster
    public const string LEFT_CLICK_PRESS = "LEFT_CLICK_PRESS";
    public const string RIGHT_CLICK_PRESS = "RIGHT_CLICK_PRESS";

    private void Awake() {
        _xRotation = transform.rotation.eulerAngles.x;
        player = transform.parent.gameObject;

        bullet = transform.Find("Bullet").gameObject;
        bullet.SetActive(false);
    }

    private void Start() {
        this.bulletPool.Initialize();
        EventBroadcaster.Instance.AddObserver(EventNames.MouseInput.LEFT_CLICK_PRESS, this.ShootDebug);
    }

    private void OnDestroy() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.MouseInput.LEFT_CLICK_PRESS);
        EventBroadcaster.Instance.RemoveObserver(EventNames.MouseInput.RIGHT_CLICK_PRESS);
    }
    
    private void Update() {
        this.gameObject.transform.position = new Vector3(player.transform.position.x, 0.05f, player.transform.position.z);

        toIsoRotation();
        rot = Quaternion.Euler(_xRotation, -angle - 45, 0.0f);
        transform.rotation = rot;
    }

    private void toIsoRotation() {
        orbVector = Camera.main.WorldToScreenPoint(this.transform.position);
        orbVector = Input.mousePosition - orbVector;
        angle = Mathf.Atan2(orbVector.y, orbVector.x) * Mathf.Rad2Deg;
    }

    // private void ShootDebug(Parameters parameters) {
    //     leftPress = parameters.GetBoolExtra(LEFT_CLICK_PRESS, false);
    //     if(leftPress) {
    //         this.bulletPool.RequestPoolable();
    //     }
    // }

    //Add Shooting (Left Click)
    private void ShootDebug(Parameters parameters) {
        leftPress = parameters.GetBoolExtra(LEFT_CLICK_PRESS, false);

        if(leftPress) {
            GameObject tempObject = Instantiate(bullet, bullet.transform.position, this.transform.rotation);
            tempObject.transform.localScale = new Vector3(0.168f,0.53f,0.168f);

            Rigidbody rb = tempObject.GetComponent<Rigidbody>();
            tempObject.SetActive(true);
            rb.AddForce(tempObject.transform.up * 10f, ForceMode.Impulse);
        }
    }

}
