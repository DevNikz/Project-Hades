using UnityEngine;

public class PointerTest : MonoBehaviour
{
    [SerializeReference] private GameObject player;   
    private Vector3 orbVector;
    public float angle;
    private Quaternion rot;
    public const string MOUSE_POS = "MOUSE_POS";

    private float _xRotation;

    private void Awake() {
        _xRotation = transform.rotation.eulerAngles.x;
    }

    private void Update() {
        this.gameObject.transform.position = new Vector3(player.transform.position.x, 0.05f, player.transform.position.z);
        
        //rotate on an axis using mouse position (Isometric)
        orbVector = Camera.main.WorldToScreenPoint(player.transform.position);
        orbVector = Input.mousePosition - orbVector;
        angle = Mathf.Atan2(orbVector.y, orbVector.x) * Mathf.Rad2Deg;
        rot = Quaternion.Euler(_xRotation, -angle - 45, 0.0f);
        transform.rotation = rot;

        //2D (For Future Reference)
        // orbVector = Camera.main.WorldToScreenPoint(this.transform.position);
        // orbVector = Input.mousePosition - orbVector;
        // angle = Mathf.Atan2(orbVector.y, orbVector.x) * Mathf.Rad2Deg;
        // this.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }
}
