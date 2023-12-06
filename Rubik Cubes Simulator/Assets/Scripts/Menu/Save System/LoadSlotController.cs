using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
Script for controlling the load slots in the load menu.
updateText checks if relevant the save file is there, if it is there then the save slot displays the full button and makes the button interacable,
otherwise the program shows the empty button instead and makes the button uninteracable.

*/

namespace Menu.Save
{
    public class LoadSlotController : MonoBehaviour
    {
        [SerializeField]
        private int saveID;

        [SerializeField]
        private TMP_Text empty;

        [SerializeField]
        private TMP_Text full;

        [SerializeField]
        private Button button;

        private string path;

        void Start()
        {
            path = Application.persistentDataPath + "/cube" + saveID + ".txt";
            full.text = "cube " + saveID;
            updateText();
        }

        void Update()
        {
            updateText();
        }

        public void updateText()
        {
            if (System.IO.File.Exists(path))
            {
                empty.gameObject.SetActive(false);
                full.gameObject.SetActive(true);
                button.interactable = true;
            }
            else
            {
                empty.gameObject.SetActive(true);
                full.gameObject.SetActive(false);
                button.interactable = false;
            }
        }
    }

}
