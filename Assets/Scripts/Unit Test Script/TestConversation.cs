using DIALOGUE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestConversation : MonoBehaviour
{
    // Start is called before the first frame update
    public string TextAsset;
    void Awake()
    {
        StartConversation();
    }

    // Update is called once per frame
    void StartConversation()
    {
        List<string> lines = FileManager.ReadTextAsset(TextAsset);

        DialogueSystem.instance.Say(lines);
    }
}
