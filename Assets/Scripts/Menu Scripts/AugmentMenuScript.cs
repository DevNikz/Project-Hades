using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentMenuScript : MonoBehaviour
{
    [SerializeField] GameObject augmentMenu;
    [SerializeField] Sprite[] stanceSprites;
    [SerializeField] GameObject[] stanceButtons;

    public const string WATER_UNLOCKED = "WATER_UNLOCKED";
    public const string WIND_UNLOCKED = "WIND_UNLOCKED";
    public const string FIRE_UNLOCKED = "FIRE_UNLOCKED";

    protected static bool isAltHeld = false;
    [SerializeReference] bool debug = false;

    public static bool augmentMenuCheck {
        get { return isAltHeld; }
        set { isAltHeld = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        augmentMenu.SetActive(false);
    }

    private void OnEnable()
    {
        EventBroadcaster.Instance.AddObserver(EventNames.Augment.WATER_UNLOCKED, this.SetWaterAugmentSprite);
        EventBroadcaster.Instance.AddObserver(EventNames.Augment.WIND_UNLOCKED, this.SetWindAugmentSprite);
        EventBroadcaster.Instance.AddObserver(EventNames.Augment.FIRE_UNLOCKED, this.SetFireAugmentSprite);
    }

    private void OnDisable()
    {
        EventBroadcaster.Instance.RemoveObserver(EventNames.Augment.WATER_UNLOCKED);
        EventBroadcaster.Instance.RemoveObserver(EventNames.Augment.WIND_UNLOCKED);
        EventBroadcaster.Instance.RemoveObserver(EventNames.Augment.FIRE_UNLOCKED);
    }

    // Update is called once per frame
    void Update()
    {
        if(MenuScript.weaponWheelCheck == false) DoAction();

        if(debug) augmentMenu.SetActive(true);
        else augmentMenu.SetActive(false);
    }

    void DoAction() {
        if (Input.GetKey(KeyCode.LeftAlt)) {
            isAltHeld = true;
            debug = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt)) {
            isAltHeld = false;
            debug = false;
        }
    }

    void SetWaterAugmentSprite(Parameters param)
    {
        bool waterUnlocked = param.GetBoolExtra(WATER_UNLOCKED, false);

        if (waterUnlocked)
        {
            stanceButtons[0].GetComponent<UnityEngine.UI.Image>().sprite = stanceSprites[0];
            stanceButtons[0].name = stanceSprites[0].name;
        }
    }

    void SetWindAugmentSprite(Parameters param)
    {
        bool windUnlocked = param.GetBoolExtra(WIND_UNLOCKED, false);

        if (windUnlocked)
        {
            stanceButtons[1].GetComponent<UnityEngine.UI.Image>().sprite = stanceSprites[1];
            stanceButtons[1].name = stanceSprites[1].name;
        }
    }

    void SetFireAugmentSprite(Parameters param)
    {
        bool fireUnlocked = param.GetBoolExtra(FIRE_UNLOCKED, false);

        if (fireUnlocked)
        {
            stanceButtons[2].GetComponent<UnityEngine.UI.Image>().sprite = stanceSprites[2];
            stanceButtons[2].name = stanceSprites[2].name;
        }
    }
}
