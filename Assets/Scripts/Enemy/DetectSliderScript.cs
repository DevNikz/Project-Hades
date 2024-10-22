using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class DetectSliderScript : MonoBehaviour
{
    private GameObject obj;

    Slider slider;

    bool playerSeen, meterFull = false;

    float fillTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        obj = this.transform.Find("DetectionSlider").gameObject;
        slider = obj.GetComponent<Slider>();

        slider.minValue = 0f;
        slider.maxValue = 3f;

        obj.SetActive(false);
    }

    private void OnEnable()
    {
        EventBroadcaster.Instance.AddObserver(EventNames.Combat.PLAYER_SEEN, SetPlayerSeen);
    }

    private void OnDisable()
    {
        EventBroadcaster.Instance.RemoveObserver(EventNames.Combat.PLAYER_SEEN);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerSeen)
        {
            FillSlider();

            if(meterFull) obj.SetActive(false);
        }
            
        else ResetSlider();
    }

    void FillSlider()
    {
        slider.value = Mathf.Lerp(slider.minValue, slider.maxValue, fillTime);

        fillTime += 0.375f * Time.deltaTime;

        if(fillTime >= 3.0f) meterFull = true;

    }

    void ResetSlider()
    {
        slider.value = slider.minValue;

        fillTime = 0f;
        meterFull = false;
    }

    void SetPlayerSeen(Parameters param)
    {
        playerSeen = param.GetBoolExtra("HIDDEN", false);

        if(playerSeen) obj.SetActive(true);
    }
}
