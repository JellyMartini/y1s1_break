using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Titanfall : MonoBehaviour
{
    // CONSTANTS
    public enum PlayerState {Ground, Airborne, Wall_Left, Wall_Right}; // All of the possible states of a player

    // PRIVATE MEMBERS
    private CharacterController controller; // A reference to the CharacterController for ease of access
    private LayerMask layerMaskDefault; // Masks out the Player layer from Raycasts

    // PUBLIC MEMBERS
    public float wallDetectDist; // the length of the line that detects walls

    // PROPERTIES
    public float MoveSpeed { get; set; }
    public float JumpSpeed { get; set; }
    public float Gravity { get; set; }
    public float WallLaunchSpeed { get; set; }
    public float MaxSpeed { get; set; }
    public float Damp { get; set; }
    public Vector3 Momentum { get; set; }
    public PlayerState CurrentPlayerState { get; set; }

    void Awake()
    {
        /*-------------------------------------------------------------------------------------------------------------------------------//
                                                        SEQUENCE BREAKDOWN
        //-------------------------------------------------------------------------------------------------------------------------------//
        1. Set "controller" as a reference to the CharacterController component of the Player GameObject
        2. Set "layerMaskDefault" as a bitmask of the "Player" layer
        3. Set private members to default values
        4. Set properties to default values
        */
        // 1.
        controller = GetComponent<CharacterController>();
        // 2.
        layerMaskDefault = ~LayerMask.GetMask("Player", "Ignore Raycast");
        // 3.
        wallDetectDist = 0.7f;
        // 4.
        MoveSpeed = 10f;
        JumpSpeed = 10f;
        Gravity = -20f;
        WallLaunchSpeed = 20f;
        MaxSpeed = 15f;
        Damp = -30f;
        Momentum = Vector3.zero;
        CurrentPlayerState = PlayerState.Ground;
        
    }

    // Update is called once per frame
    void Update()
    {
        /*-------------------------------------------------------------------------------------------------------------------------------//
                                                        SEQUENCE BREAKDOWN
        //-------------------------------------------------------------------------------------------------------------------------------//
        1.  Check the game state. If it isn't Play, end the update.
        2.  If E is pressed, reset the local Y rotation of the player
        3.  Rotate about the Y axis relative to the mouse's horizontal movement since the last frame
        4.  Evaluate and store the sin and cos components of the Player's current rotation about the local Y axis
        5.  Create a temporary variable to store the Player's current Momentum, allowing its xyz components to be manipulated individually
        6.  Create a variable for storing the movement vector, the velocity at this moment. Initialise it from the input at that moment
        7.  Ensure that the magnitude of the initial velocity doesn't exceed 1
        8.  Rescale the velocity to the range [0, moveSpeed]
        9.  Add Gravity to the Momentum, scaled dependent on the Player State
        10.  Check if Jump has been pressed AND that the Player isn't Airborne. If not, skip to 16.
        11. Add JumpSpeed to the Momentum's y component
        12. Check if the Player is touching a wall. If not, skip to 15.
        13. Push away from the wall with 
        14. Flip the sign of the wall push if the player is facing away from the starting direction, allowing for relative left and right
        15. Set Player's state to Airborne
        16. Cap the Momentum's Y component to JumpSpeed, otherwise the player can jump twice as high off of the wall
        17. Dampen the Momentum on the XZ plane, with a target of 0
        18. Reassign Momentum to the modified tempMomentum with all of the transformations on it
        19. Add the Momentum to the velocity
        20. Create a new Vector2 representing the velocity of the player on the XZ plane
        21. Check if the Player is moving too quickly on the XZ plane. If not, skip to 24.
        22. Normalise the XZ velocity and rescale to MaxSpeed. This proportionally caps the velocity in those directions. 
        23. Replace the old XZ with the new XZ.
        24. Adjust the velocity for variable frame durations
        25. Get the CharacterController to move the Player by the velocity
        */
        // 1.
        if (Driver_Titanfall.currentGameState != Driver_Titanfall.GameState.Play) return;

        // 2.
        if (Input.GetKeyDown(KeyCode.E)) transform.rotation = Quaternion.identity;
        
        // 3.
        transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * Camera.main.GetComponent<Camera_Titanfall>().CamSpeed * Time.deltaTime);

        // 4.
        float camY_Cos = Mathf.Cos(transform.rotation.eulerAngles.y * Mathf.Deg2Rad);
        float camY_Sin = Mathf.Sin(transform.rotation.eulerAngles.y * Mathf.Deg2Rad);
        
        // 5.
        Vector3 tempMomentum = Momentum;
        
        // 6.
        Vector3 velocity = new Vector3(
            Input.GetAxis("Horizontal") * camY_Cos + Input.GetAxis("Vertical") * camY_Sin,
            0, 
            -(Input.GetAxis("Horizontal") * camY_Sin) + Input.GetAxis("Vertical") * camY_Cos
        );

        // 7.
        if (velocity.magnitude > 1) velocity.Normalize();

        // 8.
        velocity *= MoveSpeed; // XZ Plane movement Complete

        // 9.
        switch (CurrentPlayerState)
        {
            case PlayerState.Airborne:
             tempMomentum.y += Gravity * Time.deltaTime;
             break;
            case PlayerState.Wall_Left:
            case PlayerState.Wall_Right:
             tempMomentum.y += Gravity / 2f * Time.deltaTime; // Gravity has a lessened impact when wallrunning
             break;
            default:
             tempMomentum.y = 0f;
             break;
        }

        // 10.
        if (Input.GetAxis("Jump") > 0 && CurrentPlayerState != PlayerState.Airborne)
        {
            // 11.
            tempMomentum.y += JumpSpeed;
            
            // 12.
            if (CurrentPlayerState == PlayerState.Wall_Left || CurrentPlayerState == PlayerState.Wall_Right)
            {
                // 13.
                if (CurrentPlayerState == PlayerState.Wall_Left) tempMomentum.x = -WallLaunchSpeed;
                else tempMomentum.x = WallLaunchSpeed;
                
                // 14.
                tempMomentum.x *= Mathf.Sign((transform.eulerAngles.y + 90f) % 360 - 180f); 
            }
            
            // 15.
            CurrentPlayerState = PlayerState.Airborne;
        }
        // 16.
        if (tempMomentum.y > JumpSpeed) tempMomentum.y = JumpSpeed;

        // 17.
        if (tempMomentum.x > 0.01) tempMomentum.x += Damp * Time.deltaTime;
        else if (tempMomentum.x < -0.01) tempMomentum.x -= Damp * Time.deltaTime;
        else tempMomentum.x = 0f;
        
        if (tempMomentum.z > 0.01) tempMomentum.z += Damp * Time.deltaTime;
        else if (tempMomentum.z < -0.01) tempMomentum.z -= Damp * Time.deltaTime;
        else tempMomentum.z = 0f;

        // 18.
        Momentum = tempMomentum;
        
        // 19.
        velocity += Momentum;

        // 20.
        Vector2 xzVelocity = new Vector2(velocity.x, velocity.z);
        
        // 21.
        if (xzVelocity.magnitude > MaxSpeed)
        {
            // 22.
            xzVelocity.Normalize();
            xzVelocity *= MaxSpeed;

            // 23.
            velocity = new Vector3(xzVelocity.x, velocity.y, xzVelocity.y);
        }

        // 24.
        velocity *= Time.deltaTime;
        
        // 25.
        controller.Move(velocity);
    }

    // PHYSICS
    void FixedUpdate()
    {
        // Check if the player is touching a wall. If not, check if it's grounded. If not, it's Airborne.
        if (!CheckForWall())
        {
            if (controller.isGrounded) CurrentPlayerState = PlayerState.Ground;
            else CurrentPlayerState = PlayerState.Airborne;
        }
    }

    bool CheckForWall()
    {   
        // Is there a wall to the right?
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out RaycastHit hit, wallDetectDist, layerMaskDefault.value))
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * hit.distance, Color.red);
            CurrentPlayerState = PlayerState.Wall_Right; // Set PlayerState
            return true; // Player is touching wall
        }
        //else Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right), Color.green);
        else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hit, wallDetectDist, layerMaskDefault.value))
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * hit.distance, Color.red);
            CurrentPlayerState = PlayerState.Wall_Left; // Set PlayerState
            return true; // Player is touching wall
        }
        //else Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left), Color.green);
        return false; // If neither return true, then no wall was hit
    }

    void OnTriggerEnter(Collider other)
    {
        // If Player collided with a trigger, but not with the DeathPlane trigger, then exit
        if (other != GameObject.Find("DeathPlane").GetComponent<Collider>()) return;

        // Reset Player and Camera transforms
        controller.Move(new Vector3(0, 1, -49) - transform.position);
        transform.rotation = Quaternion.identity;
        Camera.main.transform.rotation = Quaternion.identity;
        Momentum = Vector3.zero;
    }
}
