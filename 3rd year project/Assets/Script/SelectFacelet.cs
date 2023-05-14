using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectFacelet : MonoBehaviour
{
    [SerializeField] private Transform InputCube;
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            modifyFacelet(1);
        }
        if(Input.GetMouseButtonDown(1))
        {
            modifyFacelet(-1);
        }
    }

    private void modifyFacelet(int n)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            GameObject hitObject = hit.transform.gameObject;
            InputFaceletController facelet = hitObject.GetComponent<InputFaceletController>();
            if (facelet != null)
            {
                facelet.changeColour(n);
            }
        }
    }

    private string getStringRep()
    {
        string output = "";
        foreach (Transform child in InputCube)
        {
            foreach (Transform grandChild in child)
            {
                InputFaceletController facelet = grandChild.GetComponent<InputFaceletController>();
                if (facelet != null)
                {
                    output += facelet.getColourString();
                }
                else
                {
                    Debug.LogError("Coludn't find facelet.");
                }
            }
        }
        return output;
    }

    public void SaveCube()
    {
        SaveSystem.SaveCube(getStringRep());
    }
}
