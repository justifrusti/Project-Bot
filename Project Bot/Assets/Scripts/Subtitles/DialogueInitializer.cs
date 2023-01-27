using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInitializer : MonoBehaviour
{
    public Dialogue dialogueToInitialize;
    public SubtitleManager manager;

    [Header("Debug")]
    public bool initializeSubs;

    public void InitializeSubs()
    {
        manager.InitializeSubtitles(dialogueToInitialize);
    }

    private void Update()
    {
        //Debug
        if(initializeSubs)
        {
            InitializeSubs();
            initializeSubs = false;

            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            initializeSubs = true;

            Destroy(this.gameObject);
        }
    }
}
