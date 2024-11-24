using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ASyncLoader : MonoBehaviour
{
    [Header("Menu Screens")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject mainMenu;

    [Header("Slider")]
    [SerializeField] private Slider loadingSlider;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI loadingText;

    private bool isLoading = false;

    public void LoadLevelBtn(string levelToLoad)
    {
        loadingScreen.SetActive(true);
        StartCoroutine(LoadLevelAsync(levelToLoad));
    }

    IEnumerator LoadLevelAsync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

        isLoading = true;
        string baseText = "Loading";
        int dotCount = 0;

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingSlider.value = progressValue;
            yield return null;

            while (isLoading)
            {

                dotCount = (dotCount + 1) % 4;


                loadingText.text = baseText + new string('.', dotCount);

                yield return new WaitForSeconds(0.5f);
            }


            loadingText.text = baseText + "...";
        }

        isLoading = false;


    }
}

