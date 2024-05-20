using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(LineRenderer))]
public class WolfBehavior : MonoBehaviour
{
    public GameObject player;
    public Transform eyesightLineStart;
    // public float yRot = -33f;
    public float lineRange = 1f;
    public float rotSpeed = 50f;
    public float chargeSpeed = 5f;
    public Vector3 rotDirection = new Vector3();

    Vector3 collision = Vector3.zero;
    Vector3 lastCollision = Vector3.zero;
    Rigidbody rb;
    Vector3 moveDirection;
    bool isCharging = false;
    float chargeTime = 0f;

    // LineRenderer eyesightLine;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // eyesightLine = GetComponent<LineRenderer>();

    }


    // Update is called once per frame
    void Update()
    {
        if (isCharging)
        {
            if (lastCollision != collision || chargeTime >= 5f)
            {
                if (chargeTime >= 5f)
                {
                    isCharging = false;
                }
                else if (lastCollision != collision)
                {
                    lastCollision = collision;
                }
                chargeTime = 0f;
            }
            else
            {
                chargeTime += Time.deltaTime;
            }

            this.transform.Rotate(rotDirection * Time.deltaTime * rotSpeed * 0);

            Vector3 target = collision;
            this.transform.Translate(Vector3.forward * chargeSpeed * Time.deltaTime);
            if (this.transform.position.x <= (target.x + 1) && this.transform.position.x >= (target.x - 1))
            {
                if (this.transform.position.z <= (target.z + 1) && this.transform.position.z >= (target.z - 1))
                {
                    isCharging = false;
                }
            }
        }
        else
        {
            this.transform.Rotate(rotDirection * Time.deltaTime * rotSpeed);
            var ray = new Ray(eyesightLineStart.position, eyesightLineStart.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, lineRange))
            {
                if (hit.transform.gameObject == player)
                {
                    collision = hit.point;
                    isCharging = true;
                }
            }
        }
    }

    void FixedUpdate()
    {

    }
}
