using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string firstLevel;

    public string analysis;

    public void StartGame()
    {
        SceneManager.LoadScene(firstLevel);
    }

    public void loadAnalysis()
    {
        SceneManager.LoadScene(analysis);
    }
}
