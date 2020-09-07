using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public PlayerController player;

    private void Start()
    {
        player.PauseGame();
    }

    public void StartGame()
    {
        player.ResumeGame();
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
