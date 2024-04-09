using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Camera_Titanfall : MonoBehaviour
{
    private Player_Titanfall Player;
    public float playerHeight_half = 1f, camSpeed = 360f;
    private float targetRot, previousRot, lerpTime;
    private Player_Titanfall.PlayerState previousPlayerState;
    // Start is called before the first frame update
    void Start()
    {
        targetRot = 0f;
        previousRot = 0f;
        lerpTime = 0f;
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Titanfall>();
        previousPlayerState = Player.currentPlayerState;
        transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        if (Driver_Titanfall.currentGameState != Driver_Titanfall.GameState.Play) return;
        transform.Rotate(Vector3.left * Input.GetAxis("Mouse Y") * camSpeed * Time.deltaTime, Space.Self);
        if (Player.currentPlayerState != previousPlayerState)
        {
            if (Player.currentPlayerState == Player_Titanfall.PlayerState.Wall_Left) 
            {
                //Debug.Log("Touching Wall on Left");
                targetRot = -20f;
                previousRot = transform.localEulerAngles.z;
                lerpTime = 0f;
                Player.momentum = new Vector3(0f, Player.momentum.y / 2f, 0f);
            }
            else if (Player.currentPlayerState == Player_Titanfall.PlayerState.Wall_Right)
            {
                //Debug.Log("Touching Wall on Right");
                targetRot = 20f;
                previousRot = transform.localEulerAngles.z;
                lerpTime = 0f;
                Player.momentum = new Vector3(0f, Player.momentum.y / 2f, 0f);
            }
            else
            {
                targetRot = 0f;
                previousRot = transform.localEulerAngles.z;
                lerpTime = 0f;
            }
            //Debug.Log("Target Rot: " + targetRot.ToString() + "\nPrevious Rot: " + previousRot.ToString());
        }
        lerpTime += 10f * Time.deltaTime;
        transform.localRotation = Quaternion.Euler
        (
            transform.localEulerAngles.x,
            transform.localEulerAngles.y, 
            Mathf.LerpAngle(previousRot, targetRot, lerpTime)
        );
        previousPlayerState = Player.currentPlayerState;
    }
}
