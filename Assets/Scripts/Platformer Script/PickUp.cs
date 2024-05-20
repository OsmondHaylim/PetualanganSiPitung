using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public int value;
    public ParticleSystem pickUpEffect;

    private void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            FindObjectOfType<GameManager>().AddMore(value);
            Instantiate(pickUpEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
