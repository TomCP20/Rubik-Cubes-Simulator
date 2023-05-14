using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string interactive;

    public string analysis;

    public string input;

    public void StartGame()
    {
        SceneManager.LoadScene(interactive);
    }

    public void loadAnalysis()
    {
        SceneManager.LoadScene(analysis);
    }

    public void loadInput()
    {
        SceneManager.LoadScene(input);
    }
}
