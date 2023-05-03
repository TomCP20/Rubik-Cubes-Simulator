using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCube
{
    public Colour[,,] array;

    public string move = " ";
    public SimpleCube(Cube c)
    {
        array = new Colour[6, 3, 3];
        foreach (Piece p in c.pieces)
        {
            foreach (Face f in p.faces)
            {
                Vector3 d = f.direction;
                int dx = (int)p.position.x;
                int dy = (int)p.position.y;
                int dz = (int)p.position.z;
                if (d == Vector3.up)
                {
                    array[0, 1+dz, 1-dx] = f.colour;
                }
                else if (d == Vector3.down)
                {
                    array[3, 1-dz, 1-dx] = f.colour;
                }
                else if (d == Vector3.left) // actualy right due to parity
                {
                    array[1, 1-dy, 1-dz] = f.colour;
                }
                else if (d == Vector3.right) // actually left due to parity
                {
                    array[4, 1-dy, 1+dz] = f.colour;
                }
                else if (d == Vector3.forward)
                {
                    array[2, 1-dy, 1-dx] = f.colour;
                }
                else if (d == Vector3.back)
                {
                    array[5, 1-dy, 1+dx] = f.colour;
                }
                else
                {
                    Debug.LogError("Invalid direction " + d);
                }
            }
        }
    }

    public SimpleCube(Colour[,,] a, string m)
    {
        array = a;
        move = m;
    }

    public SimpleCube Clone()
    {
        Colour[,,] a = new Colour[6,3,3];
        string m = move;
        for (int i = 0; i < array.GetLongLength(0); i++)
        {
            for (int j = 0; j < array.GetLongLength(1); j++)
            {
                for (int k = 0; k < array.GetLongLength(2); k++)
                {
                    a[i,j,k] = array[i,j,k];
                }
            }
        }
        return new SimpleCube(a, m);
    }

    public void rotate(string m)
    {
        move = m;
        int angle = 0;
        if (m.Length == 1) { angle = 1; }
        else if (m.Substring(1) == "'") { angle = 3; }
        else if (m.Substring(1) == "2") { angle = 2; }
        else
        {
            Debug.LogError("invalid move");
        }
        string face = m.Substring(0, 1);
        rotate(face, angle);
    }
    private void rotate(string face, int angle)
    {
        switch (angle)
        {
            case 1:
                rotate_face1(face);
                break;
            case 2:
                rotate_face2(face);
                break;
            case 3:
                rotate_face3(face);
                break;   
        }
    }

    private void rotate_face1(string face) //rotate side 90 degres
    {
        rotate_array1(getFaceInt(face));
        switch (face)
        {
            case "U":
                swap4(getCoords("F", 1), getCoords("L", 1), getCoords("B", 1), getCoords("R", 1));
                swap4(getCoords("F", 2), getCoords("L", 2), getCoords("B", 2), getCoords("R", 2));
                swap4(getCoords("F", 3), getCoords("L", 3), getCoords("B", 3), getCoords("R", 3));
                break;
            case "R":
                swap4(getCoords("F", 3), getCoords("U", 3), getCoords("B", 7), getCoords("D", 3));
                swap4(getCoords("F", 6), getCoords("U", 6), getCoords("B", 4), getCoords("D", 6));
                swap4(getCoords("F", 9), getCoords("U", 9), getCoords("B", 1), getCoords("D", 9));
                break;
            case "F":
                swap4(getCoords("R", 1), getCoords("D", 3), getCoords("L", 9), getCoords("U", 7));
                swap4(getCoords("R", 4), getCoords("D", 2), getCoords("L", 6), getCoords("U", 8));
                swap4(getCoords("R", 7), getCoords("D", 1), getCoords("L", 3), getCoords("U", 9));
                break;
            case "D":
                swap4(getCoords("F", 7), getCoords("R", 7), getCoords("B", 7), getCoords("L", 7));
                swap4(getCoords("F", 8), getCoords("R", 8), getCoords("B", 8), getCoords("L", 8));
                swap4(getCoords("F", 9), getCoords("R", 9), getCoords("B", 9), getCoords("L", 9));
                break;
            case "L":
                swap4(getCoords("F", 1), getCoords("D", 1), getCoords("B", 9), getCoords("U", 1));
                swap4(getCoords("F", 4), getCoords("D", 4), getCoords("B", 6), getCoords("U", 4));
                swap4(getCoords("F", 7), getCoords("D", 7), getCoords("B", 3), getCoords("U", 7));
                break;
            case "B":
                swap4(getCoords("R", 3), getCoords("U", 1), getCoords("L", 7), getCoords("D", 9));
                swap4(getCoords("R", 6), getCoords("U", 2), getCoords("L", 4), getCoords("D", 8));
                swap4(getCoords("R", 9), getCoords("U", 3), getCoords("L", 1), getCoords("D", 7));
                break;
            default:
                Debug.LogError("Invalid Face: " + face);
                break;
        }
    }

    private void rotate_face2(string face) //rotate side 180 degres
    {
        rotate_array2(getFaceInt(face));
        switch (face)
        {
            case "U":
                swap2(getCoords("F", 1), getCoords("B", 1));
                swap2(getCoords("F", 2), getCoords("B", 2));
                swap2(getCoords("F", 3), getCoords("B", 3));

                swap2(getCoords("L", 1), getCoords("R", 1));
                swap2(getCoords("L", 2), getCoords("R", 2));
                swap2(getCoords("L", 3), getCoords("R", 3));
                break;
            case "R":
                swap2(getCoords("F", 3), getCoords("B", 7));
                swap2(getCoords("F", 6), getCoords("B", 4));
                swap2(getCoords("F", 9), getCoords("B", 1));

                swap2(getCoords("U", 3), getCoords("D", 3));
                swap2(getCoords("U", 6), getCoords("D", 6));
                swap2(getCoords("U", 9), getCoords("D", 9));
                break;
            case "F":
                swap2(getCoords("R", 7), getCoords("L", 3));
                swap2(getCoords("R", 4), getCoords("L", 6));
                swap2(getCoords("R", 1), getCoords("L", 9));

                swap2(getCoords("U", 9), getCoords("D", 1));
                swap2(getCoords("U", 8), getCoords("D", 2));
                swap2(getCoords("U", 7), getCoords("D", 3));
                break;
            case "D":
                swap2(getCoords("F", 9), getCoords("B", 9));
                swap2(getCoords("F", 8), getCoords("B", 8));
                swap2(getCoords("F", 7), getCoords("B", 7));

                swap2(getCoords("L", 9), getCoords("R", 9));
                swap2(getCoords("L", 8), getCoords("R", 8));
                swap2(getCoords("L", 7), getCoords("R", 7));
                break;
            case "L":
                swap2(getCoords("F", 7), getCoords("B", 3));
                swap2(getCoords("F", 4), getCoords("B", 6));
                swap2(getCoords("F", 1), getCoords("B", 9));

                swap2(getCoords("U", 7), getCoords("D", 7));
                swap2(getCoords("U", 4), getCoords("D", 4));
                swap2(getCoords("U", 1), getCoords("D", 1));
                break;
            case "B":
                swap2(getCoords("R", 3), getCoords("L", 7));
                swap2(getCoords("R", 6), getCoords("L", 4));
                swap2(getCoords("R", 9), getCoords("L", 1));

                swap2(getCoords("U", 1), getCoords("D", 9));
                swap2(getCoords("U", 2), getCoords("D", 8));
                swap2(getCoords("U", 3), getCoords("D", 7));
                break;
            default:
                Debug.LogError("Invalid Face: " + face);
                break;
        }
    }

    private void rotate_face3(string face) //rotate side -90 degres
    {
        rotate_array3(getFaceInt(face));
        switch (face)
        {
            case "U":
                swap4(getCoords("R", 1), getCoords("B", 1), getCoords("L", 1), getCoords("F", 1));
                swap4(getCoords("R", 2), getCoords("B", 2), getCoords("L", 2), getCoords("F", 2));
                swap4(getCoords("R", 3), getCoords("B", 3), getCoords("L", 3), getCoords("F", 3));
                break;
            case "R":
                swap4(getCoords("D", 3), getCoords("B", 7), getCoords("U", 3), getCoords("F", 3));
                swap4(getCoords("D", 6), getCoords("B", 4), getCoords("U", 6), getCoords("F", 6));
                swap4(getCoords("D", 9), getCoords("B", 1), getCoords("U", 9), getCoords("F", 9));
                break;
            case "F":
                swap4(getCoords("U", 7), getCoords("L", 9), getCoords("D", 3), getCoords("R", 1));
                swap4(getCoords("U", 8), getCoords("L", 6), getCoords("D", 2), getCoords("R", 4));
                swap4(getCoords("U", 9), getCoords("L", 3), getCoords("D", 1), getCoords("R", 7));
                break;
            case "D":
                swap4(getCoords("L", 7), getCoords("B", 7), getCoords("R", 7), getCoords("F", 7));
                swap4(getCoords("L", 8), getCoords("B", 8), getCoords("R", 8), getCoords("F", 8));
                swap4(getCoords("L", 9), getCoords("B", 9), getCoords("R", 9), getCoords("F", 9));
                break;
            case "L":
                swap4(getCoords("U", 1), getCoords("B", 9), getCoords("D", 1), getCoords("F", 1));
                swap4(getCoords("U", 4), getCoords("B", 6), getCoords("D", 4), getCoords("F", 4));
                swap4(getCoords("U", 7), getCoords("B", 3), getCoords("D", 7), getCoords("F", 7));
                break;
            case "B":
                swap4(getCoords("D", 9), getCoords("L", 7), getCoords("U", 1), getCoords("R", 3));
                swap4(getCoords("D", 8), getCoords("L", 4), getCoords("U", 2), getCoords("R", 6));
                swap4(getCoords("D", 7), getCoords("L", 1), getCoords("U", 3), getCoords("R", 9));
                break;
            default:
                Debug.LogError("Invalid Face: " + face);
                break;
        }
    }

    private void rotate_array1(int face) // rotates array 90 degrees
    {
        Colour[,] newface = new Colour[3,3];
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                newface[col, 2-row] = array[face, row, col];
            }
        }
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                array[face, i, j] = newface[i,j];
            }
        }
    }

    private void rotate_array2(int face) // rotates array 180 degrees
    {
        Colour[,] newface = new Colour[3,3];
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                newface[row, 3-1-col] = array[face, 3-1-row, col];
            }
        }
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                array[face, i, j] = newface[i,j];
            }
        }
    }

    private void rotate_array3(int face) // rotates array -90 degrees
    {
        Colour[,] newface = new Colour[3,3];
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                newface[2-col, row] = array[face, row, col];
            }
        }
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                array[face, i, j] = newface[i,j];
            }
        }
    }

    private void swap4(int[] a, int[] b, int[] c, int[] d)
    {
        Colour temp = getFacelet(d);
        applyFacelet(c, d); // c -> d
        applyFacelet(b, c); // b -> c
        applyFacelet(a, b); // a -> b
        setFacelet(a, temp); // d -> a
    }

    private void swap2(int[] a, int[] b)
    {
        Colour temp = getFacelet(b);
        applyFacelet(a, b); // a -> b
        setFacelet(a, temp); // b -> a
    }

    public Colour getFacelet(int[] coords)
    {
        return array[coords[0], coords[1], coords[2]];
    }

    public Colour getFacelet(string face, int n)
    {
        return getFacelet(getCoords(face, n));
    }

    private void setFacelet(int[] coords, Colour c)
    {
        array[coords[0], coords[1], coords[2]] = c;
    }

    private void applyFacelet(int[] source, int[] goal)
    {
        setFacelet(goal, getFacelet(source));
    }

    private int getFaceInt(string face)
    {
        switch (face)
        {
            case "U":
                return 0;
            case "R":
                return 1;
            case "F":
                return 2;
            case "D":
                return 3;
            case "L":
                return 4;
            case "B":
                return 5;
        }
        Debug.Log("invalid face " + face);
        return 6;
    }
    private int[] getCoords(string face, int n)
    {
        int[] output = new int[3];
        output[0] = getFaceInt(face);
        output[1] = (n-1)/3;
        output[2] = (n-1)%3;
        return output;
    }
    public bool isGN(int goal)
    {
        switch (goal)
        {
            case 1:
                return isG1();
            case 2:
                return isG2();
            default:
                Debug.Log("invalid goal");
                return false;
        }
    }

    public bool isG1()
    {
        int[][] frontEdges = getEdges("F");
        int[][] backEdges = getEdges("B");

        foreach (int[] edge in frontEdges)
        {
            if (isOneOfSideColours(edge, "UD"))
            {
                return false;
            }
        }

        foreach (int[] edge in backEdges)
        {
            if (isOneOfSideColours(edge, "UD"))
            {
                return false;
            }
        }

        int[][] upEdges = getEdges("U");
        int[][] downEdges = getEdges("D");

        foreach (int[] edge in upEdges)
        {
            if (isOneOfSideColours(edge, "FB"))
            {
                return false;
            }
        }

        foreach (int[] edge in downEdges)
        {
            if (isOneOfSideColours(edge, "FB"))
            {
                return false;
            }
        }

        if (isOneOfSideColours("R", 2, "UD"))
        {
            return false;
        }
        if (isOneOfSideColours("R", 8, "UD"))
        {
            return false;
        }
        if (isOneOfSideColours("L", 2, "UD"))
        {
            return false;
        }
        if (isOneOfSideColours("L", 8, "UD"))
        {
            return false;
        }

        if (isOneOfSideColours("R", 4, "FB"))
        {
            return false;
        }
        if (isOneOfSideColours("R", 6, "FB"))
        {
            return false;
        }
        if (isOneOfSideColours("L", 4, "FB"))
        {
            return false;
        }
        if (isOneOfSideColours("L", 6, "FB"))
        {
            return false;
        }

        return true;
    }

    public bool isG2()
    {
        Colour[] rightFace = getFace("R");
        Colour[] leftFace = getFace("L");
        foreach (Colour face in rightFace)
        {
            if (!isOneOfSideColours(face, "RL"))
            {
                return false;
            }
        }
        foreach (Colour face in leftFace)
        {
            if (!isOneOfSideColours(face, "RL"))
            {
                return false;
            }
        }
        int[][] frontMiddle = getMiddleEdges("F");
        int[][] backMiddle = getMiddleEdges("B");
        int[][] upMiddle = getMiddleEdges("U");
        int[][] downMiddle = getMiddleEdges("D");
        foreach (int[] edge in frontMiddle)
        {
            if (!isOneOfSideColours(edge, "FB"))
            {
                return false;
            }
        }
        foreach (int[] edge in backMiddle)
        {
            if (!isOneOfSideColours(edge, "FB"))
            {
                return false;
            }
        }
        foreach (int[] edge in upMiddle)
        {
            if (!isOneOfSideColours(edge, "UD"))
            {
                return false;
            }
        }
        foreach (int[] edge in downMiddle)
        {
            if (!isOneOfSideColours(edge, "UD"))
            {
                return false;
            }
        }
        return true;
    }

    private int[][] getEdges(string face)
    {
        int[][] edges = new int[4][];
        for (int i = 0; i < edges.Length; i++)
        {
            edges[i] = getCoords(face, i*2+2);
        }
        return edges;
    }

    private int[][] getMiddleEdges(string face)
    {
        int[][] edges = new int[2][];
        edges[0] = getCoords(face, 2);
        edges[1] = getCoords(face, 8);
        return edges;
    }

    private Colour[] getFace(string face)
    {
        Colour[] output = new Colour[9];
        
        for (int i = 0; i < array.GetLongLength(1); i++)
        {
            for (int j = 0; j < array.GetLongLength(2); j++)
            {
                output[3*i+j] = array[getFaceInt(face), i, j];
            }
        }
        return output;
    }

    public override bool Equals(object obj)
    {
        if ((obj == null) || ! this.GetType().Equals(obj.GetType()))
        {
            Debug.LogError("Object null or wrong type");
            return false;
        }
        SimpleCube c = (SimpleCube) obj;
        for (int i = 0; i < array.GetLongLength(0); i++)
        {
            for (int j = 0; j < array.GetLongLength(1); j++)
            {
                for (int k = 0; k < array.GetLongLength(2); k++)
                {
                    if (c.array[i,j,k] != array[i,j,k])
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public bool sameFace(string m)
    {
        return m.Substring(0, 1) == move.Substring(0, 1);
    }

    public override string ToString()
    {
        string output = "";
        foreach (Colour facelet in array)
        {
            switch (facelet)
            {
                case Colour.White:
                    output += "W";
                    break;
                case Colour.Orange:
                    output += "O";
                    break;
                case Colour.Yellow:
                    output += "Y";
                    break;
                case Colour.Blue:
                    output += "B";
                    break;
                case Colour.Red:
                    output += "R";
                    break;
                case Colour.Green:
                    output += "G";
                    break;
                default:
                    Debug.LogError("invalid colour " + facelet);
                    break;
            }
        }
        return output;
    }

    public bool opositePrime(string m) // checks if a move is oposite the last move and the last move was a first face, U R F
    {
        if (move.Substring(0, 1) == "U" && m.Substring(0, 1) == "D")
        {
            return true;
        }
        if (move.Substring(0, 1) == "R" && m.Substring(0, 1) == "L")
        {
            return true;
        }
        if (move.Substring(0, 1) == "F" && m.Substring(0, 1) == "B")
        {
            return true;
        }
        return false;
    }

    public bool isOneOfSideColours(string face, int n, string sides)
    {
        return isOneOfSideColours(getCoords(face, n), sides);
    }
    public bool isOneOfSideColours(int[] coords, string sides)
    {
        return isOneOfSideColours(getFacelet(coords), sides);
    }

    public bool isOneOfSideColours(Colour col, string sides)
    {
        switch (sides)
        {
            case "RL":
                return col == Colour.Red || col == Colour.Orange;
            case "UD":
                return col == Colour.Yellow || col == Colour.White;
            case "FB":
                return col == Colour.Green || col == Colour.Blue;
            
            default:
                Debug.LogError("invalid sides");
                return true;
        }
    }
}
