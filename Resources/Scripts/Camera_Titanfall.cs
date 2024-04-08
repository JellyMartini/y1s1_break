using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Titanfall : MonoBehaviour
{
    private GameObject Player;
    public float playerHeight_half = 1f, camSpeed = 360f;
    private float targetRot;
    // Start is called before the first frame update
    void Start()
    {
        targetRot = 0f;
        Cursor.lockState = CursorLockMode.Locked;
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.left * Input.GetAxis("Mouse Y") * camSpeed * Time.deltaTime, Space.Self);
        if (Player.GetComponent<Player_Titanfall>().currentPlayerState == Player_Titanfall.PlayerState.Wall_Left) 
        {
            Debug.Log("Touching Wall on Left");
            targetRot = -20f;
            //transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, -0.001f);
        }
        else if (Player.GetComponent<Player_Titanfall>().currentPlayerState == Player_Titanfall.PlayerState.Wall_Right)
        {
            Debug.Log("Touching Wall on Right");
            targetRot = 20f;
            //transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, 0.001f);
        }
        else
        {
            targetRot = 0f;
            //transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, 0f);
        }
        transform.localRotation = Quaternion.Euler
        (
            transform.localEulerAngles.x,
            transform.localEulerAngles.y, 
            Mathf.Lerp(transform.localEulerAngles.z, targetRot, 0.1f)
        );
    }
}
