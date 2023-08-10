using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public bool gamePaused;
    [SerializeField] GameObject menuOverlay;

    void Start()
    {
        menuOverlay.SetActive(false);
        gamePaused = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (!gamePaused)
            {
                Time.timeScale = 0;
                menuOverlay.SetActive(true);

                gamePaused = true;
            }
            else
            {
                Time.timeScale = 1;
                menuOverlay.SetActive(false);

                gamePaused = false;
            }
        }
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
        menuOverlay.SetActive(false);
        gamePaused = false;
    }

    public void RestartGame()
    {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
