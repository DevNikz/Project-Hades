using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreen_Script : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject bg;
    [SerializeField] string _sceneToReloadTo = "Title Screen";
    [SerializeField][Range(0.1f, 5f)] private float delay = 1f;
    [SerializeField] private float clickDelay = 2.0f;
    private float _showcaseTime = 0.0f;
    private bool _isDisplayed = false;

    void Awake()
    {
        panel.SetActive(false);
        bg.SetActive(false);
    }

    void Update()
    {
        if(RevampPlayerStateHandler.Instance != null)
        if (RevampPlayerStateHandler.Instance.gameObject.tag == "Player(Dead)" && !_isDisplayed)
        {
            _isDisplayed = true;   
            bg.SetActive(true);
            panel.SetActive(true);
            _showcaseTime = Time.time + clickDelay;
           
                //Defeat();
        }

        if (Input.anyKeyDown && IsMouseOverGameWindow && Time.time > _showcaseTime && _isDisplayed)
        {
            Debug.Log("Pressed");
            Reset();

            //Disable
            panel.SetActive(false);
            bg.SetActive(false);

            //GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadLevel("HubLevel");
            GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadLevel(_sceneToReloadTo);
        }
    }

    void Reset()
    {
        if (RevampPlayerStateHandler.Instance != null)
        {
            _isDisplayed = false;
            RevampPlayerStateHandler.Instance.gameObject.tag = "Player";
            RevampPlayerStateHandler.Instance.ResetHealth();
            RevampPlayerStateHandler.Instance.ResetCharge();
        }
    }

    bool IsMouseOverGameWindow
    {
        get
        {
            Vector3 mp = Input.mousePosition;
            return !(0 > mp.x || 0 > mp.y || Screen.width < mp.x || Screen.height < mp.y);
        }
    }

    public void Defeat()
    {
        StartCoroutine(ShowDefeatScreenWithDelay());
    }

    private IEnumerator ShowDefeatScreenWithDelay()
    {
        yield return new WaitForSeconds(delay);
        bg.SetActive(true);
        panel.SetActive(true);
    }
}
