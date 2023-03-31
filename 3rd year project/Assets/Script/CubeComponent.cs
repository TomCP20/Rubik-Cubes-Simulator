using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CubeComponent : MonoBehaviour
{
    private Cube c;

    //public UnityEvent OnCubeChange;

    public CubeUpdater updater;

    private void Start()
    {
        updater = GetComponent<CubeUpdater>();
        createCube();
    }

    public void createCube()
    {
        c = new Cube();
        updater.spawnCube();
        updater.colourCube();
    }

    public void resetCube()
    {
        c = new Cube();
        updater.colourCube();
    }

    public Cube getCube()
    {
        return c.Clone();
    }

    public void setCube(Cube cube)
    {
        c = cube;
        updater.colourCube();
    }

    public void rotateCube(Move m)
    {
        c.rotate(m);
        StartCoroutine(updater.animateMove(m));
    }

    public void solveCube()
    {
        CubeSolver solver = new CFOP(c);
        c = solver.getSlovedCube();
        updater.colourCube();
    }

    public IEnumerator animate()
    {
        CubeSolver solver = new CFOP(c);
        Queue<Move> moves = solver.getSolution();
        while (moves.Count > 0)
        {
            UnityEngine.Debug.Log(moves.Count);
            Move m = moves.Dequeue();
            c.rotate(m);
            yield return StartCoroutine(updater.animateMove(m));
        }
    }

    public void startAnimate()
    {
        StartCoroutine(animate());
    }

    public void scramble()
    {
        c.randomMoveSequence();
        updater.colourCube();
        //OnCubeChange.Invoke();
    }
}
