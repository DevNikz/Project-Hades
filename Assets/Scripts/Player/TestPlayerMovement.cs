using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    //[SerializeField] private float impulse = 5.0f;
    //[SerializeField] private float force = 12.0f;
    
    private bool keyPressed = false;

    private bool moveForward = false;
    private bool moveBackward = false;
    private bool moveLeft = false;
    private bool moveRight = false;

    Event Event;

    private enum Direction
    {
        FORWARD,
        BACKWARD,
        LEFT,
        RIGHT,
        NONE
    }

    private Direction direction = Direction.NONE;

    // Start is called before the first frame update
    void Start()
    {
        //this.gameObject.GetComponent<Collider>().isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        this.InputListen();
        this.Move();
    }

    void InputListen()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            this.direction = Direction.FORWARD;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            this.direction = Direction.BACKWARD;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            this.direction = Direction.LEFT;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            this.direction = Direction.RIGHT;
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            this.direction = Direction.NONE;
        }
    }

    void Move()
    {
        if (this.direction == Direction.FORWARD)
            this.gameObject.transform.Translate(Vector3.forward * Time.deltaTime * this.speed);

        if (this.direction == Direction.BACKWARD)
            this.gameObject.transform.Translate(Vector3.back * Time.deltaTime * this.speed);

        if (this.direction == Direction.LEFT)
            this.gameObject.transform.Translate(Vector3.left * Time.deltaTime * this.speed);

        if (this.direction == Direction.RIGHT)
            this.gameObject.transform.Translate(Vector3.right * Time.deltaTime * this.speed);
    }


    /*private void OnTriggerEnter(Collider other)
    {
        if (Input.GetKey(KeyCode.E)) //Input.GetKey(KeyCode.E)
        {
            other.attachedRigidbody.AddForce(Vector3.up * impulse, ForceMode.Impulse);
        }
    }

    private void OnTriggerExit(Collider other)
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.E)) //Input.GetKey(KeyCode.E)
        {
            other.attachedRigidbody.AddForce(Vector3.up * force, ForceMode.Acceleration);
        }
    }*/
}
