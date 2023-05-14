using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFaceletController : MonoBehaviour
{
    public Material[] cols;
    private int length;

    [SerializeField] private int currentColor = 0;
    [SerializeField] private bool modifiable = true;

    private Renderer r;
    void Start()
    {
        length = cols.Length;
        r = GetComponent<Renderer>();
        r.material = cols[currentColor];
    }

    public void changeColour(int n)
    {
        if (modifiable)
        {
            currentColor +=n;
            if (currentColor >= length)
            {
                currentColor -= length;
            }
            else if (currentColor < 0)
            {
                currentColor += length;
            }
            r.material = cols[currentColor];
        }
    }

    public string getColourString()
    {
        switch (currentColor)
        {
            case 0:
                return "W";
            case 1:
                return "G";
            case 2:
                return "B";
            case 3:
                return "R";
            case 4:
                return "Y";
            case 5:
                return "O";
            default:
                Debug.LogError("Invalid current Colour");
                return "";
        }
    }
}
