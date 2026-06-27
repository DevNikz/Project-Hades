using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SaveUIManager : MonoBehaviour
{
    public List<GameObject> save;
    public List<GameObject> load;

    void Update() {
        if(SaveManager.Instance) {
            save[0].SetActive(!SaveManager.Instance.HadPlayedSave1);
            load[0].SetActive(SaveManager.Instance.HadPlayedSave1);
            save[1].SetActive(!SaveManager.Instance.HadPlayedSave2);
            load[1].SetActive(SaveManager.Instance.HadPlayedSave2);
            save[2].SetActive(!SaveManager.Instance.HadPlayedSave3);
            load[2].SetActive(SaveManager.Instance.HadPlayedSave3);
        }
    }
}
