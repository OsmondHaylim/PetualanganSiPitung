using System.Collections;
using UnityEngine;
using TMPro;

namespace DIALOGUE{
    public class TextArchitect
    {
        private TextMeshProUGUI tmpro_ui;
        private TextMeshPro tmpro_world;
        public TMP_Text tmpro => tmpro_ui != null ? tmpro_ui : tmpro_world;
        public string currentText => tmpro.text;
        public string targetText {get; private set; } = "";
        public string preText {get; private set; } = "";
        public string fullTargetText => preText + targetText;
        public float speed { get { return baseSpeed * speedMultiplier;} set { speedMultiplier = value;}}
        private const float baseSpeed = 1;
        private float speedMultiplier = 1;
        public int characterPerCycle { get { return speed <= 2f ? characterMultiplier : speed <= 2.5f ? characterMultiplier * 2 : characterMultiplier * 3;}}
        private int characterMultiplier = 1;
        public bool hurry = false;
        public TextArchitect(TextMeshProUGUI tmpro_ui){
            this.tmpro_ui = tmpro_ui;
        }
        public TextArchitect(TextMeshPro tmpro_world){
            this.tmpro_world = tmpro_world;
        }
        public Coroutine Build(string text){
            preText = "";
            targetText = text;
            Stop();
            buildProcess = tmpro.StartCoroutine(Building());
            return buildProcess;
        }

        public Coroutine Append(string text){
            preText = tmpro.text;
            targetText = text;
            Stop();
            buildProcess = tmpro.StartCoroutine(Building());
            return buildProcess;
        }
        private Coroutine buildProcess = null;
        public bool isBuilding => buildProcess != null;
        public void Stop(){
            if(!isBuilding)
                return;
            tmpro.StopCoroutine(buildProcess);
            buildProcess = null;
        }
        IEnumerator Building(){
            Prepare_Typewriter();
            yield return Build_Typewriter();
        }
        private void OnComplete(){
            buildProcess = null;
            hurry = false;
        }
        public void ForceComplete(){
            tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
        }
        private void Prepare_Typewriter(){
            tmpro.color = tmpro.color;
            tmpro.maxVisibleCharacters = 0;
            tmpro.text = preText;
            if(preText != ""){
                tmpro.ForceMeshUpdate();
                tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
            }
            tmpro.text += targetText;
            tmpro.ForceMeshUpdate();
        }
        private IEnumerator Build_Typewriter(){
            while(tmpro.maxVisibleCharacters < tmpro.textInfo.characterCount){
                tmpro.maxVisibleCharacters += hurry ? characterPerCycle * 5 : characterPerCycle;
                yield return new WaitForSeconds(0.015f / speed);
            }
            OnComplete();
        }
    }
}
