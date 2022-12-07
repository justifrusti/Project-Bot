using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Subtitles/New Subtitle", fileName = "New Subtitle")]
public class Dialogue : ScriptableObject
{
    [System.Serializable]
    public class Conversation
    {
        public Line[] englishConversation;
        public Line[] dutchConversation;
        public Line[] frenchConversation;
        public Line[] germanConversation;
    }

    [System.Serializable]
    public class Line
    {
        public string text;
        [Space]
        public float timeBetweenSentences;
        [Space]
        public AudioClip spokenDialogue;
    }

    public Conversation conversation;
}
