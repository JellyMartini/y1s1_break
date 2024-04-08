using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Titanfall : MonoBehaviour
{
    private GameObject Player;
    public float playerHeight_half = 1f, camSpeed = 360f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) transform.rotation = Quaternion.identity;
        transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y + playerHeight_half, Player.transform.position.z);
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * camSpeed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.left * Input.GetAxis("Mouse Y") * camSpeed * Time.deltaTime, Space.Self);
    }
}
