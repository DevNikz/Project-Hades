using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SaveUIManager : MonoBehaviour
{
    public List<GameObject> save;
    public List<GameObject> load;

    void Update() {
        if(SaveManager.Instance.save1) {
            save[0].SetActive(false);
            load[0].SetActive(true);
        }
        if(SaveManager.Instance.save2) {
            save[1].SetActive(false);
            load[1].SetActive(true);
        }
        if(SaveManager.Instance.save3) {
            save[2].SetActive(false);
            load[2].SetActive(true);
        }
    }
}
