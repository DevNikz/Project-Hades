using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;

public class DialogueView : MonoBehaviour
{
    [SerializeField] private TMP_Text _textbox;
    private bool _isTextCrawling = false;

    public void DialogueBoxClickCallback()
    {
        if (_isTextCrawling)
        {
            _isTextCrawling = false;
            return;
        }

        string text = DialogueManager.Instance.GetNextDialogueLine();
        if (text.IsNullOrWhitespace())
        {
            _textbox.text = "";
            return;
        }

        StartCoroutine(TextCrawl(text, DialogueManager.Instance.TextCrawlTimePerCharacter));
    }

    IEnumerator TextCrawl(string text, float timePerCharacter)
    {
        _isTextCrawling = true;
        float elapsedTime = 0.0f;
        int index = 0;
        while (index < text.Length && _isTextCrawling)
        {
            elapsedTime += Time.unscaledDeltaTime;
            if (elapsedTime >= timePerCharacter)
            {
                _textbox.text = text.Substring(0, index);
                index++;
                elapsedTime = 0.0f;
            }
            yield return new WaitForEndOfFrame();
        }
        _textbox.text = text;

        _isTextCrawling = false;
    }
}
