using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
A simple program that checks when the user presses the tab key and saves a screenshot.
The Screenshot is saved with a name in the format of Screenshot-yyyy-MM-dd-HH-mm-ss.png.
*/
namespace Screenshot
{
    public class Screenshot : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                string date = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
                ScreenCapture.CaptureScreenshot(string.Format("Screenshot-{0}.png", date));
            }
        }
    }
}
