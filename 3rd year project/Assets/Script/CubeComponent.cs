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
        StartCoroutine(solving());
    }

    public IEnumerator solving()
    {
        CubeSolver solver = new Thistlethwaite(c);
        yield return solver.solve();
        c = solver.getSlovedCube();
        updater.colourCube();
        yield return null;
    }

    public IEnumerator animate()
    {
        CubeSolver solver = new CFOP(c);
        yield return solver.solve();
        Queue<Move> moves = solver.getSolution();
        int i = 0;
        while (moves.Count > 0)
        {
            if (solver.sections.ContainsKey(i))
            {
                Debug.Log(solver.sections[i]);
            }
            UnityEngine.Debug.Log(moves.Count);
            Move m = moves.Dequeue();
            c.rotate(m);
            yield return StartCoroutine(updater.animateMove(m));
            i++;
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
