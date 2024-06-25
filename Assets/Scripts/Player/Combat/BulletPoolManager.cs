using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : APoolable
{
    [SerializeField] private Rigidbody bulletRB;
    [SerializeField] private float bulletSpeed = 10.0f;

    private const float Y_BOUNDARY = 10f;
    private Vector3 originPos;

    private void Awake() {
        this.originPos = this.transform.position;
    }

    private void Update() {
    }

    public override void Initialize()
    {
        this.transform.localScale = new Vector3(0.168f, 0.53f, 0.168f);
    }

    public override void OnActivate()
    {
        this.transform.position = bulletRB.transform.position;
        this.transform.rotation = this.transform.parent.rotation;
        this.bulletRB.AddForce(this.transform.up * bulletSpeed, ForceMode.Impulse);
    }

    public override void Release()
    {
        this.bulletRB.velocity = Vector3.zero;
    }
}

    // //Add Shooting (Left Click)
    // private void ShootDebug(Parameters parameters) {
    //     leftPress = parameters.GetBoolExtra(LEFT_CLICK_PRESS, false);

    //     if(leftPress) {
    //         GameObject tempObject = Instantiate(bullet, bullet.transform.position, this.transform.rotation);
    //         tempObject.transform.localScale = new Vector3(0.168f,0.53f,0.168f);

    //         Rigidbody rb = tempObject.GetComponent<Rigidbody>();
    //         tempObject.SetActive(true);
    //         rb.AddForce(tempObject.transform.up * 10f, ForceMode.Impulse);

    //     }
    // }