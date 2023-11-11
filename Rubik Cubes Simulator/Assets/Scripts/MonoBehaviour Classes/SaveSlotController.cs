using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveSlotController : MonoBehaviour
{

    public int saveID;

    public TMP_Text empty;
    public TMP_Text full;

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
