using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundDisable : MonoBehaviour
{
    public Image img;
    public float duration = 2f;

    private void Awake()
    {
        if (RevampPlayerController.Instance != null)
            RevampPlayerController.Instance.SetInput(false); //disable player input

        StartCoroutine("fadeOut");
    }

    IEnumerator fadeOut()
    {
        float counter = 0;
        //Get current color
        Color spriteColor = img.color;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            //Fade from 1 to 0
            float alpha = Mathf.Lerp(1, 0, counter / duration);
            Debug.Log(alpha);

            //Change alpha only
            img.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            //Wait for a frame
            yield return null;
        }
    }
}
