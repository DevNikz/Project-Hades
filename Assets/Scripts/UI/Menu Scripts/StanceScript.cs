using UnityEngine;

public class StanceHitbox : MonoBehaviour
{
    [Tooltip("The stance index corresponding to this hitbox (0 = Earth, 1 = Water, 2 = Air, 3 = Fire)")]
    [SerializeField] private int stanceIndex;

    [SerializeField] private AugmentMenuScript augmentMenuScript; // Reference to the AugmentMenuScript

    private void OnTriggerEnter(Collider other)
    {
        // Log the tag of the object that entered the collider
        Debug.Log($"Object with tag '{other.tag}' entered the collider.");

        // Check if the object entering the hitbox has the "Player" tag
        if (other.CompareTag("Player"))
        {
            // Unlock the selected stance and lock all others
            switch (stanceIndex)
            {
                case 0: // Earth
                    SelectStance(AugmentType.Earth);
                    break;
                case 1: // Water
                    SelectStance(AugmentType.Water);
                    UpdateAugmentMenu(AugmentMenuScript.WATER_UNLOCKED);
                    break;
                case 2: // Wind
                    SelectStance(AugmentType.Air);
                    UpdateAugmentMenu(AugmentMenuScript.WIND_UNLOCKED);
                    break;
                case 3: // Fire
                    SelectStance(AugmentType.Fire);
                    UpdateAugmentMenu(AugmentMenuScript.FIRE_UNLOCKED);
                    break;
                default:
                    Debug.LogWarning($"Invalid stance index: {stanceIndex}");
                    break;
            }
            Debug.Log("Player Entered");
        }
        Debug.Log("Entered");
    }

    private void SelectStance(AugmentType selectedStance)
    {
        // Check if ItemManager.Instance is null
        if (ItemManager.Instance == null)
        {
            Debug.LogError("ItemManager.Instance is null. Ensure the ItemManager is in the scene and properly initialized.");
            return;
        }

        // Lock all stances using RemoveAugment
        ItemManager.Instance.RemoveAugment(AugmentType.Earth);
        ItemManager.Instance.RemoveAugment(AugmentType.Water);
        ItemManager.Instance.RemoveAugment(AugmentType.Air);
        ItemManager.Instance.RemoveAugment(AugmentType.Fire);

        // Debug logs to confirm locking
        Debug.Log($"Earth locked: {ItemManager.Instance.Earth}");
        Debug.Log($"Water locked: {ItemManager.Instance.Water}");
        Debug.Log($"Wind locked: {ItemManager.Instance.Wind}");
        Debug.Log($"Fire locked: {ItemManager.Instance.Fire}");

        // Unlock the selected stance
        ItemManager.Instance.AddAugment(selectedStance);
        ItemManager.Instance.HubSelectedStance = selectedStance;

        // Optional: Debug log to confirm the stance selection
        Debug.Log($"Stance selected: {selectedStance}");
    }

    private void UpdateAugmentMenu(string stanceUnlocked)
    {
        if (augmentMenuScript == null)
        {
            Debug.LogError("AugmentMenuScript reference is not assigned in StanceHitbox.");
            return;
        }

        // Log resetting all stance booleans using RemoveAugment
        Debug.Log("Resetting all stance booleans to locked using isLocked.");
        augmentMenuScript.earthUnlocked = false;
        augmentMenuScript.waterUnlocked = false;
        augmentMenuScript.windUnlocked = false;
        augmentMenuScript.fireUnlocked = false;

        // Update the locked state for each stance
        switch (stanceUnlocked)
        {
            case AugmentMenuScript.EARTH_UNLOCKED:
                Debug.Log("Unlocking Earth stance.");
                augmentMenuScript.earthUnlocked = true;
                UpdateStanceElements(3, false); // Unlock Earth
                break;
            case AugmentMenuScript.WATER_UNLOCKED:
                Debug.Log("Unlocking Water stance.");
                augmentMenuScript.waterUnlocked = true;
                UpdateStanceElements(0, false); // Unlock Water
                break;
            case AugmentMenuScript.WIND_UNLOCKED:
                Debug.Log("Unlocking Wind stance.");
                augmentMenuScript.windUnlocked = true;
                UpdateStanceElements(1, false); // Unlock Wind
                break;
            case AugmentMenuScript.FIRE_UNLOCKED:
                Debug.Log("Unlocking Fire stance.");
                augmentMenuScript.fireUnlocked = true;
                UpdateStanceElements(2, false); // Unlock Fire
                break;
            default:
                Debug.LogWarning($"Unknown stance unlocked: {stanceUnlocked}");
                break;
        }

        // Lock all other stances
        LockOtherStances(stanceUnlocked);

        // Log completion of UpdateAugmentMenu
        Debug.Log("UpdateAugmentMenu completed.");
    }

    private void LockOtherStances(string unlockedStance)
    {

        if (unlockedStance != AugmentMenuScript.WATER_UNLOCKED)
        {
            UpdateStanceElements(0, true); // Lock Water
        }
        if (unlockedStance != AugmentMenuScript.WIND_UNLOCKED)
        {
            UpdateStanceElements(1, true); // Lock Wind
        }
        if (unlockedStance != AugmentMenuScript.FIRE_UNLOCKED)
        {
            UpdateStanceElements(2, true); // Lock Fire
        }
        if (unlockedStance != AugmentMenuScript.EARTH_UNLOCKED)
        {
            UpdateStanceElements(3, true); // Lock Earth
        }
    }

    private void UpdateStanceElements(int index, bool isLocked)
    {
        if (index < 0 || index >= augmentMenuScript.stanceButtons.Length)
        {
            Debug.LogError($"Invalid stance button index: {index}");
            return;
        }

        var buttonImage = augmentMenuScript.stanceButtons[index].GetComponent<UnityEngine.UI.Image>();
        if (buttonImage == null)
        {
            Debug.LogError($"No Image component found on stance button at index {index}");
            return;
        }

        if (isLocked)
        {
            // Update the button to show it is locked
            buttonImage.sprite = augmentMenuScript.lockedSprite;
            augmentMenuScript.stanceButtons[index].name = "Locked";
        }
        else
        {
            // Update the button to show it is unlocked
            buttonImage.sprite = augmentMenuScript.stanceSprites[index];
            augmentMenuScript.stanceButtons[index].name = augmentMenuScript.stanceSprites[index].name;
        }
    }






}





