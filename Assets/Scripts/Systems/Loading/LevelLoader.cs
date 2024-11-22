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
    private bool isLoading = false;

    void OnEnable(){
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    void OnSceneLoad(Scene scene, LoadSceneMode mode){
        this.loadingScreen.SetActive(false);
        isLoading = false;
    }

    public void LoadLevel(string levelToLoad) {
        Debug.Log("Loading Level...");

        //Init
        loadingScreen.SetActive(true);
        StartCoroutine(LoadLevelAsync(levelToLoad));
    }

    IEnumerator LoadLevelAsync(string levelToLoad)
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        Resources.UnloadUnusedAssets();

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad, LoadSceneMode.Single);
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
    }
}
