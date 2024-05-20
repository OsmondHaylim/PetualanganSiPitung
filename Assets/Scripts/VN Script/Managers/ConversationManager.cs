using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DIALOGUE{
    public class ConversationManager{
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
        
        public void StartConversation(List<string> conversation){
            StopConversation();

            process = dialogueSystem.StartCoroutine(RunningConversation(conversation));
        }

        public void StopConversation(){
            if(!isRunning)
                return;

            dialogueSystem.StopCoroutine(process);
            process = null;
        }
        IEnumerator RunningConversation(List<string> conversation){
            for (int i = 0; i < conversation.Count; i++)
            {
                if(string.IsNullOrWhiteSpace(conversation[i]))
                    continue;

                DialogueLine line = DialogueParser.Parse(conversation[i]);

                if (line.hasDialogue){
                    yield return Line_RunDialogue(line);
                }

                if (line.hasCommand){
                    yield return Line_RunCommands(line);
                }
            }
        }

        IEnumerator Line_RunDialogue(DialogueLine line){
            if(line.hasSpeaker)
                dialogueSystem.ShowSpeakerName(line.speaker);

            yield return BuildLineSegments(line.dialogue);
            yield return WaitForInput();
        }

        IEnumerator Line_RunCommands(DialogueLine line){
            Debug.Log(line.commands);
            yield return null;
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
                yield return WaitForSignalTrigger(segment);
                yield return BuildDialogue(segment.dialogue, segment.append);

            }
        }

        IEnumerator WaitForSignalTrigger(DialogueData.DialogueSegment segment){
            switch(segment.startSignal){
                case DialogueData.DialogueSegment.StartSignal.C:
                case DialogueData.DialogueSegment.StartSignal.A:
                    yield return WaitForInput();
                    break;
                case DialogueData.DialogueSegment.StartSignal.WA:
                case DialogueData.DialogueSegment.StartSignal.WC:
                    yield return new WaitForSeconds(segment.Delay);
                    break;
                default:
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

