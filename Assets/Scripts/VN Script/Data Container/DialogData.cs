using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System;

public class DialogueData
{
    public List<DialogueSegment> segments;
    private const string segmentRegex = @"\{[ca]\}|\{w[ca]\s\d*\.?\d*\}";
    public bool hasDialogue => segments.Count > 0;
    public DialogueData(string rawDialogue){
        segments = RipSegment(rawDialogue);
    }
    public List<DialogueSegment> RipSegment(string rawDialogue){
        List<DialogueSegment> segments = new List<DialogueSegment>();
        MatchCollection matches = Regex.Matches(rawDialogue, segmentRegex);
        int lastIndex = 0;
        DialogueSegment segment = new DialogueSegment();
        segment.dialogue = (matches.Count == 0 ? rawDialogue : rawDialogue.Substring(0, matches[0].Index));
        segment.startSignal = DialogueSegment.StartSignal.NONE;
        segment.Delay = 0;
        segments.Add(segment);
        if(matches.Count == 0)
            return segments;
        else
            lastIndex = matches[0].Index;

        for (int i = 0; i < matches.Count; i++){
            Match match = matches[i];
            segment = new DialogueSegment();
            string signalMatch = match.Value;
            signalMatch = signalMatch.Substring(1, match.Length - 2);
            string[] signalSplit = signalMatch.Split(' ');
            segment.startSignal = (DialogueSegment.StartSignal) Enum.Parse(typeof(DialogueSegment.StartSignal), signalSplit[0].ToUpper());
            if(signalSplit.Length > 1)
                float.TryParse(signalSplit[1], out segment.Delay);
            int nextIndex = i + 1 <  matches.Count ? matches[i + 1].Index : rawDialogue.Length;
            segment.dialogue = rawDialogue.Substring(lastIndex + match.Length, nextIndex - (lastIndex + match.Length)); 
            lastIndex = nextIndex;
            segments.Add(segment);
        }
        return segments;
    }
    public struct DialogueSegment{
        public string dialogue;
        public StartSignal startSignal;
        public float Delay;
        public enum StartSignal { NONE, C, A, WC, WA}
        public bool append => (startSignal == StartSignal.A || startSignal == StartSignal.WA);
        public bool wait => (startSignal == StartSignal.WC || startSignal == StartSignal.WA);
    }
}
