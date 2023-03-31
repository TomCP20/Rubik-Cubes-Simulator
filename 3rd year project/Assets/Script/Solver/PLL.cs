using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using ExtensionMethods;


class PLL : CubeSolver
{
    public PLL(Cube cube)
    {
        this.cube = cube.Clone();
        moves = new Queue<Move>();
    }
    public override void solve()
    {
        permuteCorners();
        permuteEdges();
    }
    public void permuteCorners()
    {
        List<Piece> yellowCorners = cube.filter(Colour.Yellow, 3);
        for (int i = 0; i < 4; i++)
        {
            if (yellowSolvedCount(yellowCorners) < 2)
            {
                rotate("U");
            }
            else
            {
                break;
            }
        }
        List<Piece> notSolved = notSolvedPieces(yellowCorners);
        if (notSolved.Count == 2)
        {
            Vector3 relation = relativePositionOfPieces(notSolved[0], notSolved[1]);
            if (relation == new Vector3(2, 0, -2))
            {
                int shiftVal = getShiftVal(notSolved[0].position);
                rotateSequence(shiftVal, "F R U' R' U' R U R' F' R U R' U' R' F R F'");
            }
            else if (relation == new Vector3(0, 0, -2))
            {
                int shiftVal = getShiftVal(notSolved[0].position);
                rotateSequence(shiftVal, "R U R' U' R' F R2 U' R' U' R U R' F'");
            }
            else
            {
                int shiftVal = getShiftVal(notSolved[1].position);
                rotateSequence(shiftVal, "R U R' U' R' F R2 U' R' U' R U R' F'");
            }
        }
    }
    public void permuteEdges()
    {
        List<Piece> yellowEdges = cube.filter(Colour.Yellow, 2);
        int solvedCount = yellowSolvedCount(yellowEdges);
        if (solvedCount == 0)
        {
            if (notYellowFace(yellowEdges[0]).defaultDirection() == -notYellowFace(yellowEdges[0]).direction)
            {
                rotateSequence("L2 R2 D L2 R2 U2 L2 R2 D L2 R2");
            }
            else
            {
                int shiftValue = getZShiftValue(yellowEdges);
                rotateSequence(shiftValue, "L R' F L2 R2 B L2 R2 F L R' D2 L2 R2 U'");
            }
        }
        else if (solvedCount == 1)
        {
            Piece middleEdge = getMiddleEdge(yellowEdges);
            int shiftValue = getShiftVal(middleEdge.position);
            if (Quaternion.Euler(0, 90*getShiftVal(middleEdge.position), 0) * (middleEdge.SolvedPosition() - middleEdge.position) == new Vector3(-1, 0, -1))
            {
                rotateSequence(shiftValue, "R U' R U R U R U' R' U' R2");
            }
            else
            {
                rotateSequence(shiftValue, "R2 U R U R' U' R' U' R' U R'");
            }
        }
    }
    public int yellowSolvedCount(List<Piece> yellow)
    {
        int count = 0;
        foreach (Piece p in yellow)
        {
            if (p.correctPosition()) { count++; }
        }
        return count;
    }
    public List<Piece> notSolvedPieces(List<Piece> yellow)
    {
        List<Piece> output = new List<Piece>();
        foreach (Piece p in yellow)
        {
            if (!p.correctPosition()) { output.Add(p); }
        }
        return output;
    }
    public Face notYellowFace(Piece P)
    {
        foreach (Face f in P.faces)
        {
            if (f.colour != Colour.Yellow) { return f; }
        }
        Debug.LogError("Couldn't find non yellow face");
        return null;
    }
    public int getZShiftValue(List<Piece> yellowEdges)
    {
        foreach (Piece p1 in yellowEdges)
        {
            foreach (Piece p2 in yellowEdges)
            {
                if (relativePositionOfPieces(p1, p2) == new Vector3(1, 0, -1) && p1.SolvedPosition() == p2.position)
                {
                    return getShiftVal(p1.position);
                }
            }
        }
        Debug.LogError("couldn't get z shift value");
        return 0;
    }
    public Piece getMiddleEdge(List<Piece> yellowEdges)
    {
        foreach (Piece p in yellowEdges)
        {
            if (p.correctPosition())
            {
                return cube.getPiece(new Vector3(-p.position.x, p.position.y, -p.position.z));
            }
        }
        Debug.LogError("couldn't get middle edge");
        return null;
    }
}