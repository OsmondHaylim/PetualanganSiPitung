using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Transform xPivot;
    public Transform yPivot;
    private Vector3 offset;
    public float rotateSpeed;
    public float cameraDownPositionLimit;
    public float cameraUpAngleLimit;
    public float cameraDownAngleLimit;
    public bool invertY;
    void Start()
    {
        offset = target.position - transform.position;
        xPivot.transform.position = target.transform.position;
        yPivot.transform.position = target.transform.position;
        Cursor.lockState = CursorLockMode.Locked;
        xPivot.transform.parent = null;
        yPivot.transform.parent = null;
    }
    void LateUpdate()
    {        
        xPivot.transform.position = target.transform.position;
        yPivot.transform.position = target.transform.position;
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        yPivot.Rotate(0, horizontal, 0);
        float vertical = Input.GetAxis("Mouse Y") * rotateSpeed;
        if(invertY){
            xPivot.Rotate(-vertical, 0, 0);
        }else{
            xPivot.Rotate(vertical, 0, 0);
        }
        if(xPivot.rotation.eulerAngles.x > cameraUpAngleLimit && xPivot.rotation.eulerAngles.x <= 180f){
            xPivot.rotation = Quaternion.Euler(cameraUpAngleLimit,0f,0f);
        } else if (xPivot.rotation.eulerAngles.x > 180f && xPivot.rotation.eulerAngles.x < 360f - cameraDownAngleLimit){
            xPivot.rotation = Quaternion.Euler(360f - cameraDownAngleLimit,0f,0f);
        } 
        float desiredXAngle = xPivot.eulerAngles.x;
        float desiredYAngle = yPivot.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);
        transform.position = target.position - (rotation * offset);
        if(transform.position.y < target.position.y - cameraDownPositionLimit){
            transform.position = new Vector3(transform.position.x, target.position.y - cameraDownPositionLimit, transform.position.z);

        }
        transform.LookAt(target);
    }
}
