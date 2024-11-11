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

    public void LoadLevel(string levelToLoad) {
        Debug.Log("Loading Level...");

        //Disable any ui

        //Init
        loadingScreen.SetActive(true);

        if(PlayerController.Instance != null) {
            Destroy(PlayerController.Instance.gameObject);
        }

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        Resources.UnloadUnusedAssets();
        SceneManager.LoadScene(levelToLoad, LoadSceneMode.Single);

        //StartCoroutine(LoadLevelAsync(levelToLoad));
    }

    IEnumerator LoadLevelAsync(string levelToLoad)
    {
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
        isLoading = false;
    }
}
