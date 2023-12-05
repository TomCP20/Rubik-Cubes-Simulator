using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using ExtensionMethods;


class YellowEdgesSolver : CubeSolver
{
    public YellowEdgesSolver(Cube cube)
    {
        this.cube = cube.Clone();
        moves = new Queue<Move>();
    }
    public override IEnumerator solve()
    {
        yellowEdges();
        yield return null;
    }
    private void yellowEdges()
    {
        List<Piece> yellowEdges = cube.filter(Colour.Yellow, 2);
        rotate(Axis.Y, 1, topLayerRotation(yellowEdges));
        foreach (Piece yellowEdge in yellowEdges)
        {
            if (!yellowEdge.correctPosition())
            {
                if (yellowEdge.position.z == 0 && yellowEdge.SolvedPosition().z == 0)
                {
                    rotate("U");
                    switchEdges(0);
                    switchEdges(2);
                }
                else if (yellowEdge.position.x == 0 && yellowEdge.SolvedPosition().x == 0)
                {
                    rotate("U");
                    switchEdges(1);
                    switchEdges(3);
                }
                else if (Vector3Int.RoundToInt(Quaternion.Euler(0, 90, 0) * yellowEdge.position) == yellowEdge.SolvedPosition())
                {
                    int shiftVal = getShiftVal(yellowEdge.position);
                    switchEdges(shiftVal);
                }
                else if (Vector3Int.RoundToInt(Quaternion.Euler(0, -90, 0) * yellowEdge.position) == yellowEdge.SolvedPosition())
                {
                    int shiftVal = getShiftVal(yellowEdge.SolvedPosition());
                    switchEdges(shiftVal);
                }  
            }
        }
    }
    private int topLayerRotation(List<Piece> yellowEdges)
    {
        for (int angle = 0; angle < 4; angle++)
        {
            int score = topLayerScore(yellowEdges, angle);
            if (score >= 2)
            {
                return angle;
            }
        }
        UnityEngine.Debug.LogError("top layer error");
        return 0;
    }
    private int topLayerScore(List<Piece> yellowEdges, int angle)
    {
        int score = 0;
        foreach (Piece yellowEdge in yellowEdges)
        {
            if (yellowEdge.SolvedPosition() == Vector3Int.RoundToInt(Quaternion.Euler(0, 90*angle, 0) * yellowEdge.position))
            {
                score++;
            }
        }
        return score;
    }
    private void switchEdges(int shiftVal)
    {
        rotateSequence(shiftVal, new string[] {"R", "U", "R'", "U", "R", "U2", "R'", "U"});
    }
}


