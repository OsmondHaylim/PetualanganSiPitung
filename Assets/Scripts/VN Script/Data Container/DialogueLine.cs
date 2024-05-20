using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIALOGUE{
    public class DialogueLine
    {
        public string speaker;
        public DialogueData dialogue;
        public CommandData commands;

        public bool hasSpeaker => speaker != string.Empty;
        public bool hasDialogue => dialogue != null;
        public bool hasCommand => commands != null;

        public DialogueLine(string speaker, string dialogue, string commands){
            this.speaker = speaker;
            this.dialogue = (string.IsNullOrWhiteSpace(dialogue) ? null : new DialogueData(dialogue));
            this.commands = (string.IsNullOrWhiteSpace(commands) ? null : new CommandData(commands));
        }
    }
}

