using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enable : MonoBehaviour
{

    [SerializeField] GameObject go;

    // Start is called before the first frame update
    void Start()
    {
        go.SetActive(true);
    }

}
