using System;
using UnityEngine;
using LayerByLayers;
using CubeSolvers;
using Faces;
using Pieces;
using System.Collections.Specialized;
using System.Collections;
using Cubes;
using Moves;
using System.Collections.Generic;
using MultiCubeAnalysers;
using UnityEngine.Events;

public class CubeController : MonoBehaviour
{
    public CubeVariable CubeState;

    public UnityEvent OnCubeChange;

    // Start is called before the first frame update
    void Start()
    {

        CubeState.CubeValue = new Cube();
        OnCubeChange.Invoke();
        //InvokeRepeating("RotCube", 1.0f, 1.0f);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            CubeState.CubeValue.randomMoveSequence();
            OnCubeChange.Invoke();
        }
        if (Input.GetKeyDown("l"))
        {
            StartCoroutine(showSolution());
        }
        if (Input.GetKeyDown("p"))
        {
            StartCoroutine(animate());
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) { rotateFace(hit.transform.name, 1); }
        }
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) { rotateFace(hit.transform.name, -1); }
            
        }
        if (Input.GetKeyDown("o"))
        {
            StartCoroutine(Analysis());
        }
    }

    void rotateFace(String name, int direction)
    {
        String move;
        switch(name)
        {
            case ("Quad 23"):
                move = "U";
                break;
            case ("Quad 32"):
                move = "D";
                break;
            case ("Quad 5"):
                move = "L";
                break;
            case ("Quad 14"):
                move = "R";
                break;
            case ("Quad 41"):
                move = "F";
                break;
            case ("Quad 50"):
                move = "B";
                break;
            default:
                return;
        }
        if (direction == -1) { move += "'"; }
        CubeState.CubeValue.rotate(move);
        UnityEngine.Debug.Log(move);
        OnCubeChange.Invoke();
    }

    IEnumerator showSolution()
    {
        CubeSolver solver = new LayerByLayer(CubeState.CubeValue);
        CubeState.CubeValue = solver.getSlovedCube();
        OnCubeChange.Invoke();
        yield return null;
    }

    IEnumerator animate()
    {
        CubeSolver solver = new LayerByLayer(CubeState.CubeValue);
        Queue<Move> moves = solver.getSolution();
        while (moves.Count > 0)
        {
            UnityEngine.Debug.Log(moves.Count);
            Move m = moves.Dequeue();
            CubeState.CubeValue.rotate(m);
            OnCubeChange.Invoke();
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator Analysis()
    {
        MultiCubeAnalyser m = new MultiCubeAnalyser(10);
        m.printAnalysis();
        yield return null;
    }
}
