using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayer : MonoBehaviour
{
    public int damage = 1;
    public bool damageOverTime = false;
    public float knockBackTime = 2;
    public float knockBackForce = 10;

    public ParticleSystem hurtEffect;
    private GameManager gameManager;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            Vector3 hitDirection = other.transform.position - transform.position;
            hitDirection = hitDirection.normalized;
            Instantiate(hurtEffect, other.transform.position, other.transform.rotation);
            gameManager.HurtPlayer(damage, knockBackTime, knockBackForce, hitDirection);
        }
    }

    private void OnCollideEnter(Collider other){
        if(other.tag == "Player"){
            Vector3 hitDirection = other.transform.position - transform.position;
            hitDirection = hitDirection.normalized;
            Instantiate(hurtEffect, other.transform.position, other.transform.rotation);
            gameManager.HurtPlayer(damage, knockBackTime, knockBackForce, hitDirection);
        }
    }
}
