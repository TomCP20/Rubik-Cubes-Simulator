using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
A simple script that handles the buttons in the main menu.
StartGame, loadInput, and loadBarchart load their respective scenes.
quitGame shuts down the program
*/

namespace Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private string interactive;

        [SerializeField]
        private string analysis;

        [SerializeField]
        private string input;

        [SerializeField]
        private string barchart;

        public void StartGame()
        {
            SceneManager.LoadScene(interactive);
        }

        public void loadInput()
        {
            SceneManager.LoadScene(input);
        }

        public void loadBarchart()
        {
            SceneManager.LoadScene(barchart);
        }

        public void quitGame()
        {
            Application.Quit();
        }
    }

}
