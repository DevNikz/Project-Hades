using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public Vector2 normalisedMousePosition;
    public float currentAngle;
    public int selection;
    private int previousSelection = -1;

    public GameObject[] menuItems;

    private MenuItemScript menuItemSc;
    private MenuItemScript previousMenuItemSc;

    private bool isTabHeld = false;
    public bool wheelHeld;
    public bool wheelReleased;

    public const string WHEEL_PRESS = "WHEEL_PRESS";
    public const string WHEEL_HOLD = "WHEEL_HOLD";

    public static int LastSelection { get; private set; } = -1;

    private void Start()
    {
        foreach (GameObject item in menuItems)
        {
            item.SetActive(false);
        }
    }    

    private void Update()
    {
        // Check if Tab key is pressed
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isTabHeld = true;
            ShowMenuItems();
        }

        // Check if Tab key is released
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            isTabHeld = false;
            HideMenuItems();
        }

        if (isTabHeld)
        {
            normalisedMousePosition = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2);
            currentAngle = Mathf.Atan2(normalisedMousePosition.y, normalisedMousePosition.x) * Mathf.Rad2Deg;

            currentAngle = (currentAngle + 360.0f) % 360;
            selection = (int)(currentAngle / 72);

            if (selection >= 0 && selection < menuItems.Length)
            {
                if (selection != previousSelection)
                {
                    if (previousSelection >= 0 && previousSelection < menuItems.Length)
                    {
                        previousMenuItemSc = menuItems[previousSelection].GetComponent<MenuItemScript>();
                        previousMenuItemSc.Deselect();
                    }

                    previousSelection = selection;
                    menuItemSc = menuItems[selection].GetComponent<MenuItemScript>();
                    menuItemSc.Select();
                    
                    LastSelection = selection;
                }
            }
        }
    }




        private void ShowMenuItems()
    {
        foreach (GameObject item in menuItems)
        {
            item.SetActive(true);
        }
    }

    private void HideMenuItems()
    {
        foreach (GameObject item in menuItems)
        {
            item.SetActive(false);
        }

        // Keep the last selection highlighted
        if (previousSelection >= 0 && previousSelection < menuItems.Length)
        {
            menuItemSc = menuItems[previousSelection].GetComponent<MenuItemScript>();
            menuItemSc.Select();
        }
    }
}
