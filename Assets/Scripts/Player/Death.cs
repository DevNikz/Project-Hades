using Sirenix.OdinInspector;
using UnityEngine;

public class Death : MonoBehaviour
{
    [Space] [Title("Properties")]
    [ReadOnly] [SerializeReference] public int deaths; 

    [Title("[Debug]")]

    [PropertySpace] [Button(ButtonSizes.Gigantic, Name = "Kill Yourself", Icon = SdfIconType.ExclamationCircle), GUIColor("#990c05")] 
    void KillYourself() {
        //Tick Counter
        deaths += 1;

        //Play Animation / Game over screen

        //Leave a corpse (maybe)

        //Respawn Player to same spot
    }


    [PropertySpace] [Button(ButtonSizes.Gigantic, Name = "Forgive Yourself (Reset)")]
    void ResetCounter() {
        deaths = 0;
    }
}
