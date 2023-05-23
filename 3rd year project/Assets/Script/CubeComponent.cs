using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CubeComponent : MonoBehaviour
{
    private Cube c;

    public bool modifiable = true;


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
        setCube(new Cube());
    }

    public Cube getCube()
    {
        return c.Clone();
    }

    public void setCube(Cube cube)
    {
        if (modifiable)
        {
            c = cube;
            updater.colourCube();
        }
    }

    public void rotateCube(Move m)
    {
        if (modifiable)
        {
            modifiable = false;
            c.rotate(m);
            StartCoroutine(updater.animateMove(m, true));
        }
    }

    public IEnumerator animate()
    {
        modifiable = false;
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
            yield return StartCoroutine(updater.animateMove(m, false));
            i++;
        }
        modifiable = true;
    }

    public void startAnimate()
    {
        if (modifiable)
        {
            StartCoroutine(animate());
        }
        
    }

    public void scramble()
    {
        c.randomMoveSequence();
        updater.colourCube();
    }

    public void saveCube(int saveID)
    {
        SaveSystem.SaveCube(c, saveID);
    }

    public void LoadCube(int saveID)
    {
        setCube(SaveSystem.LoadCube(saveID));
    }
}
