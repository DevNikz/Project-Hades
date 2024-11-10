using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelRewardScript : MonoBehaviour
{
    [SerializeField] GameObject levelRewardMenu;
    [SerializeField] Sprite[] augmentSprites;
    [SerializeField] Sprite[] stanceSprites;
    [SerializeField] GameObject[] augmentButtons;
    [SerializeField] GameObject stanceButton;

    string choice = null;
    bool choiceMade = false;

    public string nextLevel = null;

    void Start()
    {
        AssignSprites();
        levelRewardMenu.SetActive(false);
    }

    private void Update()
    {
        if(levelRewardMenu.activeInHierarchy)
        {
            //Time.timeScale = 0.0f;

            if (choiceMade && nextLevel != null)
            {
                TransitionLevel();
            }
        }
    }

    void AssignSprites()
    {
        foreach (var button in augmentButtons)
        {
            int n = Random.Range(0, augmentSprites.Length - 1);
            button.GetComponent<UnityEngine.UI.Image>().sprite = augmentSprites[n];

            button.name = augmentSprites[n].name;
        }

        if(!ItemManager.Instance.Water)
        {
            stanceButton.GetComponent<UnityEngine.UI.Image>().sprite = stanceSprites[0];
            stanceButton.name = stanceSprites[0].name;  
        }
        else if(!ItemManager.Instance.Wind)
        {
            stanceButton.GetComponent<UnityEngine.UI.Image>().sprite = stanceSprites[1];
            stanceButton.name = stanceSprites[1].name;
        }
        else if (!ItemManager.Instance.Fire)
        {
            stanceButton.GetComponent<UnityEngine.UI.Image>().sprite = stanceSprites[2];
            stanceButton.name = stanceSprites[2].name;
        }
    }

    public void ButtonSelected()
    {
        GameObject button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        choice = button.name;

        Debug.Log(choice);
    }

    public void ChoiceMade()
    {
        if (choice != null)
        {
            switch (choice)
            {
                case "Aggro":
                    ItemManager.Instance.PAddAggro(1);
                    break;
                case "Vitality":
                    ItemManager.Instance.PAddVitality(1);
                    break;
                case "Heavy":
                    ItemManager.Instance.PAddHeavy(1);
                    break;
                case "Steel":
                    ItemManager.Instance.PAddSteel(1);
                    break;
                case "Thalassa":
                    ItemManager.Instance.Water = true;
                    Broadcaster.Instance.AddBoolParam(AugmentMenuScript.WATER_UNLOCKED, EventNames.Augment.WATER_UNLOCKED, true);
                    break;
                case "Ouranos":
                    ItemManager.Instance.Wind = true;
                    Broadcaster.Instance.AddBoolParam(AugmentMenuScript.WIND_UNLOCKED, EventNames.Augment.WIND_UNLOCKED, true);
                    break;
                case "Gehenna":
                    ItemManager.Instance.Fire = true;
                    Broadcaster.Instance.AddBoolParam(AugmentMenuScript.FIRE_UNLOCKED, EventNames.Augment.FIRE_UNLOCKED, true);
                    break;

            }
            choiceMade = true;
        }
        else
            Debug.Log("bad");
    }

    public void TransitionLevel()
    {
        //Time.timeScale = 1f;
        SceneManager.LoadScene(nextLevel, LoadSceneMode.Single);
        levelRewardMenu.SetActive(false);
        //StartCoroutine(Deactivate());
    }

    IEnumerator Deactivate()
    {
        levelRewardMenu.SetActive(false);
        yield return null;
    }

    public void Activate()
    {
        levelRewardMenu.SetActive(true);
    }
}
