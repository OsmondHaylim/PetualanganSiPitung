using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIALOGUE
{
    public class DialogueSystem : MonoBehaviour
    {
        public string sceneToLoad; 
        public DialogueContainer dialogueContainer = new DialogueContainer();
        private ConversationManager conversationManager;
        private TextArchitect architect;
        public string TextAsset;
        public static DialogueSystem instance {get; private set;}
        public delegate void DialogueSystemEvent();
        public event DialogueSystemEvent onUserPrompt_Next;
        private bool _initialized = false;
        private void Awake(){
            if(instance == null){
                instance = this;
                if(_initialized)
                    return;
                architect = new TextArchitect(dialogueContainer.dialogueText);
                conversationManager = new ConversationManager(architect);
                _initialized = true;
            }else
                DestroyImmediate(gameObject);
            List<string> lines = FileManager.ReadTextAsset(TextAsset);
            instance.Say(lines, sceneToLoad);
            
        }
        public void OnUserPrompt_Next(){
            onUserPrompt_Next?.Invoke();
        }
        public void ShowSpeakerName(string speakerName = "") => dialogueContainer.nameContainer.Show(speakerName);
        public void HideSpeakerName() => dialogueContainer.nameContainer.Hide();
        public void Say(List<string> conversation, string sceneToLoad){
            conversationManager.StartConversation(conversation, sceneToLoad);
        }
    }
}

