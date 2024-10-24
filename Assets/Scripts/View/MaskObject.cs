using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class MaskObject : MonoBehaviour
{
    [PropertySpace, TitleGroup("Properties", "", TitleAlignments.Centered)] 
    [InfoBox("Assign Wall / Obstruction Layer and the Mask layer. Walls and Obstruction objects need to be assigned with the same Wall / Obstruction Layer")]
    [SerializeField] private LayerMask myLayerMask;

    [PropertySpace, TitleGroup("Reference", "", TitleAlignments.Centered)] 
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject target;


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
