using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopHandler : MonoBehaviour
{
    public static ShopHandler Instance;

    [TitleGroup("References", "General Shop References", Alignment = TitleAlignments.Centered)]
    public bool ShowReferences;
    [ShowIfGroup("ShowReferences")]
    [BoxGroup("ShowReferences/Ref", ShowLabel = false)]
    [SerializeReference] private GameObject itemRef;

    private AugmentType itemType;

    [BoxGroup("ShowReferences/Ref", ShowLabel = false)]
    [SerializeReference] private Button buyBtn;

    [BoxGroup("ShowReferences/Ref", ShowLabel = false)]
    [SerializeReference] private Button sellBtn;

    void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void AddItem(GameObject item) {
        itemRef = item;
        itemType = item.GetComponent<ItemShopHandler>().GetAugmentType();
    }

    public void EnableBuyButton() {
        buyBtn.gameObject.SetActive(true);
    }

    public void BuyItem() {
        switch(itemType) {
            case AugmentType.AGGRO:
                ItemManager.Instance.PAddAggro(1);
                break;
            case AugmentType.STEEL:
                ItemManager.Instance.PAddSteel(1);
                break;
        }
    }

    public void ExitMenu() {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    
}
