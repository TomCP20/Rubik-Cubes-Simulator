using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
A simple script that loads the main menu whenever the main menu butto is pressed.
*/

namespace Menu
{
    public class LoadMenu : MonoBehaviour
    {
        [SerializeField]        
        private string MainMenu;

        public void loadMenu()
        {
            SceneManager.LoadScene(MainMenu);
        }
    }
}

