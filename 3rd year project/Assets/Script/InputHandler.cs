using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputHandler : MonoBehaviour
{
    public UnityEvent onScrambleCube;
    public UnityEvent onSolveCube;
    public UnityEvent onAnimateCube;
    public UnityEvent onDisplayAnalysis;
    public UnityEvent onMouseLeft;
    public UnityEvent onMouseRight;

    public GameObject target;

    public float rotationSpeed = 7.0F;

    void Update()
    {
        float Xrot = Input.GetAxis("Horizontal");
        float Yrot = Mathf.Clamp(Input.GetAxis("Vertical"), -90, 90);
        transform.LookAt(target.transform);
        transform.Translate(new Vector3(Xrot, Yrot, 0) * Time.deltaTime * rotationSpeed);

        if (Input.GetKeyDown("r"))
        {
            onScrambleCube.Invoke();
        }
        if (Input.GetKeyDown("l"))
        {
            onSolveCube.Invoke();
        }
        if (Input.GetKeyDown("p"))
        {
            onAnimateCube.Invoke();
        }
        if (Input.GetKeyDown("o"))
        {
            onDisplayAnalysis.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            string date1 = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            ScreenCapture.CaptureScreenshot(string.Format("Screenshots/Screenshot-{0}.png", date1));
        }
        if (Input.GetMouseButtonDown(0))
        {
            onMouseLeft.Invoke();
        }
        if (Input.GetMouseButtonDown(1))
        {
            onMouseRight.Invoke();
        }
    }
}
