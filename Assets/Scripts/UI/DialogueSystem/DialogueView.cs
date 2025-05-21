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

        int index = 0;
        while (index < text.Length && _isTextCrawling)
        {
            _textbox.text = text.Substring(0, index);
            index++;
            yield return new WaitForSeconds(timePerCharacter);
        }
        _textbox.text = text;

        _isTextCrawling = false;
    }
}
