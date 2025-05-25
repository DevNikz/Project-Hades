using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreen_Script : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject bg;
    [SerializeField][Range(0.1f, 5f)] private float delay = 1f;

    void Awake()
    {
        panel.SetActive(false);
        bg.SetActive(false);
    }

    void Update()
    {
        if(PlayerController.Instance != null)
        if (PlayerController.Instance.gameObject.tag == "Player(Dead)")
        {
            bg.SetActive(true);
            panel.SetActive(true);
            //Defeat();

            if (Input.anyKeyDown && IsMouseOverGameWindow)
            {
                Debug.Log("Pressed");
                Reset();

                //Disable
                panel.SetActive(false);
                bg.SetActive(false);

                GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadLevel("HubLevel");

            }
        }

        if(RevampPlayerStateHandler.Instance != null)
        if (RevampPlayerStateHandler.Instance.gameObject.tag == "Player(Dead)")
        {
            bg.SetActive(true);
            panel.SetActive(true);
            //Defeat();

            if (Input.anyKeyDown && IsMouseOverGameWindow)
            {
                Debug.Log("Pressed");
                Reset();

                //Disable
                panel.SetActive(false);
                bg.SetActive(false);

                GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadLevel("HubLevel");

            }
        }
    }

    void Reset()
    {
        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.gameObject.tag = "Player";
            PlayerController.Instance.RevertHealth();
            PlayerController.Instance.RevertMana();
            PlayerController.Instance.isDead = false; 
        }

        if (RevampPlayerStateHandler.Instance != null)
        {
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
