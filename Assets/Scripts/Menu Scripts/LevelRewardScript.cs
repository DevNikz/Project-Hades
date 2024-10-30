using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelRewardScript : MonoBehaviour
{
    [SerializeField] GameObject levelRewardMenu;
    [SerializeField] Sprite[] sprites;
    [SerializeField] GameObject[] buttons;

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
        foreach (var button in buttons)
        {
            int n = Random.Range(0, sprites.Length - 1);
            button.GetComponent<UnityEngine.UI.Image>().sprite = sprites[n];

            button.name = sprites[n].name;
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
