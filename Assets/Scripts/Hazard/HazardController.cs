using System.Diagnostics;
using Sirenix.OdinInspector;
using UnityEngine;

public class HazardController : MonoBehaviour
{
    [TitleGroup("Properties", "General Hazard Properties", TitleAlignments.Centered)]

    [BoxGroup("Properties/box1", ShowLabel = false)]
    public HazardType hazardType;

    [BoxGroup("Properties/box1", ShowLabel = false)]
    public float entitySpeed;

    [PropertySpace, TitleGroup("Hazards", "Specific Hazard Properties", TitleAlignments.Centered)]

    public bool EnableBog;
    [ShowIfGroup("EnableBog")]
    [BoxGroup("EnableBog/Bog", ShowLabel = false)]
    [Range(0.1f, 10f)] public float debuffBogScalar = 1.35f;

    public bool EnablePillar;
    [ShowIfGroup("EnablePillar")]
    [BoxGroup("EnablePillar/Pillar", ShowLabel = false)]
    [ReadOnly, SerializeReference] protected int counter = 1;

    [BoxGroup("EnablePillar/Pillar", ShowLabel = false)]
    [ReadOnly, SerializeReference] protected GameObject pillar1;

    [BoxGroup("EnablePillar/Pillar", ShowLabel = false)]
    [ReadOnly, SerializeReference] protected GameObject pillar2;

    [BoxGroup("EnablePillar/Pillar", ShowLabel = false)]
    [ReadOnly, SerializeReference] protected GameObject pillar3;

    [BoxGroup("EnablePillar/Pillar", ShowLabel = false)]
    [ReadOnly, SerializeReference] protected GameObject pillar4;

    [BoxGroup("EnablePillar/Pillar", ShowLabel = false)]
    [ReadOnly, SerializeReference] protected GameObject pillar5;


    void Awake() {
        if(hazardType == HazardType.Pillar) {
            pillar1 = transform.Find("1").gameObject;
            pillar2 = transform.Find("2").gameObject;
            pillar3 = transform.Find("3").gameObject;
            pillar4 = transform.Find("4").gameObject;
            pillar5 = transform.Find("5").gameObject;
        }
    }

    public void InitHazard(GameObject gameObject = null) {
        if(gameObject != null) entitySpeed = gameObject.GetComponent<Movement>().strafeSpeed;
        switch(hazardType) {
            case HazardType.Bog:
                InitBog(gameObject);
                break;
            case HazardType.Pillar:
                InitPillar();
                break;
        }
    }

    public void OnTriggerExit() {
        switch(hazardType) {
            case HazardType.Bog:
                entitySpeed = 0;
                break;
        } 
    }

    void InitBog(GameObject gameObject) {
       gameObject.GetComponent<Movement>().SetSpeed(entitySpeed / debuffBogScalar);
    }

    void InitPillar() {
        counter += 1;
        if(counter > 5) counter = 5;
        SwitchSpritePillar();
    }

    void SwitchSpritePillar() {
        switch(counter) {
            case 1:
                pillar1.SetActive(true);
                pillar2.SetActive(false);
                pillar3.SetActive(false);
                pillar4.SetActive(false);
                pillar5.SetActive(false);
                break;
            case 2:
                pillar1.SetActive(false);
                pillar2.SetActive(true);
                pillar3.SetActive(false);
                pillar4.SetActive(false);
                pillar5.SetActive(false);
                break;
            case 3:
                pillar1.SetActive(false);
                pillar2.SetActive(false);
                pillar3.SetActive(true);
                pillar4.SetActive(false);
                pillar5.SetActive(false);
                break;
            case 4:
                pillar1.SetActive(false);
                pillar2.SetActive(false);
                pillar3.SetActive(false);
                pillar4.SetActive(true);
                pillar5.SetActive(false);
                break;
            case 5:
                pillar1.SetActive(false);
                pillar2.SetActive(false);
                pillar3.SetActive(false);
                pillar4.SetActive(false);
                pillar5.SetActive(true);
                break;
        }
    }
}