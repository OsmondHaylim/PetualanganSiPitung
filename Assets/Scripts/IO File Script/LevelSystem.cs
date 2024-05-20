using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public string key;
    // public int value;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(key + " : " + PlayerPrefs.GetInt(key));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
