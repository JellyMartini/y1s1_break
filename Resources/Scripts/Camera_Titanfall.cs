using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Camera_Titanfall : MonoBehaviour
{
    // PRIVATE MEMBERS
    private Player_Titanfall player; // A reference to the Player_Titanfall component of the Player for ease of access
    private Player_Titanfall.PlayerState previousPlayerState; // The PlayerState since the last change
    private float previousRot, targetRot, lerpTime; // the min, max and the t of the LerpAngle function
    
    // PROPERTIES
    public float CamSpeed { get; set; }

    void Awake()
    {
        /*---------------------------------------------------------------------------------------------------//
                                                SEQUENCE BREAKDOWN
        //---------------------------------------------------------------------------------------------------//
        1. Grab the reference to the Player GameObject's Player_Titanfall component as "player"
        2. Set private members to defaults
        3. Set properties to defaults
        */
        // 1.
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Titanfall>();
        
        // 2.
        previousPlayerState = player.CurrentPlayerState;
        previousRot = 0f;
        targetRot = 0f;
        lerpTime = 0f;

        // 3.
        CamSpeed = 200f;
    }

    // Update is called once per frame
    void Update()
    {
        /*---------------------------------------------------------------------------------------------------//
                                                SEQUENCE BREAKDOWN
        //---------------------------------------------------------------------------------------------------//
        1. Check the game state. Only proceed if in the Play state
        2. Rotate about the local X axis relative to the mouse's vertical movement since the last frame
        3. Check if the PlayerState has changed. If not, skip to 7.
        4. Update previousPlayerState to reflect the changed PlayerState
        5. Set lerpTime back to 0, so Lerp will evaluate to previousRot
        6. Set previousRot to the current Z axis rotation, the minimum of the Lerp
        7. Set the targetRot based on the PlayerState
        8. Increment lerpTime by a fixed amount. This increases how close to targetRot the Lerp function is evaluated to
        9. Update the rotation of the camera about the local Z axis to the Lerp function's evaluation 
        */
        // 1.
        if (Driver_Titanfall.currentGameState != Driver_Titanfall.GameState.Play) return;
        
        // 2.
        transform.Rotate(Vector3.left, Input.GetAxis("Mouse Y") * CamSpeed * Time.deltaTime, Space.Self);
        
        // 3.
        if (player.CurrentPlayerState != previousPlayerState)
        {
            // 4.
            previousPlayerState = player.CurrentPlayerState;
            
            // 5.
            lerpTime = 0f;
            // 6.
            previousRot = transform.localEulerAngles.z;

            // 7.
            switch (player.CurrentPlayerState)
            {
                case Player_Titanfall.PlayerState.Wall_Left:
                 targetRot = -20f;
                 break;
                case Player_Titanfall.PlayerState.Wall_Right:
                 targetRot = 20f;
                 break;
                default:
                 targetRot = 0f;
                 break;
            }

            
        }
        // 8.
        lerpTime += 10f * Time.deltaTime;
        
        // 9.
        transform.localRotation = Quaternion.Euler // Produces a Quaternion from a Vector3 of Euler Angles representing degrees about the 3 axes
        (
            transform.localEulerAngles.x,
            transform.localEulerAngles.y, 
            // LerpAngle(min, max, t): Creates a number line between min and max mapped 0 to 1
            // t represents how far along that number line to reference, clamped [0, 1]
            // for example: let min = 0, max = 4
            // then LerpAngle(0, 4, 0.5) = 2
            // LerpAngle outputs the result in the range [-180, 180], wrapping around upon overflow
            Mathf.LerpAngle(previousRot, targetRot, lerpTime)
        );
    }
}
