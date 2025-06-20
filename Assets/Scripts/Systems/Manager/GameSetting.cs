using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSetting : MonoBehaviour
{

    public static GameSetting Instance;
    [SerializeField] public bool highDetail { get; set; }
    [SerializeField] public List<ClutterSpawner> clutterSpawners;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        SetScreenMode(SaveManager.Instance.CurrentSettings.screenMode != 0);
        SetVsync(SaveManager.Instance.CurrentSettings.vsync != 0);
        SetDifficulty(SaveManager.Instance.CurrentSettings.difficulty != 0);
        SetDetailMode(SaveManager.Instance.CurrentSettings.detail != 0);
    }

    // IEnumerator SetDetail(bool value)
    // {
    //     GameObject[] temp = GameObject.FindGameObjectsWithTag("Clutter");


    //     if (temp != null)
    //     {
    //         foreach (GameObject obj in temp)
    //         {
    //             clutterSpawners.Add(obj.GetComponent<ClutterSpawner>());
    //         }

    //         yield return new WaitForSeconds(1f);

    //         foreach (ClutterSpawner cs in clutterSpawners)
    //         {
    //             cs.SetClutterVisiblity(value);
    //         }
    //     }

    //     yield return null;
    // }

    public void SetVsync(bool value)
    {
        QualitySettings.vSyncCount = value ? 1 : 0;
    }

    public void SetScreenMode(bool value)
    {
        switch (value)
        {
            case true:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case false:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
    }

    public void SetDifficulty(bool value)
    {

    }

    public void SetDetailMode(bool value)
    {
        highDetail = value;
        // StartCoroutine(SetDetail(value));
    }
}
