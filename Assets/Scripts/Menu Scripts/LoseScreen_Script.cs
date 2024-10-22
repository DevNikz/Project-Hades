using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreen_Script : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject bg;
    [SerializeField, Range(0f, 10f)] private float defeatScreenDelay;

    void Awake()
    {
        panel.SetActive(false);
        bg.SetActive(false);
    }

    void Update()
    {
        if (PlayerController.Instance.entityState == EntityState.Dead)
        {
            Defeat();

            if (Input.anyKeyDown)
                SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }

    public void Defeat()
    {
        StartCoroutine(ShowDefeatScreenWithDelay());
    }

    private IEnumerator ShowDefeatScreenWithDelay()
    {
        yield return new WaitForSeconds(defeatScreenDelay); 
        bg.SetActive(true);
        panel.SetActive(true);
    }
}
