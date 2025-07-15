using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CheatMenu : MonoBehaviour
{
    public void UnlockAllStances()
    {
        if (ItemManager.Instance != null)
        {
            ItemManager.Instance.AddAugment(AugmentType.Earth);
            ItemManager.Instance.AddAugment(AugmentType.Water);
            ItemManager.Instance.AddAugment(AugmentType.Air);
            ItemManager.Instance.AddAugment(AugmentType.Fire);
        }
    }

    public void GodMode()
    {
        if (RevampPlayerStateHandler.Instance != null)
        {
            RevampPlayerStateHandler.Instance.IsGod = !RevampPlayerStateHandler.Instance.IsGod;
        }
    }

    [Button("Tutorial")]
    public void Tutorial()
    {
        Time.timeScale = 1.0f;
        LevelLoader levelLoader;
        if (CheckLoader(out levelLoader))
        {
            SaveManager.Instance.SetDepth(0);
            levelLoader.LoadLevel("Tutorial");
        }
    }

    public void Level1()
    {
        LevelLoader levelLoader;
        if (CheckLoader(out levelLoader))
        {
            SaveManager.Instance.SetDepth(1);
            levelLoader.LoadLevel("Level 1");
        }
    }

    public void Level2()
    {
        LevelLoader levelLoader;
        if (CheckLoader(out levelLoader))
        {
            SaveManager.Instance.SetDepth(2);
            levelLoader.LoadLevel("Level 2");
        }
    }

    public void Level3()
    {
        LevelLoader levelLoader;
        if (CheckLoader(out levelLoader))
        {
            SaveManager.Instance.SetDepth(3);
            levelLoader.LoadLevel("Level 3");
        }
    }

    public void BossLevel()
    {
        LevelLoader levelLoader;
        if (CheckLoader(out levelLoader))
        {
            SaveManager.Instance.SetDepth(4);
            levelLoader.LoadLevel("CronosLevel");
        }
    }

    public void HUBLevel()
    {
        LevelLoader levelLoader;
        if (CheckLoader(out levelLoader))
        {
            levelLoader.LoadLevel("HubLevel");
        }
    }

    public void MainMenu()
    {
        LevelLoader levelLoader;
        if (CheckLoader(out levelLoader))
        {
            levelLoader.LoadLevel("Title Screen");
        }
    }

    bool CheckLoader(out LevelLoader levelLoader)
    {
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        if (levelLoader == null)
        {
            Debug.LogWarning("[WARN]: LevelLoader not found");
            return false;
        }
        else return true;
    }
}
