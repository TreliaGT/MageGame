using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseControls : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject Gamescreen;
    bool paused = false;
    public PlayerController player;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    void Pause()
    {
        paused = true;
        pauseMenu.SetActive(true);
        Gamescreen.SetActive(false);
        Time.timeScale = 0f;
        player.enabled = false;
    }

    public void Resume()
    {
        paused = false;
        pauseMenu.SetActive(false);
        Gamescreen.SetActive(true);
        Time.timeScale = 1f;
        player.enabled = true;
    }
}
