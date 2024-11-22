using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    public void PlaySFX(string sfx) {
        SFXManager.Instance.Play(sfx);
    }
}
