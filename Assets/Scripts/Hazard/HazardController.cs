using Sirenix.OdinInspector;
using UnityEngine;

public class HazardController : MonoBehaviour
{
    [PropertySpace][TitleGroup("Properties", "General Hazard Properties", TitleAlignments.Centered)]

    [BoxGroup("Properties/box1", ShowLabel = false)]
    public HazardType hazardType;

    [BoxGroup("Properties/box1", ShowLabel = false)]
    public float entitySpeed;

    [PropertySpace][TitleGroup("Bog", "General Bog Properties", TitleAlignments.Centered)]

    [BoxGroup("Bog/box1", ShowLabel = false)]
    [Range(0.1f, 10f)] public float debuffBogScalar = 1.35f;

    public void InitHazard(GameObject gameObject) {
        entitySpeed = gameObject.GetComponent<Movement>().strafeSpeed;
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

    }
}