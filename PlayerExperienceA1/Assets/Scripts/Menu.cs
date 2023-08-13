using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public bool gamePaused;
    [SerializeField] GameObject menuOverlay;
    [SerializeField] bool isMenu;

    void Start()
    {
        menuOverlay.SetActive(false);
        gamePaused = false;

        Time.timeScale = 1;
    }
    void Update()
    {
        if (!isMenu)
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

    public void LoadScene()
    {
        SceneManager.LoadScene("Task1");
    }
}
