using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCheckpoint : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player"){
            Debug.Log("Checkpoint Reached");
            // FindObjectOfType<RespawnManager>().OnCheckpointReached();
        }
    }
}
