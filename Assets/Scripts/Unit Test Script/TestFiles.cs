using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

namespace TESTING{
    public class TestFiles : MonoBehaviour
    {
        [SerializeField]private TextAsset fileName;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Run());
        }

        // Update is called once per frame
        IEnumerator Run(){
            List<string> lines = FileManager.ReadTextAsset(fileName, true);
            foreach(string line in lines)
                Debug.Log(line);

            yield return null;

        }
    }
}

