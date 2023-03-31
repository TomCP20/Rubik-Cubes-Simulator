using System;
using UnityEngine;

[RequireComponent(typeof(CubeComponent))]
public class CubeController : MonoBehaviour
{
    public CubeComponent cube;

    void Start()
    {
        cube = GetComponent<CubeComponent>();
    }

    public void userMove(int direction)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out hit)) { return; }
        String move = getFace(hit.transform.position);
        if (move == "") { return; }
        if (direction == -1) { move += "'"; }
        cube.rotateCube(new Move(move));
    }

    public string getFace(Vector3 pos) 
    {
        if (pos.y >= 1.4)
        {
            return "U";
        }
        else if (pos.y <= -1.4)
        {
            return "D";
        }
        else if (pos.x >= 1.4)
        {
            return "L";
        }
        else if (pos.x <= -1.4)
        {
            return "R";
        }
        else if (pos.z >= 1.4)
        {
            return "F";
        }
        else if (pos.z <= -1.4)
        {
            return "B";
        }
        else
        {
            return "";
        }
    }
}
