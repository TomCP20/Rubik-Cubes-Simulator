using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using UnityEngine;

public class Thistlethwaite : CubeSolver
{
    private SimpleCube simplecube;
    public static string[] movesG0 = { "U", "U2", "U'", "R", "R2", "R'", "F", "F2", "F'", "D", "D2", "D'", "L", "L2", "L'", "B", "B2", "B'" };
    public static string[] movesG1 = { "U", "U2", "U'", "R2", "F", "F2", "F'", "D", "D2", "D'", "L2", "B", "B2", "B'" };
    public static string[] movesG2 = { "U", "U2", "U'", "R2", "F2", "D", "D2", "D'", "L2", "B2" };
    public static string[] movesG3 = { "U2", "R2", "F2", "D2", "L2", "B2" };

    public static string[][] movesGN = {movesG0, movesG1, movesG2, movesG3};

    private Stack<SimpleCube> path;

    private bool[] solved = {false, false, false, false};

    public Thistlethwaite(Cube cube)
    {
        simplecube = new SimpleCube(cube);
        this.cube = cube.Clone();
        moves = new Queue<Move>();
    }
    public override IEnumerator solve()
    {
        foreach (string move in movesG0)
        {
            if (!checkMove(move))
            {
                Debug.Log("bad move " + move);
            }
        }
        path = new Stack<SimpleCube>();
        path.Push(simplecube);
        yield return IDDFS(1);
        Debug.Log("finnished G1");
        yield return IDDFS(2);
        simplecube = path.Peek();
        if (simplecube.isG2())
        {
            Debug.Log("Succsess");
        }
        else
        {
            Debug.Log("loss");
        }
        cube.applySimpleRep(simplecube);
        yield return null;
    }

    private IEnumerator IDDFS(int goal)
    {
        for (int i = 0; i < 20; i++)
        {
            Debug.Log("goal " + goal + " depth " + i);
            yield return DFS(i, goal);
            if (solved[goal-1]) { break; }
            yield return null;
        }
    }

    private IEnumerator DFS(int depth, int goal)
    {
        SimpleCube node = path.Peek();
        if (node.isGN(goal))
        {
            solved[goal-1] = true;
        }
        else if (depth > 0)
        {
            int branch = 0;
            foreach (SimpleCube nextState in getNextState(path, goal))
            {
                branch++;
                yield return null;
                path.Push(nextState);
                yield return DFS(depth-1, goal);
                if (solved[goal-1]) { break; }
                path.Pop();
            }
            //Debug.Log("branch " + branch);
        }
        yield return null;
    }

    private IEnumerable<SimpleCube> getNextState(Stack<SimpleCube> path, int goal)
    {
        SimpleCube node = path.Peek();
        foreach (string m in movesGN[goal-1])
        {
            if(!node.sameFace(m) && !node.opositePrime(m))
            {
                SimpleCube next = node.Clone();
                next.rotate(m);
                if (!next.inPath(path))
                {
                    yield return next;
                }
            }
            else
            {
                //Debug.Log(m + " " + node.move);
            }
        }
    }

    private bool checkMove(string move)
    {
        Cube c = new Cube();
        c.randomMoveSequence();
        SimpleCube s = new SimpleCube(c);
        c.rotate(move);
        s.rotate(move);
        return s.Equals(new SimpleCube(c));
    }
}
