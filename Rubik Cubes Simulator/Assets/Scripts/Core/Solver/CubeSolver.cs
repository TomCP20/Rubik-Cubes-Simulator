using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using ExtensionMethods;

public abstract class CubeSolver
{
    public Cube cube;
    public Queue<Move> moves = new Queue<Move>();
    public Dictionary<int, string> sections = new Dictionary<int, string>();

    protected void addSection(string name)
    {
        if (sections.ContainsKey(moves.Count))
        {
            sections[moves.Count] = name;
        }
        else
        {
            sections.Add(moves.Count, name);
        }
    }
    protected void rotate(Axis axis, int slice, int quarterTurns)
    {
        rotate(new Move(axis, slice, quarterTurns));
    }
    protected void rotate(string notation)
    {
        rotate(new Move(notation));
    }
    protected void rotate(Move move)
    {
        cube.rotate(move);
        moves.Enqueue(move);
    }
    public Queue<Move> getSolution()
    {
        return moves;
    }

    public Cube getSlovedCube() // for testing
    {
        return cube;
    }
    public abstract IEnumerator solve();

    private string shiftFace(int shiftVal, string face)
    {
        if (face == "U" || face == "D")
        {
            return face;
        }
        string[] sideFaces = {"F", "R", "B", "L"};
        int pos = Array.IndexOf(sideFaces, face);
        int newPos = (pos + shiftVal) % 4;
        string newFace = sideFaces[newPos].ToString();
        return newFace;
    }

    protected void shiftRotate(int shiftVal, string notation)
    {
        string face = shiftFace(shiftVal, notation.Substring(0, 1));
        string angle = notation.Substring(1);
        rotate(face + angle);
    }

    protected void rotateSequence(int shiftVal, string[] sequence)
    {
        foreach (string move in sequence)
        {
            shiftRotate(shiftVal, move);
        }
    }

    protected void rotateSequence(int shiftVal, string sequence)
    {
        rotateSequence(shiftVal, sequence.Split(" "));
    }

    protected void rotateSequence(string sequence)
    {
        rotateSequence(sequence.Split(" "));
    }

    protected void rotateSequence(string[] sequence)
    {
        foreach (string move in sequence)
        {
            rotate(move);
        }
    }

    protected int getShiftVal(Vector3 pos)
    {
        if (pos.z == 1 && pos.x != 1) { return 0; }
        else if (pos.x == -1) { return 1; }
        else if (pos.z == -1) { return 2; }
        else { return 3; }
    }

    protected IEnumerator subCubeSolver(CubeSolver solver)
    {
        yield return solver.solve();
        cube = solver.getSlovedCube();
        while (solver.moves.Count > 0)
        {
            moves.Enqueue(solver.moves.Dequeue());
        }
    }

    protected void rotateToCorrectPosition(Piece piece)
    {
        Vector3 targetPos = piece.SolvedPosition();
        rotateToCorrectPosition(piece, targetPos);
    }
    protected void rotateToCorrectPosition(Piece piece, Vector3 targetPos)
    {
        Vector3 currentPos = piece.position;
        Vector3 rot1 = Vector3Int.RoundToInt(Quaternion.Euler(0, 90, 0) * currentPos);
        Vector3 rot2 = Vector3Int.RoundToInt(Quaternion.Euler(0, 180, 0) * currentPos);
        Vector3 rot3 = Vector3Int.RoundToInt(Quaternion.Euler(0, 270, 0) * currentPos);
        if (correctXZ(rot1, targetPos))
        {
            rotate("U");
        }
        else if (correctXZ(rot2, targetPos))
        {
            rotate("U2");
        }
        else if (correctXZ(rot3, targetPos))
        {
            rotate("U'");
        }
    }
    protected bool correctXZ(Vector3 current, Vector3 target)
    {
        return ((int)current.x == (int)target.x && (int)current.z == (int)target.z);
    }
    protected Vector3 faceletRelativeDirection(Piece p, Colour c)
    {
        Face f = p.getFaceByColour(c);
        return Quaternion.Euler(0, 90*getShiftVal(p.position), 0) * f.direction;
    }
    protected Vector3 relativePositionOfPieces(Piece start, Piece end)
    {
        return Quaternion.Euler(0, 90*getShiftVal(start.position), 0) * (end.position - start.position);
    }
}  

