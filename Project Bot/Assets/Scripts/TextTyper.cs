using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextTyper : MonoBehaviour
{
    [Header("Dialogue Components")]
    public TextMeshProUGUI textBox;
    public float timeBetweenChar;
    public string textToType;
    private bool isTyping;

    void Update()
    {
        if (textBox.text != textToType)
        {
            if (!isTyping)
            {
                textBox.text = "";
                isTyping = true;
                StartCoroutine(Type());
            }
        }
        else if (textBox.text == textToType)
        {
            isTyping = false;
        }
    }

    IEnumerator Type()
    {
        foreach (char letter in textToType)
        {
            textBox.text += letter;
            yield return new WaitForSeconds(timeBetweenChar);
        }
    }
}
