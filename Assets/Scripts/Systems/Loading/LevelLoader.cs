using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private TextMeshProUGUI loadingText;
    [Header("Properties")]
    [SerializeField] private string baseLoadingText = "Loading";

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (loadingScreen != null)
            this.loadingScreen.SetActive(false);
    }

    public void LoadLevel(string levelToLoad)
    {
        Debug.Log($"[INFO]: Loading Level {levelToLoad}...");

        //Init
        loadingScreen.SetActive(true);
        StartCoroutine(LoadLevelAsync(levelToLoad));
    }

    private IEnumerator LoadLevelAsync(string levelToLoad)
    {
        // SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad, LoadSceneMode.Single);
        loadOperation.allowSceneActivation = false;
        int dotCount = 0;
        while (!loadOperation.isDone)
        {
            Debug.Log($"[INFO]: Still loading level {levelToLoad}...");
            loadingSlider.value = loadOperation.progress;
            dotCount = (dotCount + 1) % 3;
            RefreshLoadingText(dotCount + 1);
            yield return new WaitForSeconds(0.2f);

            if (loadOperation.progress >= 0.9f)
            {
                Resources.UnloadUnusedAssets();
                loadOperation.allowSceneActivation = true;
            }
        }
    }

    private void RefreshLoadingText(int variant)
    {
        loadingText.text = baseLoadingText;
        for(int i = 0; i <= variant; i++)
            loadingText.text += ".";
    }
}
