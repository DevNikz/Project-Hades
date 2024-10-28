using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentMenuScript : MonoBehaviour
{
    [SerializeField] GameObject augmentMenu;

    // Start is called before the first frame update
    void Start()
    {
        augmentMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
            augmentMenu.SetActive(true);
        else if (Input.GetKeyUp(KeyCode.Tab))
            augmentMenu.SetActive(false);
    }
}
