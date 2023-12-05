using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFaceletController : MonoBehaviour
{
    public Material[] cols;
    private int length;

    [SerializeField] private int currentColour = 0;
    [SerializeField] private bool modifiable = true;

    private Renderer r;
    void Start()
    {
        length = cols.Length;
        r = GetComponent<Renderer>();
        r.material = cols[currentColour];
    }

    public void changeColour(int n)
    {
        if (modifiable)
        {
            currentColour +=n;
            if (currentColour >= length)
            {
                currentColour -= length;
            }
            else if (currentColour < 0)
            {
                currentColour += length;
            }
            r.material = cols[currentColour];
        }
    }

    public string getColourString()
    {
        switch (currentColour)
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
