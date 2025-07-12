using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AugmentMenuScript : MonoBehaviour
{
    [SerializeField] GameObject augmentMenu;
    [SerializeField] public Sprite[] stanceSprites;
    [SerializeField] public Sprite lockedSprite;
    [SerializeField] public GameObject[] stanceButtons;

    [SerializeField] private TextMeshProUGUI vitalityCount;
    [SerializeField] private TextMeshProUGUI aggroCount;
    [SerializeField] private TextMeshProUGUI steelCount;
    [SerializeField] private TextMeshProUGUI heavyCount;

    protected static bool isKeyHeld = false;
    [SerializeReference] bool debug = false;

    public const string WATER_UNLOCKED = "WATER_UNLOCKED";
    public const string WIND_UNLOCKED = "WIND_UNLOCKED";
    public const string FIRE_UNLOCKED = "FIRE_UNLOCKED";
    public const string EARTH_UNLOCKED = "EARTH_UNLOCKED";

    public bool waterUnlocked = false;
    public bool windUnlocked = false;
    public bool fireUnlocked = false;
    public bool earthUnlocked = false;

    /*public static bool augmentMenuCheck {
        get { return isKeyHeld; }
        set { isKeyHeld = value; }
    }*/

    // Start is called before the first frame update
    void Start()
    {
        augmentMenu.SetActive(false);

        vitalityCount.text = ItemManager.Instance.getAugmentCount(AugmentType.Vitality).ToString();
        aggroCount.text = ItemManager.Instance.getAugmentCount(AugmentType.Aggro).ToString();
        steelCount.text = ItemManager.Instance.getAugmentCount(AugmentType.Steel).ToString();
        heavyCount.text = ItemManager.Instance.getAugmentCount(AugmentType.Heavy).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        DoAction();

        if (debug)
        {
            augmentMenu.SetActive(true);

            vitalityCount.text = ItemManager.Instance.getAugmentCount(AugmentType.Vitality).ToString();
            aggroCount.text = ItemManager.Instance.getAugmentCount(AugmentType.Aggro).ToString();
            steelCount.text = ItemManager.Instance.getAugmentCount(AugmentType.Steel).ToString();
            heavyCount.text = ItemManager.Instance.getAugmentCount(AugmentType.Heavy).ToString();
        }
        else augmentMenu.SetActive(false);
    }

    void DoAction() {
        if (Input.GetKey(KeyCode.T)) {
            isKeyHeld = true;
            debug = true;
        }
        else if (Input.GetKeyUp(KeyCode.T)) {
            isKeyHeld = false;
            debug = false;
        }
    }

    /*
     private void OnEnable()
    {
        EventBroadcaster.Instance.AddObserver(EventNames.Augment.WATER_UNLOCKED, this.SetWaterAugmentSprite);
        EventBroadcaster.Instance.AddObserver(EventNames.Augment.WIND_UNLOCKED, this.SetWindAugmentSprite);
        EventBroadcaster.Instance.AddObserver(EventNames.Augment.FIRE_UNLOCKED, this.SetFireAugmentSprite);
        EventBroadcaster.Instance.AddObserver(EventNames.Augment.EARTH_UNLOCKED, this.SetEarthAugmentSprite);
    }

    private void OnDisable()
    {
        EventBroadcaster.Instance.RemoveObserver(EventNames.Augment.EARTH_UNLOCKED);
        EventBroadcaster.Instance.RemoveObserver(EventNames.Augment.WATER_UNLOCKED);
        EventBroadcaster.Instance.RemoveObserver(EventNames.Augment.WIND_UNLOCKED);
        EventBroadcaster.Instance.RemoveObserver(EventNames.Augment.FIRE_UNLOCKED);
    }

    public void SetWaterAugmentSprite(Parameters param)
    {
        bool waterUnlocked = param.GetBoolExtra(WATER_UNLOCKED, false);

        if (waterUnlocked)
        {
            stanceButtons[0].GetComponent<UnityEngine.UI.Image>().sprite = stanceSprites[0];
            stanceButtons[0].name = stanceSprites[0].name;
            Debug.Log("water unlocked");
        }
        else
        {
            stanceButtons[0].GetComponent<UnityEngine.UI.Image>().sprite = lockedSprite;
            stanceButtons[0].name = "Locked";
        }
    }

    public void SetWindAugmentSprite(Parameters param)
    {
        bool windUnlocked = param.GetBoolExtra(WIND_UNLOCKED, false);

        if (windUnlocked)
        {
            stanceButtons[1].GetComponent<UnityEngine.UI.Image>().sprite = stanceSprites[1];
            stanceButtons[1].name = stanceSprites[1].name;
        }
        else
        {
            stanceButtons[1].GetComponent<UnityEngine.UI.Image>().sprite = lockedSprite;
            stanceButtons[1].name = "Locked";
        }
    }

    public void SetFireAugmentSprite(Parameters param)
    {
        bool fireUnlocked = param.GetBoolExtra(FIRE_UNLOCKED, false);

        if (fireUnlocked)
        {
            stanceButtons[2].GetComponent<UnityEngine.UI.Image>().sprite = stanceSprites[2];
            stanceButtons[2].name = stanceSprites[2].name;
        }
        else
        {
            stanceButtons[2].GetComponent<UnityEngine.UI.Image>().sprite = lockedSprite;
            stanceButtons[2].name = "Locked";
        }
    }

    public void SetEarthAugmentSprite(Parameters param)
    {
        bool earthUnlocked = param.GetBoolExtra(EARTH_UNLOCKED, false);
        if (earthUnlocked)
        {
            stanceButtons[3].GetComponent<UnityEngine.UI.Image>().sprite = stanceSprites[3];
            stanceButtons[3].name = stanceSprites[3].name;
        }
        else
        {
            stanceButtons[3].GetComponent<UnityEngine.UI.Image>().sprite = lockedSprite;
            stanceButtons[3].name = "Locked";
        }
    }
    */
}
