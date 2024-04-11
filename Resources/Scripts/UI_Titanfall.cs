using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Titanfall : MonoBehaviour
{
    // PRIVATE MEMBERS
    private Player_Titanfall player;
    private Canvas canvas;

    void Awake()
    {
        /*---------------------------------------------------------------------------------------------------//
                                                SEQUENCE BREAKDOWN
        //---------------------------------------------------------------------------------------------------//
        1. Grab the references for the Player as "player" and the Canvas as "canvas"
        */
        // 1.
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Titanfall>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    // Start is called before the first frame update
    void Start()
    {
        /*---------------------------------------------------------------------------------------------------//
                                                SEQUENCE BREAKDOWN
        //---------------------------------------------------------------------------------------------------//
        1. Grab the debug Input Fields for the Player properties
        2. Create an array that contains the relevant Player properties in the same order as the corresponding Input Fields
        3. Set the placeholder text of each Input Field to the corresponding Player property
        */
        // 1.
        GameObject[] paramFields = GameObject.FindGameObjectsWithTag("InputFields");
        
        // 2.
        float[] paramValues = new float[7] {player.MoveSpeed, player.JumpSpeed, player.Gravity,
            player.WallLaunchSpeed, player.MaxSpeed, player.Damp, player.wallDetectDist };
        
        // 3.
        foreach (var paramField in GameObject.FindGameObjectsWithTag("InputFields")) // Unity is real weird about grabbing stuff in order, so you have to verify
        {
            if (paramField.name == "MoveSpeedInputField") paramField.GetComponent<TMP_InputField>().text = paramValues[0].ToString();
            else if (paramField.name == "JumpSpeedInputField") paramField.GetComponent<TMP_InputField>().text = paramValues[1].ToString();
            else if (paramField.name == "GravityInputField") paramField.GetComponent<TMP_InputField>().text = paramValues[2].ToString();
            else if (paramField.name == "WallLaunchSpeedInputField") paramField.GetComponent<TMP_InputField>().text = paramValues[3].ToString();
            else if (paramField.name == "MaxSpeedInputField") paramField.GetComponent<TMP_InputField>().text = paramValues[4].ToString();
            else if (paramField.name == "MomentumDampInputField") paramField.GetComponent<TMP_InputField>().text = paramValues[5].ToString();
            else if (paramField.name == "WallDetectDistInputField") paramField.GetComponent<TMP_InputField>().text = paramValues[6].ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*---------------------------------------------------------------------------------------------------//
                                                SEQUENCE BREAKDOWN
        //---------------------------------------------------------------------------------------------------//
        1. Show and allow interactions to the Canvas dependent on the Game State
        */
        // 1.
        if (Driver_Titanfall.currentGameState == Driver_Titanfall.GameState.Pause) canvas.enabled = true;
        else canvas.enabled = false;
    }

    // PUBLICLY ACCESSIBLE FUNCTIONS FOR UPDATING CAMERA PROPERTIES
    public void UpdateCamSpeed(Slider camSpeedSlider) { Camera.main.GetComponent<Camera_Titanfall>().CamSpeed = camSpeedSlider.value; }

    // PUBLICLY ACCESSIBLE FUNCTIONS FOR UPDATING PLAYER PROPERTIES
    public void UpdateMoveSpeed(string _moveSpeed) { player.MoveSpeed = float.Parse(_moveSpeed); }

    public void UpdateJumpSpeed(string _jumpSpeed) { player.JumpSpeed = float.Parse(_jumpSpeed); }

    public void UpdatePlayerGravity(string _gravity) { player.Gravity = float.Parse(_gravity); }

    public void UpdateWallLaunchSpeed(string _wallLaunchSpeed) { player.WallLaunchSpeed = float.Parse(_wallLaunchSpeed); }

    public void UpdateMaxSpeed(string _maxSpeed) { player.MaxSpeed = float.Parse(_maxSpeed); }

    public void UpdateDamp(string _damp) { player.Damp = float.Parse(_damp); }

    public void UpdateWallDetectDist(string _wallDetectDist) { player.wallDetectDist = float.Parse(_wallDetectDist); }
}
