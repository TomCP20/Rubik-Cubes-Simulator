using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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

    private Stack<SimpleCube>[] paths = new Stack<SimpleCube>[4];

    private bool[] solved = {false, false, false, false};

    public Thistlethwaite(Cube cube)
    {
        simplecube = new SimpleCube(cube);
        this.cube = cube.Clone();
        moves = new Queue<Move>();
        paths[0] = new Stack<SimpleCube>();
        paths[1] = new Stack<SimpleCube>();
        paths[2] = new Stack<SimpleCube>();
        paths[3] = new Stack<SimpleCube>();
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
        paths[0].Push(simplecube);
        yield return IDDFS(1);
        Debug.Log("finnished G0 -> G1");
        Debug.Log(G0index(paths[0].Peek()).ToString());
        paths[1].Push(paths[0].Peek());
        yield return IDDFS(2);
        simplecube = paths[1].Peek();
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
        SimpleCube node = paths[goal-1].Peek();
        if (depth == 0)
        {
            if (node.isGN(goal))
            {
                solved[goal-1] = true;
                Debug.Log("solved!");
            }
        }
        else if (depth > 0)
        {
            int branch = 0;
            foreach (SimpleCube nextState in getNextState(goal))
            {
                branch++;
                yield return null;
                paths[goal-1].Push(nextState);
                yield return DFS(depth-1, goal);
                if (solved[goal-1]) { break; }
                paths[goal-1].Pop();
            }
            //Debug.Log("branch " + branch);
        }
        yield return null;
    }

    private IEnumerable<SimpleCube> getNextState(int goal)
    {
        SimpleCube node = paths[goal-1].Peek();
        foreach (string m in movesGN[goal-1])
        {
            if(!node.sameFace(m) && !node.opositePrime(m))
            {
                SimpleCube next = node.Clone();
                next.rotate(m);
                if (!inPath(next, goal))
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

    public bool inPath(SimpleCube c, int goal)
    {
        foreach (SimpleCube node in paths[goal-1])
        {
            if (isEqual(c, node, goal))
            {
                return true;
            }
        }
        return false;
    }

    public bool isEqual(SimpleCube c1, SimpleCube c2, int goal)
    {
        if (goal == 1)
        {
            return G0index(c1).Equals(G0index(c2));
        }
        else if (goal == 2)
        {
            return G1index(c1).Equals(G1index(c2));
        }
        else
        {
            return c1.Equals(c2);
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

    private BitVector32 G0index(SimpleCube c)
    {
        BitVector32 index = new BitVector32();
        int[] masks = new int[12];
        {
            masks[0] = BitVector32.CreateMask();
        }
        for (int i = 1; i < 12; i++)
        {
            masks[i] = BitVector32.CreateMask(masks[i - 1]);
        }
        index[masks[0]] = !c.isOneOfSideColours("U", 2, "FB") && !c.isOneOfSideColours("B", 2, "UD");
        index[masks[1]] = !c.isOneOfSideColours("U", 4, "FB") && !c.isOneOfSideColours("R", 2, "UD");
        index[masks[2]] = !c.isOneOfSideColours("U", 6, "FB") && !c.isOneOfSideColours("F", 2, "UD");
        index[masks[3]] = !c.isOneOfSideColours("U", 8, "FB") && !c.isOneOfSideColours("L", 2, "UD");

        index[masks[4]] = !c.isOneOfSideColours("D", 2, "FB") && !c.isOneOfSideColours("B", 8, "UD");
        index[masks[5]] = !c.isOneOfSideColours("D", 4, "FB") && !c.isOneOfSideColours("R", 8, "UD");
        index[masks[6]] = !c.isOneOfSideColours("D", 6, "FB") && !c.isOneOfSideColours("F", 8, "UD");
        index[masks[7]] = !c.isOneOfSideColours("D", 8, "FB") && !c.isOneOfSideColours("L", 8, "UD");

        index[masks[8]] = !c.isOneOfSideColours("R", 4, "FB") && !c.isOneOfSideColours("F", 6, "UD");
        index[masks[9]] = !c.isOneOfSideColours("R", 6, "FB") && !c.isOneOfSideColours("B", 4, "UD");
        index[masks[10]] = !c.isOneOfSideColours("L", 4, "FB") && !c.isOneOfSideColours("B", 6, "UD");
        index[masks[11]] = !c.isOneOfSideColours("L", 6, "FB") && !c.isOneOfSideColours("F", 4, "UD");
        
        return index;
    }

    private BitVector32 G1index(SimpleCube c)
    {
        BitVector32 index = new BitVector32();
        int[] masks = new int[28];
        {
            masks[0] = BitVector32.CreateMask();
        }
        for (int i = 1; i < 28; i++)
        {
            masks[i] = BitVector32.CreateMask(masks[i - 1]);
        }

        if (c.isOneOfSideColours("F", 1, "RL"))
        {
            index[masks[0]] = true;
        }
        else if (c.isOneOfSideColours("U", 7, "RL"))
        {
            index[masks[1]] = true;
        }

        if (c.isOneOfSideColours("F", 3, "RL"))
        {
            index[masks[2]] = true;
        }
        else if (c.isOneOfSideColours("U", 9, "RL"))
        {
            index[masks[3]] = true;
        }

        if (c.isOneOfSideColours("F", 7, "RL"))
        {
            index[masks[4]] = true;
        }
        else if (c.isOneOfSideColours("D", 1, "RL"))
        {
            index[masks[5]] = true;
        }

        if (c.isOneOfSideColours("F", 9, "RL"))
        {
            index[masks[6]] = true;
        }
        else if (c.isOneOfSideColours("D", 3, "RL"))
        {
            index[masks[7]] = true;
        }

        if (c.isOneOfSideColours("B", 1, "RL"))
        {
            index[masks[8]] = true;
        }
        else if (c.isOneOfSideColours("U", 3, "RL"))
        {
            index[masks[9]] = true;
        }

        if (c.isOneOfSideColours("B", 3, "RL"))
        {
            index[masks[10]] = true;
        }
        else if (c.isOneOfSideColours("U", 1, "RL"))
        {
            index[masks[11]] = true;
        }

        if (c.isOneOfSideColours("B", 7, "RL"))
        {
            index[masks[12]] = true;
        }
        else if (c.isOneOfSideColours("D", 9, "RL"))
        {
            index[masks[13]] = true;
        }

        if (c.isOneOfSideColours("B", 9, "RL"))
        {
            index[masks[14]] = true;
        }
        else if (c.isOneOfSideColours("D", 7, "RL"))
        {
            index[masks[15]] = true;
        }


        //optimise later

        index[masks[16]] = !c.isOneOfSideColours("U", 2, "FB") && !c.isOneOfSideColours("B", 2, "UD");
        index[masks[17]] = !c.isOneOfSideColours("U", 4, "FB") && !c.isOneOfSideColours("R", 2, "UD");
        index[masks[18]] = !c.isOneOfSideColours("U", 6, "FB") && !c.isOneOfSideColours("F", 2, "UD");
        index[masks[19]] = !c.isOneOfSideColours("U", 8, "FB") && !c.isOneOfSideColours("L", 2, "UD");

        index[masks[20]] = !c.isOneOfSideColours("D", 2, "FB") && !c.isOneOfSideColours("B", 8, "UD");
        index[masks[21]] = !c.isOneOfSideColours("D", 4, "FB") && !c.isOneOfSideColours("R", 8, "UD");
        index[masks[22]] = !c.isOneOfSideColours("D", 6, "FB") && !c.isOneOfSideColours("F", 8, "UD");
        index[masks[23]] = !c.isOneOfSideColours("D", 8, "FB") && !c.isOneOfSideColours("L", 8, "UD");

        index[masks[24]] = !c.isOneOfSideColours("R", 4, "FB") && !c.isOneOfSideColours("F", 6, "UD");
        index[masks[25]] = !c.isOneOfSideColours("R", 6, "FB") && !c.isOneOfSideColours("B", 4, "UD");
        index[masks[26]] = !c.isOneOfSideColours("L", 4, "FB") && !c.isOneOfSideColours("B", 6, "UD");
        index[masks[27]] = !c.isOneOfSideColours("L", 6, "FB") && !c.isOneOfSideColours("F", 4, "UD");

        return index;
    }
}
