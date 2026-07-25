using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class EnemySetup : MonoBehaviour
{
    private GameObject healthUI;
    private GameObject poiseUI;
    private GameObject baseEnemy;
    public float height = 1.7f;

    [PropertySpace, TitleGroup("Debug", "", TitleAlignments.Centered)] 
    [SerializeField] private bool isDebug;

    [PropertySpace, TitleGroup("Reference", "", TitleAlignments.Centered)] 
    [SerializeReference] private GameObject sightDebug; 
    private ObjectPool bulletPool;

    void Start() {
        bulletPool = GetComponent<ObjectPool>();
        //UI
        healthUI = transform.Find("HealthAndDetection").gameObject;
        // poiseUI = transform.Find("Poise").gameObject;
        baseEnemy = transform.Find("Base").gameObject;

        sightDebug = baseEnemy.transform.Find("Cone").gameObject;
    }

    void Update()
    {
        UpdateUI();
        ToggleSightDebug();
    }

    void UpdateUI() {
        healthUI.GetComponent<RectTransform>().position = new Vector3(baseEnemy.transform.position.x, height, baseEnemy.transform.position.z);
        // poiseUI.GetComponent<RectTransform>().position = new Vector3(baseEnemy.transform.position.x, 1.9f, baseEnemy.transform.position.z);

        healthUI.GetComponent<RectTransform>().localRotation = Quaternion.Euler(45f,45f,0f);
        // poiseUI.GetComponent<RectTransform>().localRotation = Quaternion.Euler(45f,45f,0f);
    }

    void ToggleSightDebug() {
        sightDebug.GetComponent<MeshRenderer>().enabled = isDebug;
    }

}
