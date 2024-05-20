using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;


namespace TESTING{
    public class TestParse : MonoBehaviour
    {
        [SerializeField] private TextAsset file;
        void Start()
        {
            SendFileToParse();
            // string line = "Speaker \"Dialogue \\\"Goes\\\" In Here!\" Command(Arguments Here)";
            // DialogueParser.Parse(line);
        }

        void SendFileToParse()
        {
            List<string> lines = FileManager.ReadTextAsset("Prolog1", false);
            foreach(string line in lines){
                if(line == string.Empty)
                    continue;

                DialogueLine dl = DialogueParser.Parse(line);
            }
        }
    }

}
