using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cubes;
using Moves;
using CubeSolvers;
using CFOPs;
using UnityEngine.Events;


public class CubeComponent : MonoBehaviour
{
    private Cube c;

    public UnityEvent OnCubeChange;

    private void Start()
    {
        resetCube();
    }

    public void resetCube()
    {
        c = new Cube();
        OnCubeChange.Invoke();
    }

    public Cube getCube()
    {
        return c.Clone();
    }

    public void setCube(Cube cube)
    {
        c = cube;
        OnCubeChange.Invoke();
    }

    public void rotateCube(Move m)
    {
        c.rotate(m);
        OnCubeChange.Invoke();
    }

    public void solveCube()
    {
        CubeSolver solver = new CFOP(c);
        c = solver.getSlovedCube();
        OnCubeChange.Invoke();
    }

    public IEnumerator animate()
    {
        CubeSolver solver = new CFOP(c);
        Queue<Move> moves = solver.getSolution();
        while (moves.Count > 0)
        {
            UnityEngine.Debug.Log(moves.Count);
            rotateCube(moves.Dequeue());
            yield return new WaitForSeconds(1);
        }
    }

    public void startAnimate()
    {
        StartCoroutine(animate());
    }

    public void scramble()
    {
        c.randomMoveSequence();
        OnCubeChange.Invoke();
    }
}
