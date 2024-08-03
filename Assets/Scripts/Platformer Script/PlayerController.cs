using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public float gForce;
    public float rotateSpeed;
    public CharacterController controller;
    public Animator anim;
    public Transform pivot;
    public GameObject model;

    public Vector3 moveDirection;
    private float knockBackTimeCounter;
    
    void Start(){
        controller = GetComponent<CharacterController>();
        if (controller == null)
            controller = gameObject.AddComponent<CharacterController>();
    }

    
    void LateUpdate()
    {
        if(knockBackTimeCounter <= 0){
            float storeY = moveDirection.y;
            moveDirection = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"));
            moveDirection = moveDirection.normalized * moveSpeed;
            moveDirection.y = storeY;
            if(controller.isGrounded){
                moveDirection.y = -gForce;
                if(Input.GetButtonDown("Jump")){
                    moveDirection.y = jumpForce;
                }
            }
        }else{
            knockBackTimeCounter -= Time.deltaTime;
        }

        moveDirection.y += Physics.gravity.y * gForce * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0){
            transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
            Quaternion newRotation;
            if(knockBackTimeCounter > 0){
                newRotation = Quaternion.LookRotation(new Vector3(-moveDirection.x, 0f, -moveDirection.z));
            }else{
                newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            }
            model.transform.rotation = Quaternion.Slerp(model.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }
        anim.SetBool("isGrounded", controller.isGrounded);
        anim.SetFloat("Speed", (Mathf.Abs(Input.GetAxisRaw("Vertical")) + Mathf.Abs(Input.GetAxisRaw("Horizontal"))));
    }

    public void KnockBack(float time, Vector3 direction, float force){
        knockBackTimeCounter = time;

        transform.rotation = Quaternion.Euler(0, 0, 0);
        // rb.AddForce(-direction * force, ForceMode.Impulse);
        moveDirection = direction * force;
        moveDirection.y += force/2;
    }
}
