using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMoveTest : MonoBehaviour
{
    private Vector3 tempVector;
    public const string MOUSE_POS = "MOUSE_POS";

    private void Start() {
        EventBroadcaster.Instance.AddObserver(EventNames.MouseInput.MOUSE_POS, this.LookEvent);
    }

    private void Update() {
        // Get the mouse position
        Vector3 mousePos = Input.mousePosition;

        // Set the z component of the mouse position to the absolute distance between the camera's z and the object's z.
        mousePos.z = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);

        // Determine the world position of the mouse pointer
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        // Update the position of the object
        transform.position = worldPos;

        
        //this.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, this.transform.position.y, -Camera.main.transform.position.z));
    }

    private void LookEvent(Parameters parameters) {
        tempVector = parameters.GetVector3Extra(MOUSE_POS, Vector3.zero);

        Vector3 temp = new Vector3(tempVector.x, this.transform.position.y, tempVector.z);

        //this.transform.position = Vector3.MoveTowards(this.transform.position, temp, 1f);

        /*
        rigidbody.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        */
    }
}
