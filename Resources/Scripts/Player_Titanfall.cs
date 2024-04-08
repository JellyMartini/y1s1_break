using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Titanfall : MonoBehaviour
{
    private CharacterController controller;
    public float moveSpeed = 1f, jumpSpeed = 1f, gravitySpeed = 9.81f;
    private float gravityAccumulation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float camY = Camera.main.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        float camY_Cos = Mathf.Cos(camY);
        float camY_Sin = Mathf.Sin(camY);
        Vector3 velocity = new Vector3(Input.GetAxis("Horizontal") * camY_Cos + Input.GetAxis("Vertical") * camY_Sin, 0, 
            -(Input.GetAxis("Horizontal") * camY_Sin) + Input.GetAxis("Vertical") * camY_Cos);
        if (velocity.magnitude > 1)
            velocity.Normalize();
        velocity *= moveSpeed;
        if (!controller.isGrounded)
            gravityAccumulation -= gravitySpeed * Time.deltaTime;
        else gravityAccumulation = 0f;
        velocity += Vector3.up * (Input.GetAxis("Jump") * jumpSpeed + gravityAccumulation);
        velocity *= Time.deltaTime;
        controller.Move(velocity);
    }
}
