using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
Script that controlls the save slots for the save menu.
updateText checks if relevant the save file is there, if it is there then the save slot displays the full button, otherwise the program shows the empty button instead.
*/

namespace Menu.Save
{
    public class SaveSlotController : MonoBehaviour
    {
        [SerializeField]
        private int saveID;

        [SerializeField]
        private TMP_Text empty;

        [SerializeField]
        private TMP_Text full;

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
            }
            else
            {
                empty.gameObject.SetActive(true);
                full.gameObject.SetActive(false);
            }
        }
    }

}