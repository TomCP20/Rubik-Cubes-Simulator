using System;
using UnityEngine;
using LayerByLayers;
using CubeSolvers;
using System.Collections;
using Cubes;
using Moves;
using System.Collections.Generic;
using UnityEngine.Events;

public class CubeController : MonoBehaviour
{
    public CubeVariable CubeState;

    public UnityEvent OnCubeChange;

    void Start()
    {
        CubeState.CubeValue = new Cube();
        OnCubeChange.Invoke();
    }

    public void userMove(int direction)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out hit)) { return; }
        String move = getFace(hit.transform.position);
        if (move == "") { return; }
        if (direction == -1) { move += "'"; }
        roatateFace(new Move(move));
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
    

    public void showSolution()
    {
        CubeSolver solver = new LayerByLayer(CubeState.CubeValue);
        CubeState.CubeValue = solver.getSlovedCube();
        OnCubeChange.Invoke();
    }

    public void resetCube()
    {
        CubeState.CubeValue = new Cube();
        OnCubeChange.Invoke();
    }

    public void startAnimate()
    {
        StartCoroutine(animate());
    }

    public IEnumerator animate()
    {
        CubeSolver solver = new LayerByLayer(CubeState.CubeValue);
        Queue<Move> moves = solver.getSolution();
        while (moves.Count > 0)
        {
            UnityEngine.Debug.Log(moves.Count);
            roatateFace(moves.Dequeue());
            yield return new WaitForSeconds(1);
        }
    }

    public void scramble()
    {
        CubeState.CubeValue.randomMoveSequence();
        OnCubeChange.Invoke();
    }

    public void roatateFace(Move m)
    {
        CubeState.CubeValue.rotate(m);
        OnCubeChange.Invoke();
    }
}
