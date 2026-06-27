using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ItemShopHandler : MonoBehaviour
{
    [TitleGroup("Properties", "General Store Item Properties", Alignment = TitleAlignments.Centered)]
    [BoxGroup("Properties/Box", ShowLabel = false)]
    public AugmentType type;

    [BoxGroup("Properties/Box", ShowLabel = false)]
    [Range(0.1f, 100000f)] public float cost = 1f;

    public void AddItem() {
        ShopHandler.Instance.AddItem(gameObject);
    }

    public AugmentType GetAugmentType() {
        return type;
    }
}
