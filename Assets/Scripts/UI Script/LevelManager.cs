using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public Button[] levelButtons;
    public TextMeshProUGUI wine1;
    public TextMeshProUGUI wine2;
    public TextMeshProUGUI wine3;
    private int level;
    public bool reset;
    // Start is called before the first frame update
    void Start()
    {
        
        if (reset){
            PlayerPrefs.SetInt("Level", 0);
            PlayerPrefs.SetInt("Wine1", 0);
            PlayerPrefs.SetInt("Wine2", 0);
            PlayerPrefs.SetInt("Wine3", 0);
            reset = !reset;
        }
        level = PlayerPrefs.GetInt("Level");
        Debug.Log("Saved Level Number: " + level);
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (level < i){
                levelButtons[i].interactable = false;
                levelButtons[i].image.color = Color.black;
            }else {
                levelButtons[i].interactable = true;
                levelButtons[i].image.color = Color.white;
            }   
        }
        wine1.text = "Wine: " + PlayerPrefs.GetInt("Wine1") + "/3";
        wine2.text = "Wine: " + PlayerPrefs.GetInt("Wine2") + "/5";
        wine3.text = "Wine: " + PlayerPrefs.GetInt("Wine3") + "/7";
    }
}
