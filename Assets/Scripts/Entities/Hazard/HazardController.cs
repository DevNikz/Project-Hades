using Sirenix.OdinInspector;
using UnityEngine;

public class HazardController : MonoBehaviour
{
    [TitleGroup("Properties", "General Hazard Properties", TitleAlignments.Centered)]

    [BoxGroup("Properties/box1", ShowLabel = false)]
    public HazardType hazardType;

    [BoxGroup("Properties/box1", ShowLabel = false)]
    public float entitySpeed;

    [PropertySpace, TitleGroup("Timer", "General Timer Properties", TitleAlignments.Centered)]
    [BoxGroup("Timer/Settings", ShowLabel = false)]
    [ReadOnly, SerializeReference] protected TimerState timerState = TimerState.None;
    [BoxGroup("Timer/Settings", ShowLabel = false)]
    [Range(0.1f, 10f)] public float curTimer = 1f;
    [BoxGroup("Timer/Settings", ShowLabel = false)]
    [ReadOnly] public float tempTimer = 0f;

    [PropertySpace, TitleGroup("Hazards", "Specific Hazard Properties", TitleAlignments.Centered)]
    public bool EnableBog;
    [ShowIfGroup("EnableBog")]
    [BoxGroup("EnableBog/Bog", ShowLabel = false)]
    [Range(0.0f, 2.0f)] public float debuffBogScalar = 1.35f;

    public bool EnablePillar;
    [ShowIfGroup("EnablePillar")]
    [BoxGroup("EnablePillar/Pillar", ShowLabel = false)]
    [ReadOnly, SerializeReference] protected int counter = 1;

    [BoxGroup("EnablePillar/Pillar", ShowLabel = false)]
    [ReadOnly, SerializeReference] protected GameObject hitbox;

    [BoxGroup("EnablePillar/Pillar", ShowLabel = false)]
    [ReadOnly, SerializeReference] protected GameObject visualBox;

    [BoxGroup("EnablePillar/Pillar", ShowLabel = false)]
    [ReadOnly, SerializeReference] protected ParticleSystem hitEffect;

    [BoxGroup("EnablePillar/Pillar", ShowLabel = false)]
    [ReadOnly, SerializeReference] protected ParticleSystem shakeEffect;

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
            hitbox = transform.Find("HitCollider").gameObject;
            visualBox = transform.Find("VisualCollider").gameObject;
            hitEffect = transform.Find("HitEffect").GetComponent<ParticleSystem>();
            shakeEffect = transform.Find("ShakeEffect").GetComponent<ParticleSystem>();
            pillar1 = transform.Find("1").gameObject;
            pillar2 = transform.Find("2").gameObject;
            pillar3 = transform.Find("3").gameObject;
            pillar4 = transform.Find("4").gameObject;
            pillar5 = transform.Find("5").gameObject;
        }
    }

    void Update() {
        SwitchSpritePillar();
        UpdateTimer();
    }

    void UpdateTimer() {
        if(timerState == TimerState.Start) {
            tempTimer -= Time.deltaTime;
            if(tempTimer <= 0.0f) {
                hitbox.SetActive(true);
                timerState = TimerState.Stop;
            }
        }

        else if(timerState == TimerState.Stop) {
            visualBox.SetActive(false);
            hitbox.SetActive(false);
            pillar1.SetActive(false);
            pillar2.SetActive(false);
            pillar3.SetActive(false);
            pillar4.SetActive(false);
            pillar5.SetActive(true);

            shakeEffect.Stop();
            tempTimer = 0f;
            timerState = TimerState.None;
        }
        
    }

    public void InitHazard(GameObject gameObject = null) {
        // if(gameObject != null) entitySpeed = gameObject.GetComponent<Movement>().strafeSpeed;
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

    void InitBog(GameObject other)
    {
        other.transform.Find("Anims").GetComponent<Animator>().speed = 0.75f;
        // other.GetComponent<Movement>().SetSpeed(entitySpeed * debuffBogScalar);
        RevampPlayerController player = other.GetComponent<RevampPlayerController>();
        player.CurrentSpeed = player.PlayerStats.MoveSpeed * debuffBogScalar;
    }

    void InitPillar() {
        //Play Effect
        hitEffect.Play();

        //Add Counter
        counter += 1;
        if (counter == 11) {
            Debug.Log("Shaking!");
            shakeEffect.Play();
            visualBox.SetActive(true);
            tempTimer = curTimer;
            timerState = TimerState.Start;
        }
        else if(counter > 11) counter = 11;
    }

    void SwitchSpritePillar() {
        switch(counter) {
            case 3:
                pillar1.SetActive(true);
                pillar2.SetActive(false);
                pillar3.SetActive(false);
                pillar4.SetActive(false);
                pillar5.SetActive(false);
                break;
            case 5:
                pillar1.SetActive(false);
                pillar2.SetActive(true);
                pillar3.SetActive(false);
                pillar4.SetActive(false);
                pillar5.SetActive(false);
                break;
            case 7:
                pillar1.SetActive(false);
                pillar2.SetActive(false);
                pillar3.SetActive(true);
                pillar4.SetActive(false);
                pillar5.SetActive(false);
                break;
            case 9:
                pillar1.SetActive(false);
                pillar2.SetActive(false);
                pillar3.SetActive(false);
                pillar4.SetActive(true);
                pillar5.SetActive(false);
                break;
            // case 11:
            //     shakeEffect.Play();
            //     tempTimer = curTimer;
            //     timerState = TimerState.Start;
            //     break;
        }
    }
}