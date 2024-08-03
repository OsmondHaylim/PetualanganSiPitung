using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovableScript : MonoBehaviour
{
    public float pushForce;
    public float gForce;
    public float counteractingForceMultiplier;
    public Vector3 pushDirection;
    public Vector3 actualDirection;
    private bool isPushed = false;
    Rigidbody rb;
    private void Start(){
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
    private void OnCollisionEnter(Collision hitter){
        if(hitter.gameObject.CompareTag("Player")){
            pushDirection = transform.position - hitter.contacts[0].point;
            // pushDirection.Normalize();
            if (Math.Abs(pushDirection.x) > Math.Abs(pushDirection.z)){
                actualDirection = new Vector3(pushDirection.x, 0, 0);
                if ((rb.constraints & RigidbodyConstraints.FreezePositionX) != 0){
                    rb.constraints &= ~RigidbodyConstraints.FreezePositionX;
                }
                rb.constraints |= RigidbodyConstraints.FreezePositionZ;
            }else{
                actualDirection = new Vector3(0,0,pushDirection.z);
                if ((rb.constraints & RigidbodyConstraints.FreezePositionZ) != 0){
                    rb.constraints &= ~RigidbodyConstraints.FreezePositionZ;
                }
                rb.constraints |= RigidbodyConstraints.FreezePositionX;
            }
            actualDirection.y += Physics.gravity.y * gForce * Time.deltaTime;
            rb.AddForce(actualDirection * pushForce, ForceMode.Impulse);
        }
        if (hitter.gameObject.CompareTag("Boxes")){
            rb.constraints |= RigidbodyConstraints.FreezePositionX;
            rb.constraints |= RigidbodyConstraints.FreezePositionZ;
        }
    }
    void Update(){
        if (!isPushed){
            if ((rb.constraints & RigidbodyConstraints.FreezePositionX) != 0){
                rb.constraints &= ~RigidbodyConstraints.FreezePositionX;
            }
            if ((rb.constraints & RigidbodyConstraints.FreezePositionZ) != 0){
                rb.constraints &= ~RigidbodyConstraints.FreezePositionZ;
            }
        }
    }
    void FixedUpdate(){
        if (!isPushed){
            Vector3 counteractingForce = -rb.velocity * counteractingForceMultiplier;
            rb.AddForce(counteractingForce, ForceMode.Force);
            if ((rb.constraints & RigidbodyConstraints.FreezePositionX) != 0){
                rb.constraints &= ~RigidbodyConstraints.FreezePositionX;
            }
            if ((rb.constraints & RigidbodyConstraints.FreezePositionZ) != 0){
                rb.constraints &= ~RigidbodyConstraints.FreezePositionZ;
            }
        }
        rb.AddForce(Vector3.down * Physics.gravity.y * -(gForce) * rb.mass);
    }
}
