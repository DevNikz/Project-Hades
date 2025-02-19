using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public Color newColor;
    private SpriteRenderer rend;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.color = Color.white;
    }

    void Update()
    {
        int selection = MenuScript.LastSelection;

        switch(selection)
        {
            case 0: //Earth - Green
                newColor = Color.green;//new Color(0.5f, 1.0f, 0.5f, 1.0f);
                break;
            case 1: //Water - Blue
                newColor = new Color(0.7f, 0.7f, 1.0f, 1.0f); //Color.blue is too dark, Color.cyan is too green
                break;
            case 2: //Wind - Yellow?
                newColor = Color.yellow;
                break;
            case 3: //Fire - Red
                newColor = Color.red;//new Color(1.0f, 0.5f, 0.5f, 1.0f);
                break;
        }
        rend.color = newColor;
    }
}