using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    
    public Vector2 normalisedMousePosition;
    public float currentAngle;
    public int selection;
    private int previousSelection = -1;

    public Image displayImage;
    public Sprite[] images; 

    public GameObject[] menuItems;
    public GameObject panel;

    private MenuItemScript menuItemSc;
    private MenuItemScript previousMenuItemSc;

    protected static bool isTabHeld = false;
    public bool wheelHeld;
    public bool wheelReleased;

    public const string WHEEL_PRESS = "WHEEL_PRESS";
    public const string WHEEL_HOLD = "WHEEL_HOLD";

    protected static int lastSelected = 0;

    [SerializeReference] bool debug = false;

    public static bool weaponWheelCheck {
        get { return isTabHeld; }
        set { isTabHeld = value; }
    }

    public static int LastSelection {
        get { return lastSelected; }
        set { lastSelected = value; }
    }

    private void Start()
    {
        foreach (GameObject item in menuItems)
        {
            item.SetActive(false);
        }
        panel.SetActive(false);
        displayImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(AugmentMenuScript.augmentMenuCheck == false && PlayerController.Instance.entityState != EntityState.Attack) DoAction();
    }

    void DoAction() {
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
            selection = (int)(currentAngle / 90); //changed from 72

            if (selection >= 0 && selection < menuItems.Length)
            {
                if (selection != previousSelection)
                {
                    if (previousSelection >= 0 && previousSelection < menuItems.Length)
                    {
                        previousMenuItemSc = menuItems[previousSelection].GetComponent<MenuItemScript>();
                        previousMenuItemSc.Deselect();
                    }

                    /*switch(selection)
                    {
                        case 1:
                            if (!ItemManager.Instance.Water)
                                selection = 0;
                            break;
                        case 2:
                            if (!ItemManager.Instance.Wind && previousSelection >= 0)
                                selection = previousSelection;
                            else if (previousSelection == -1)
                                selection = 0;
                            break;
                        case 3:
                            if(!ItemManager.Instance.Fire && previousSelection >= 0)
                                selection = previousSelection;
                            else if (previousSelection == -1)
                                selection = 0;
                            break;
                    }*/

                    Debug.Log(selection);

                    previousSelection = selection;
                    menuItemSc = menuItems[selection].GetComponent<MenuItemScript>();
                    menuItemSc.Select();

                    LastSelection = selection;

                    // Update the displayed image based on the selection
                    UpdateDisplayImage(selection);
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
        panel.SetActive(true);
        displayImage.gameObject.SetActive(true);

    }

    private void HideMenuItems()
    {
        foreach (GameObject item in menuItems)
        {
            item.SetActive(false);
        }

        panel.SetActive(false);
        displayImage.gameObject.SetActive(false);

        // Keep the last selection highlighted
        if (previousSelection >= 0 && previousSelection < menuItems.Length)
        {
            menuItemSc = menuItems[previousSelection].GetComponent<MenuItemScript>();
            menuItemSc.Select();
        }
    }

    private void UpdateDisplayImage(int selection)
    {
        if (selection >= 0 && selection < images.Length)
        {
            displayImage.sprite = images[selection];

            switch (selection)
            {
                case 0:
                    displayImage.rectTransform.localPosition = new Vector2(125, 125);
                    break;
                case 1:
                    displayImage.rectTransform.localPosition = new Vector2(-125, 125);
                    break;
                case 2:
                    displayImage.rectTransform.localPosition = new Vector2(-125, -125);
                    break;
                case 3:
                    displayImage.rectTransform.localPosition = new Vector2(125, -125);
                    break;

            }
        }
    }
}
