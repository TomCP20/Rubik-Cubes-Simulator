using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;
using System.Collections.Specialized;
using ExtensionMethods;

public class Cube
{
    public Piece[] pieces;
    public Cube(Piece[] p)
    {
        pieces = p;
    }
    public Cube()
    {
        pieces = new Piece[26];
        Vector3[] positions = genPositions();
        for (int i = 0; i < 26; i++)
        {
            pieces[i] = new Piece(positions[i]);
        }
    }
    private Vector3[] genPositions()
    {
        Vector3[] positions = new Vector3[26];
        int index = 0;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                for (int k = -1; k <= 1; k++)
                {
                    Vector3 position = new Vector3(i, j, k);
                    if (position != Vector3.zero)
                    {
                        positions[index] = position;
                        index++;
                    }
                }
            }
        }
        return positions;
    }
    public Cube Clone()
    {
        Piece[] p = new Piece[pieces.Length];
        for (int i = 0; i < pieces.Length; i++) { p[i] = pieces[i].Clone(); }
        return new Cube(p);
    }
    public void rotate(Axis axis, int slice, int angle)
    {
        rotate( new Move(axis, slice, angle));
    }
    public void rotate(string move)
    {
        rotate(new Move(move));
    }
    public void rotate(Move move)
    {
        Quaternion rotQuaternion = move.getQuaternion();
        List<Piece> rotatePieces = getLayer(move.axis, move.slice);
        foreach (Piece rp in rotatePieces)
        {
            rp.rotate(rotQuaternion);
        }
    }
    public void randomMove()
    {
        string[] movesArray = { "F", "U", "R", "B", "D", "L", "F'", "U'", "R'", "B'", "D'", "L'", "F2", "U2", "R2", "B2", "D2", "L2" };
        System.Random rnd = new System.Random();
        rotate(new Move(movesArray[rnd.Next(movesArray.Length)]));
    }
    public void randomMoveSequence(int n = 100)
    {
        for (int i = 0; i < n; i++)
        {
            randomMove();
        }
    }
    public List<Piece> filter(Colour C, int MD)
    {
        List<Piece> output = new List<Piece>();
        foreach(Piece p in pieces)
        {
            if (p.position.ManhattanDistance() == MD && p.containsColour(C))
            {
                output.Add(p);
            }
        }
        return output;
    }
    public List<Piece> getLayer(Axis a, int layer)
    {
        List<Piece> output = new List<Piece>();
        foreach (Piece p in pieces)
        {
            if (p.position[(int) a] == layer)
            output.Add(p);
        }
        return output;
    }
    public Piece getPiece(Vector3 pos)
    {
        foreach(Piece p in pieces)
        {
            if (p.position == pos)
            {
                return p;
            }
        }
        UnityEngine.Debug.Log("get piece error");
        return null;
    }

    public bool isSolved()
    {
        foreach (Piece p in pieces)
        {
            if (!p.correctOrientation()) 
            {
                return false;
            }
        }
        return true;
    }

    public bool isDominoReduced()
    {
        foreach (Piece p in pieces)
        {
            foreach (Face f in p.faces)
            {
                if (p.position.y == 0) 
                {
                    if (!f.direction.EqualOrOposite(Vector3.up) && !f.direction.EqualOrOposite(f.defaultDirection()))
                    {
                        return false;
                    }
                }
                else
                {
                    if (f.direction.EqualOrOposite(Vector3.up) && !f.direction.EqualOrOposite(f.defaultDirection()))
                    {
                        return false;
                    }
                }
            } 
        }
        return true;
    }

    public Colour[,,] simpleRep()
    {
        Colour[,,] output = new Colour[6, 3, 3];
        foreach (Piece p in pieces)
        {
            foreach (Face f in p.faces)
            {
                Vector3 d = f.direction;
                int dx = (int)p.position.x;
                int dy = (int)p.position.y;
                int dz = (int)p.position.z;
                if (d == Vector3.up)
                {
                    output[0, 1+dz, 1-dx] = f.colour;
                }
                else if (d == Vector3.down)
                {
                    output[3, 1-dz, 1-dx] = f.colour;
                }
                else if (d == Vector3.left) // actualy right due to parity
                {
                    output[1, 1-dy, 1-dz] = f.colour;
                }
                else if (d == Vector3.right) // actually left due to parity
                {
                    output[4, 1-dy, 1+dz] = f.colour;
                }
                else if (d == Vector3.forward)
                {
                    output[2, 1-dy, 1-dx] = f.colour;
                }
                else if (d == Vector3.back)
                {
                    output[5, 1-dy, 1+dx] = f.colour;
                }
                else
                {
                    Debug.LogError("Invalid direction " + d);
                }
            }
        }
        return output;
    }

    public void applySimpleRep(SimpleCube c)
    {
        Colour[,,] cubestate = c.array;
        foreach (Piece p in pieces)
        {
            foreach (Face f in p.faces)
            {
                Vector3 d = f.direction;
                int dx = (int)p.position.x;
                int dy = (int)p.position.y;
                int dz = (int)p.position.z;
                if (d == Vector3.up)
                {
                    f.colour = cubestate[0, 1+dz, 1-dx];
                }
                else if (d == Vector3.down)
                {
                    f.colour = cubestate[3, 1-dz, 1-dx];
                }
                else if (d == Vector3.left) // actualy right due to parity
                {
                    f.colour = cubestate[1, 1-dy, 1-dz];
                }
                else if (d == Vector3.right) // actually left due to parity
                {
                    f.colour = cubestate[4, 1-dy, 1+dz];
                }
                else if (d == Vector3.forward)
                {
                    f.colour = cubestate[2, 1-dy, 1-dx];
                }
                else if (d == Vector3.back)
                {
                    f.colour = cubestate[5, 1-dy, 1+dx] ;
                }
                else
                {
                    Debug.LogError("Invalid direction " + d);
                }
            }
        }
    }
}

