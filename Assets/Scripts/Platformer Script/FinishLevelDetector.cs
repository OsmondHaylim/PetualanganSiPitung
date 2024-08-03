using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLevelDetector : MonoBehaviour
{
    Collider thisCollider;
    public int level;
    void Start()
    {
        thisCollider = GetComponent<Collider>();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player"){
            if (PlayerPrefs.GetInt("Level") >  level)
                PlayerPrefs.SetInt("Level", level);
            FindObjectOfType<FinishLevelManager>().StartCoroutine("OnLevelCompleted");
        }
    }
    void OnTriggerExit (Collider other)
    {
        if(other.gameObject.tag == "Player")
            thisCollider.isTrigger = false;
    }
}
