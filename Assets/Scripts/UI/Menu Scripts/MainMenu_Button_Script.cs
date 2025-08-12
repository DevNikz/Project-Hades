using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_Button_Script : MonoBehaviour
{
    [SerializeField] private ASyncLoader asyncLoader;
    [SerializeField] private GameObject _saveMenuInner;
    [SerializeField] private GameObject _saveMenuBacking;
    [SerializeField] private Vector2 _saveMenuYPositions;
    [SerializeField] private bool _isHubAvailable = false;

    [SerializeField] private string sceneToLoad = "Tutorial";
    public void OnStartClick(int saveIndex)
    {
        SaveManager.Instance.LoadPlayer(saveIndex);

        if (asyncLoader != null)
        {
            if (SaveManager.Instance.CurrentStats.hasPlayed == 1 && _isHubAvailable)
                sceneToLoad = "HubLevel";

            if (RevampPlayerController.Instance != null)
                RevampPlayerController.Instance.SetInput(true); //enable player input

            asyncLoader.LoadLevelBtn(sceneToLoad);
            SFXManager.Instance.SwitchAudio();
        }
        else
        {
            Debug.LogError("ASyncLoader is not assigned!");
        }
    }

    public void OnOptionsClick()
    {
        //load options
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }
    public void OnDeleteSave(int saveIndex)
    {
        if (SaveManager.Instance != null)
            SaveManager.Instance.DeleteSave(saveIndex);
    }

    public void ActivateSaveMenu()
    {
        _saveMenuBacking.SetActive(true);
        _saveMenuInner.transform.localPosition = new(_saveMenuInner.transform.localPosition.x, _saveMenuYPositions.x, _saveMenuInner.transform.localPosition.z);
        StartCoroutine(LerpSaveMenu(_saveMenuYPositions.y, 0.4f, true));
    }
    public void DeactivateSaveMenu()
    {
        _saveMenuBacking.SetActive(false);
        StartCoroutine(LerpSaveMenu(_saveMenuYPositions.x, 0.4f, false));

    }
    private IEnumerator LerpSaveMenu(float endY, float duration, bool endActivity)
    {
        Vector3 startPos = _saveMenuInner.transform.localPosition;
        Vector3 endPos = new Vector3(startPos.x, endY, startPos.z);
        float t = 0.0f;
        if(endActivity) _saveMenuInner.SetActive(true);
        while (t < 1.0f)
        {
            t += Time.deltaTime / duration;
            _saveMenuInner.transform.localPosition = Vector3.Lerp(startPos, endPos, t);
            yield return new WaitForEndOfFrame();
        }
        if(!endActivity) _saveMenuInner.SetActive(false);
    }
}
