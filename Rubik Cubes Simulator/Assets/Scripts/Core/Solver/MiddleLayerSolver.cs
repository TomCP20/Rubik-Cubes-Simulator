using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using ExtensionMethods;

class MiddleLayerSolver : CubeSolver
{
    public MiddleLayerSolver(Cube cube)
    {
        this.cube = cube.Clone();
        moves = new Queue<Move>();
    }
    public override IEnumerator solve()
    {
        middleLayer();
        yield return null;
    }
    private void middleLayer()
    {
        List<Piece> middleEdges = getMiddleEdges();
        foreach(Piece middleEdge in middleEdges)
        {
            if (middleEdge.position.y == 0)
            {
                middleEdgeMiddle(middleEdge);
            }
            else
            {
                middleEdgeTop(middleEdge);
            }
            if (!middleEdge.correctOrientation())
            {
                rightAlgorithm(middleEdge);
                rotate("U2");
                rightAlgorithm(middleEdge);
            }
        }
    }
    private void middleEdgeMiddle(Piece middleEdge)
    {
        if (!middleEdge.correctOrientation())
        {
            rightAlgorithm(middleEdge);
            middleEdgeTop(middleEdge);
        }
    }
    private void middleEdgeTop(Piece middleEdge)
    {
        rotateToCorrectPosition(middleEdge, getMiddleEdgeSide(middleEdge));
        if (middleEdge.position.x * middleEdge.SolvedPosition().z + middleEdge.position.z * middleEdge.SolvedPosition().x == 1)
        {
            rightAlgorithm(middleEdge);
        }
        else
        {
            leftAlgorithm(middleEdge);
        }
    }
    private Vector3 getMiddleEdgeSide(Piece middleEdge)
    {
        return middleEdge.faces[0].defaultDirection();
    }
    private void rightAlgorithm(Piece middleEdge)
    {
        rotateSequence(getShiftVal(middleEdge.position), new string[] {"U", "R", "U'", "R'", "U'", "F'", "U", "F"});
    }
    private void leftAlgorithm(Piece middleEdge)
    {
        rotateSequence(getShiftVal(middleEdge.position), new string[] {"U'", "L'", "U", "L", "U", "F", "U'", "F'"});
    }
    private List<Piece> getMiddleEdges()
    {
        List<Piece> output = new List<Piece>();
        foreach (Piece p in cube.pieces)
        {
            if (p.position.ManhattanDistance() == 2 && p.SolvedPosition().y == 0)
            {
                output.Add(p);
            }
        }
        return(output);
    }
}
