using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Titanfall : MonoBehaviour
{
    private Driver_Titanfall gameDriver;
    private Canvas canvas;

    void Awake()
    {
        gameDriver = GetComponentInParent<Driver_Titanfall>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] paramFields = GameObject.FindGameObjectsWithTag("InputFields");
        float[] paramValues = new float[6] {Player_Titanfall.moveSpeed, Player_Titanfall.jumpSpeed, Player_Titanfall.gravitySpeed,
            Player_Titanfall.wallLaunchSpeed, Player_Titanfall.momentumDamp, Player_Titanfall.maxSpeed};
        for (int i = 0; i < paramFields.Length; i++) paramFields[i].GetComponentsInChildren<TMP_Text>()[0].text = paramValues[i].ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Driver_Titanfall.currentGameState == Driver_Titanfall.GameState.Pause) canvas.enabled = true;
        else canvas.enabled = false;
        
    }

    public void UpdateCamSpeed(Slider camSpeedSlider)
    {
        Debug.Log("CamSpeedSlider changed");
        Camera.main.GetComponent<Camera_Titanfall>().camSpeed = camSpeedSlider.value;
    }

    public void UpdateMoveSpeed(string _moveSpeed)
    {
        Player_Titanfall.moveSpeed = float.Parse(_moveSpeed);
    }

    public void UpdateJumpSpeed(string _jumpSpeed)
    {
        Player_Titanfall.jumpSpeed = float.Parse(_jumpSpeed);
    }

    public void UpdateGravitySpeed(string _gravitySpeed)
    {
        Player_Titanfall.gravitySpeed = float.Parse(_gravitySpeed);
    }

    public void UpdateWallLaunchSpeed(string _wallLaunchSpeed)
    {
        Player_Titanfall.wallLaunchSpeed = float.Parse(_wallLaunchSpeed);
    }

    public void UpdateMomentumDamp(string _momentumDamp)
    {
        Player_Titanfall.momentumDamp = float.Parse(_momentumDamp);
    }

    public void UpdateMaxSpeed(string _maxSpeed)
    {
        Player_Titanfall.maxSpeed = float.Parse(_maxSpeed);
    }
}
