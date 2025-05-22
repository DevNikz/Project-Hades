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
        EStance selection = PlayerStanceManager.Instance.SelectedStance;

        switch(selection)
        {
            case EStance.Earth: //Earth - Green
                newColor = Color.green;//new Color(0.5f, 1.0f, 0.5f, 1.0f);
                break;
            case EStance.Water: //Water - Blue
                newColor = new Color(0.7f, 0.7f, 1.0f, 1.0f); //Color.blue is too dark, Color.cyan is too green
                break;
            case EStance.Air: //Wind - Yellow?
                newColor = Color.yellow;
                break;
            case EStance.Fire: //Fire - Red
                newColor = Color.red;//new Color(1.0f, 0.5f, 0.5f, 1.0f);
                break;
        }
        rend.color = newColor;
    }
}