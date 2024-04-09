using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver_Titanfall : MonoBehaviour
{
    public enum GameState {Play, Pause};
    public static GameState currentGameState;
    bool unlocked = true;
    // Start is called before the first frame update
    void Awake()
    {
        currentGameState = GameState.Pause;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Cancel") > 0f && unlocked) 
        {
            unlocked = false;
            currentGameState++;
            if (currentGameState > GameState.Pause) currentGameState = GameState.Play;
            Debug.Log(currentGameState);
        }
        if (Input.GetAxis("Cancel") <= 0f) unlocked = true;
        ShouldCursorLock();
    }

    void ShouldCursorLock()
    {
        if (currentGameState == GameState.Play) Cursor.lockState = CursorLockMode.Locked;
        else if (currentGameState == GameState.Pause) Cursor.lockState = CursorLockMode.None;
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
