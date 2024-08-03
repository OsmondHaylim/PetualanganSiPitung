using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine;

namespace DIALOGUE{
    public class ConversationManager{
        public string sceneToLoad; 
        private DialogueSystem dialogueSystem => DialogueSystem.instance;
        private Coroutine process = null;
        public bool isRunning => process != null;
        private TextArchitect architect = null;
        private bool userPrompt = false;
        public ConversationManager(TextArchitect architect){
            this.architect = architect;
            dialogueSystem.onUserPrompt_Next += OnUserPrompt_Next;
        }
        private void OnUserPrompt_Next(){
            userPrompt = true;
        }
        public void StartConversation(List<string> conversation, string sceneToLoad){
            StopConversation();
            process = dialogueSystem.StartCoroutine(RunningConversation(conversation, sceneToLoad));
        }
        public void StopConversation(){
            if(!isRunning)
                return;
            dialogueSystem.StopCoroutine(process);
            process = null;
        }
        IEnumerator RunningConversation(List<string> conversation, string sceneToLoad){
            for (int i = 0; i < conversation.Count; i++)
            {
                if(string.IsNullOrWhiteSpace(conversation[i]))
                    continue;
                DialogueLine line = DialogueParser.Parse(conversation[i]);
                if (line.hasDialogue){
                    yield return Line_RunDialogue(line);
                }
            }
            SceneManager.LoadScene(sceneToLoad);
        }
        IEnumerator Line_RunDialogue(DialogueLine line){
            if(line.hasSpeaker && line.speaker != "Narator")
                dialogueSystem.ShowSpeakerName(line.speaker);
            else    
                dialogueSystem.HideSpeakerName();
            yield return BuildLineSegments(line.dialogue);
        }
        IEnumerator BuildDialogue(string dialogue, bool append){
            if(!append)
                architect.Build(dialogue);
            else
                architect.Append(dialogue);
            while (architect.isBuilding){
                if(userPrompt){
                    if(!architect.hurry)
                        architect.hurry = true;
                    else
                        architect.ForceComplete();
                    userPrompt = false;
                }
                yield return null;
            }
        }
        IEnumerator BuildLineSegments(DialogueData line){
            for (int i = 0; i < line.segments.Count; i++)
            {
                DialogueData.DialogueSegment segment = line.segments[i];
                yield return BuildDialogue(segment.dialogue, segment.append);
                yield return WaitForSignalTrigger(segment);
            }
        }
        IEnumerator WaitForSignalTrigger(DialogueData.DialogueSegment segment){
            switch(segment.wait){
                case false:
                    yield return WaitForInput();
                    break;
                case true:
                    yield return new WaitForSeconds(segment.Delay);
                    break;
            }
        }
        IEnumerator WaitForInput(){
            while(!userPrompt)
                yield return null;
            userPrompt = false;
        }
    }
}

