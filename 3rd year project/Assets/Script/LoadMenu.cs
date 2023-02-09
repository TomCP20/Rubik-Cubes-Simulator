using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMenu : MonoBehaviour
{
    public string MainMenu;

    public void loadMenu()
    {
        SceneManager.LoadScene(MainMenu);
    }
}
