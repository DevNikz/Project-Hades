using UnityEngine;
using TMPro;
using System.Collections;

public class PopupTrigger : MonoBehaviour
{
    // Reference to the TextPopup prefab GameObject
    public GameObject popupTextPrefab;

    // Message to display, which can be set in the Inspector
    [TextArea]
    public string defaultMessage = "Hello, this is a popup message!";

    // Tag of the GameObject that should trigger the popup
    public string triggeringTag = "Player";

    // Delay in seconds before hiding the popup after the player leaves the trigger
    public float hideDelay = 2f;

    private Coroutine hideCoroutine;

    private void Start()
    {
        // Ensure the popup prefab is hidden initially
        if (popupTextPrefab != null)
        {
            popupTextPrefab.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the other GameObject has the correct tag
        if (other.CompareTag(triggeringTag) && popupTextPrefab != null)
        {
            // Show the popup prefab
            popupTextPrefab.SetActive(true);

            // Set the message on the TextMeshProUGUI component within the prefab
            var textComponent = popupTextPrefab.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = defaultMessage;
            }

            // Stop any ongoing hide coroutine to keep the popup visible while the player is inside the trigger
            if (hideCoroutine != null)
            {
                StopCoroutine(hideCoroutine);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Start the coroutine to hide the popup prefab after a delay when the player exits the trigger
        if (other.CompareTag(triggeringTag) && popupTextPrefab != null)
        {
            hideCoroutine = StartCoroutine(HidePopupPrefabAfterDelay());
        }
    }

    // Coroutine to hide the popup prefab after a delay
    private IEnumerator HidePopupPrefabAfterDelay()
    {
        yield return new WaitForSeconds(hideDelay);
        popupTextPrefab.SetActive(false);
    }
}
