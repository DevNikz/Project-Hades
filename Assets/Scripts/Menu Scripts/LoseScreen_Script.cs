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

            if (Input.anyKeyDown)
                SceneManager.LoadScene("Title Screen", LoadSceneMode.Single);
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
