using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovableScript : MonoBehaviour
{
    public float pushForce;
    public CharacterController cc;
    public float gForce;
    public float counteractingForceMultiplier;
    public float drag;
    public Vector3 pushDirection;
    public Vector3 actualDirection;
    public Vector3 gravity;
    private bool isPushed = false;
    Rigidbody rb;

    private void Start(){
        rb = GetComponent<Rigidbody>();
        rb.drag = drag;
    }

    private void OnCollisionEnter(Collision hitter){
        if(hitter.gameObject.CompareTag("Player")){
            isPushed = true;
            pushDirection = transform.position - hitter.transform.position;
            pushDirection.Normalize();
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
            rb.AddForce(actualDirection * pushForce, ForceMode.VelocityChange);
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

    void LateUpdate(){
        gravity = new Vector3(0,0,0);
        gravity.y += Physics.gravity.y * gForce * Time.deltaTime;
        rb.AddForce(gravity * gForce * Time.deltaTime, ForceMode.Acceleration);
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
    }
}
