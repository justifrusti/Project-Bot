using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SubtitleManager : MonoBehaviour
{
    public enum Language
    {
        English,
        Dutch,
        French,
        German
    }

    public Language language;

    public Dialogue currentDialogue;

    public List<string> subtitles;
    public List<float> timeBetweenSentences;
    public List<AudioClip> spokenDialogue;

    [Header("Dialogue Components")]
    public TextMeshProUGUI textBox;
    public float timeBetweenChar;
    public string textToType;
    [Space]
    public AudioSource dialogueSpeaker;
    //Private Variables
    private bool isTyping;
    [SerializeField]private int index;
    private bool startedSequence;

    public void InitializeSubtitles(Dialogue dialogueToUse)
    {
        currentDialogue = dialogueToUse;

        switch(language)
        {
            default:
                for (int i = 0; i < currentDialogue.conversation.englishConversation.Length; i++)
                {
                    if (!subtitles.Contains(currentDialogue.conversation.englishConversation[i].text))
                    {
                        subtitles.Add(currentDialogue.conversation.englishConversation[i].text);
                        timeBetweenSentences.Add(currentDialogue.conversation.englishConversation[i].timeBetweenSentences);
                        spokenDialogue.Add(currentDialogue.conversation.englishConversation[i].spokenDialogue);
                    }
                }
                break;

            case Language.English:
                for (int i = 0; i < currentDialogue.conversation.englishConversation.Length; i++)
                {
                    if (!subtitles.Contains(currentDialogue.conversation.englishConversation[i].text))
                    {
                        subtitles.Add(currentDialogue.conversation.englishConversation[i].text);
                        timeBetweenSentences.Add(currentDialogue.conversation.englishConversation[i].timeBetweenSentences);
                        spokenDialogue.Add(currentDialogue.conversation.englishConversation[i].spokenDialogue);
                    }
                }
                break;

            case Language.Dutch:
                for (int i = 0; i < currentDialogue.conversation.dutchConversation.Length; i++)
                {
                    if (!subtitles.Contains(currentDialogue.conversation.dutchConversation[i].text))
                    {
                        subtitles.Add(currentDialogue.conversation.dutchConversation[i].text);
                        timeBetweenSentences.Add(currentDialogue.conversation.dutchConversation[i].timeBetweenSentences);
                        spokenDialogue.Add(currentDialogue.conversation.dutchConversation[i].spokenDialogue);
                    }
                }
                break;

            case Language.French:
                for (int i = 0; i < currentDialogue.conversation.frenchConversation.Length; i++)
                {
                    if (!subtitles.Contains(currentDialogue.conversation.frenchConversation[i].text))
                    {
                        subtitles.Add(currentDialogue.conversation.frenchConversation[i].text);
                        timeBetweenSentences.Add(currentDialogue.conversation.frenchConversation[i].timeBetweenSentences);
                        spokenDialogue.Add(currentDialogue.conversation.frenchConversation[i].spokenDialogue);
                    }
                }
                break;

            case Language.German:
                for (int i = 0; i < currentDialogue.conversation.germanConversation.Length; i++)
                {
                    if (!subtitles.Contains(currentDialogue.conversation.germanConversation[i].text))
                    {
                        subtitles.Add(currentDialogue.conversation.germanConversation[i].text);
                        timeBetweenSentences.Add(currentDialogue.conversation.germanConversation[i].timeBetweenSentences);
                        spokenDialogue.Add(currentDialogue.conversation.germanConversation[i].spokenDialogue);
                    }
                }
                break;
        }

        textBox.gameObject.SetActive(true);
        textToType = subtitles[index];

        dialogueSpeaker.clip = spokenDialogue[index];
        dialogueSpeaker.Play();

        if (!startedSequence)
        {
            StartCoroutine(NextSentenceTimer(startedSequence, timeBetweenSentences[index]));
        }
    }

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

    public void NextSentence()
    {
        index++;
        
        if(index <= subtitles.Count - 1)
        {
            textToType = subtitles[index];

            dialogueSpeaker.clip = spokenDialogue[index];
            dialogueSpeaker.Play();

            if (!startedSequence)
            {
                startedSequence = true;
                StartCoroutine(NextSentenceTimer(startedSequence, timeBetweenSentences[index]));
            }
        }else if (index >= subtitles.Count)
        {
            DeInitializeSubtitles();
        }
    }

    public void DeInitializeSubtitles()
    {
        currentDialogue = null;
        index = 0;

        textBox.gameObject.SetActive(false);
    }

    IEnumerator Type()
    {
        foreach (char letter in textToType)
        {
            textBox.text += letter;
            yield return new WaitForSeconds(timeBetweenChar);
        }
    }

    IEnumerator NextSentenceTimer(bool boolToSwitchBack, float timeForNextSentence)
    {
        yield return new WaitForSeconds(timeForNextSentence);

        boolToSwitchBack = false;
        NextSentence();
    }
}
