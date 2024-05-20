using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;


namespace TESTING{
    public class test : MonoBehaviour
    {
        DialogueSystem ds;
        TextArchitect architect;

        string[] lines = new string[5]{
            "Testing.",
            "Hello, you guys hear me? maybe read me?",
            "Great, you probably read this.",
            "Congratulation, this thing did it's job succesfully!",
            "Now back to work."
        };


        // Start is called before the first frame update
        void Start()
        {
            ds = DialogueSystem.instance;
            architect = new TextArchitect(ds.dialogueContainer.dialogueText);
            architect.buildMethod = TextArchitect.BuildMethod.fade;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)){
                if (!architect.hurry)
                    architect.hurry = true;
                else
                    architect.ForceComplete();

                architect.Build(lines[Random.Range(0, lines.Length)]);
            }else if (Input.GetKeyDown(KeyCode.A)){
                architect.Append(lines[Random.Range(0, lines.Length)]);
            }
        }
    }

}
