using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Titanfall : MonoBehaviour
{
    private CharacterController controller;
    public float moveSpeed = 1f, jumpSpeed = 1f, gravitySpeed = -9.81f, wallLaunchSpeed = 100f, momentumDamp = -10f;
    public Vector3 momentum = Vector3.zero;
    public enum PlayerState {Ground, Wall_Left, Wall_Right, Jumping};
    public PlayerState currentPlayerState = PlayerState.Ground;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        // MOVEMENT
        float camY = Camera.main.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        float camY_Cos = Mathf.Cos(camY);
        float camY_Sin = Mathf.Sin(camY);

        Vector3 velocity = new Vector3(Input.GetAxis("Horizontal") * camY_Cos + Input.GetAxis("Vertical") * camY_Sin, 0, 
            -(Input.GetAxis("Horizontal") * camY_Sin) + Input.GetAxis("Vertical") * camY_Cos);
        if (velocity.magnitude > 1) velocity.Normalize();
        velocity *= moveSpeed; // XZ Plane movement Complete
        if (currentPlayerState == PlayerState.Jumping)
            momentum.y += gravitySpeed * Time.deltaTime;
        else if (currentPlayerState == PlayerState.Wall_Left || currentPlayerState == PlayerState.Wall_Right)
            momentum.y += gravitySpeed / 4 * Time.deltaTime;
        else if (currentPlayerState == PlayerState.Ground) momentum.y = 0f;

        // INPUTS
        if (Input.GetKeyDown(KeyCode.LeftShift)) moveSpeed = 100f;
        if (Input.GetKeyUp(KeyCode.LeftShift)) moveSpeed = 10f;
        if (Input.GetAxis("Jump") > 0 && currentPlayerState != PlayerState.Jumping)
        {
            momentum.y += jumpSpeed;
            if (currentPlayerState == PlayerState.Wall_Left || currentPlayerState == PlayerState.Wall_Right)
            {
                if (currentPlayerState == PlayerState.Wall_Left) momentum.x = -wallLaunchSpeed;
                else momentum.x = wallLaunchSpeed;
                momentum.x *= Mathf.Sign((transform.eulerAngles.y + 90f) % 360 - 180f); 
            }
            currentPlayerState = PlayerState.Jumping;
        }
        if (momentum.y > jumpSpeed) momentum.y = jumpSpeed;
        if (momentum.x > 0) momentum.x += momentumDamp * Time.deltaTime;
        else if (momentum.x < 0) momentum.x -= momentumDamp * Time.deltaTime;
        if (momentum.z > 0) momentum.z += momentumDamp * Time.deltaTime;
        else momentum.z -= momentumDamp * Time.deltaTime;

        velocity += momentum;
        velocity *= Time.deltaTime;
        controller.Move(velocity);
        //Debug.Log(currentPlayerState);
        if (Input.GetKeyDown(KeyCode.E)) transform.rotation = Quaternion.identity;
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Camera.main.GetComponent<Camera_Titanfall>().camSpeed * Time.deltaTime, Space.World);
        //Debug.Log((transform.eulerAngles.y + 90f) % 360 - 180f);
    }

    void FixedUpdate()
    {
        if (!CheckForWall())
        {
            if (controller.isGrounded) currentPlayerState = PlayerState.Ground;
            else currentPlayerState = PlayerState.Jumping;
        }
    }

    bool CheckForWall()
    {   
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, 1f))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * hit.distance, Color.red);
            currentPlayerState = PlayerState.Wall_Right;
            return true;
        }
        else Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right), Color.green);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hit, 1f))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * hit.distance, Color.red);
            currentPlayerState = PlayerState.Wall_Left;
            return true;
        }
        else Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left), Color.green);
        return false;
    }

    void OnTriggerEnter(Collider other)
    {
        
        controller.Move(new Vector3(0, 1, -49) - transform.position);
        transform.rotation = Quaternion.identity;
        Camera.main.transform.rotation = Quaternion.identity;
        momentum = Vector3.zero;
    }
}
