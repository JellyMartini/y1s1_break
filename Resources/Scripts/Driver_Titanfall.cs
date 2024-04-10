using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver_Titanfall : MonoBehaviour
{
    // CONSTANTS
    public enum GameState {Play, Pause}; // All of the possible states of the game
    
    // PRIVATE MEMBERS
    private bool unlocked = true; // True if able to receive a "Cancel" input, false if not

    // PUBLIC MEMBERS
    public static GameState currentGameState; // The current state of the game
    
    // Start is called before the first frame update
    void Awake()
    {
        /*---------------------------------------------------------------------------------------------------//
                                                SEQUENCE BREAKDOWN
        //---------------------------------------------------------------------------------------------------//
        1. Start the game in the Pause state
        */
        // 1.
        currentGameState = GameState.Pause;
    }

    // Update is called once per frame
    void Update()
    {
        /*---------------------------------------------------------------------------------------------------//
                                                SEQUENCE BREAKDOWN
        //---------------------------------------------------------------------------------------------------//
        1. Check if "Cancel" was KeyDown. If not, skip to 5.
        2. Set "unlocked" to false, ensuring only one input per "Cancel" KeyDown
        3. Increment the game state with wrap-around
        4. Set the lock mode of the mouse dependent on the new game state
        5. Check if "Cancel" was KeyUp, allowing it to be pressed down again
        */
        // 1.
        if (Input.GetAxis("Cancel") > 0f && unlocked) 
        {
            // 2.
            unlocked = false;

            // 3.
            currentGameState++;
            if (currentGameState > GameState.Pause) currentGameState = GameState.Play;

            // 4.
            if (currentGameState == GameState.Play) Cursor.lockState = CursorLockMode.Locked;
            else if (currentGameState == GameState.Pause) Cursor.lockState = CursorLockMode.None;
        }

        // 5.
        if (Input.GetAxis("Cancel") <= 0f) unlocked = true;
    }

    // Generally available function to allow application quitting
    public static void QuitApplication()
    {
        Application.Quit();
    }
}
