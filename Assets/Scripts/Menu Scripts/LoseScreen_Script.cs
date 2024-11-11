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
        if (PlayerController.Instance.gameObject.tag == "Player(Dead)")
        {
            Defeat();

            if (Input.anyKeyDown && IsMouseOverGameWindow)
                SceneManager.LoadScene("Title Screen", LoadSceneMode.Single);
        }
    }

    bool IsMouseOverGameWindow
    {
        get
        {
            Vector3 mp = Input.mousePosition;
            return !( 0>mp.x || 0>mp.y || Screen.width<mp.x || Screen.height<mp.y );
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
