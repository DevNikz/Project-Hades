using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MaskObject : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject target;
    [SerializeField] private LayerMask myLayerMask;

    void Update() {
        RaycastHit hit;

        if(Physics.Raycast(mainCamera.transform.position, 
        (target.transform.position - mainCamera.transform.position).normalized, 
        out hit, Mathf.Infinity, myLayerMask)) {
            Debug.Log(hit.collider.name);

            if (hit.collider.gameObject.tag == "Mask") {
                target.transform.DOScale(0, 2);
            }
            else {
                //target.transform.DOScale(4 ,2);
                target.transform.DOScaleX(3, 2);
                target.transform.DOScaleY(1.5f, 2);
                target.transform.DOScaleZ(3, 2);
            }
        }
    }
}
