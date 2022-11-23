using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;
using System.Diagnostics;
using System.Collections.Specialized;
using ExtensionMethods;

namespace CubeNamespace
{
    enum Colour
    {
        Black,
        White,
        Green,
        Blue,
        Red,
        Yellow,
        Orange,
    }

    enum Axis
    {
        X = 0,
        Y = 1,
        Z = 2
    }
    readonly struct Slice
    {
        public Axis axis { get; }
        public int slicePos { get; }

        public Slice(Axis a, int s)
        {
            this.axis = a;
            this.slicePos = s;
        }

        public Slice(string notation)
        {
            switch (notation)
            {
                case "F":
                    axis = Axis.X;
                    slicePos = 1;
                    break;
                case "U":
                    axis = Axis.Y;
                    slicePos = 1;
                    break;
                case "R":
                    axis = Axis.Z;
                    slicePos = 1;
                    break;
                case "B":
                    axis = Axis.X;
                    slicePos = -1;
                    break;
                case "D":
                    axis = Axis.Y;
                    slicePos = -1;
                    break;
                case "L":
                    axis = Axis.Z;
                    slicePos = -1;
                    break;
                default:
                    axis = Axis.X;
                    slicePos = 0;
                    break;
            }
        }

        public string getNotation()
        {
            switch (axis)
            {
                case Axis.X:
                    if (slicePos == 1) { return "F"; }
                    else { return "B"; }
                case Axis.Y:
                    if (slicePos == 1) { return "U"; }
                    else { return "D"; }
                case Axis.Z:
                    if (slicePos == 1) { return "R"; }
                    else { return "L"; }
                default:
                    return "";
            }
        }
    }

    class Face : ICloneable
    {
        public Colour colour;
        public Vector3 direction;//unit vector

        public Face(Colour c, Vector3 d)
        {
            colour = c;
            direction = d;
        }

        public Face(Vector3 d)
        {
            direction = d;
            colour = defaultColour(direction);
        }

        public object Clone()
        {
            return new Face(colour, direction);
        }

        public void rotate(Quaternion rotQuaternion)
        {
            direction = Vector3Int.RoundToInt(rotQuaternion * direction);
        }

        private Colour defaultColour(Vector3 d)
        {
            switch (d)
            {
                case Vector3 vec when vec.Equals(Vector3.right): return Colour.Green;
                case Vector3 vec when vec.Equals(Vector3.left): return Colour.Yellow;
                case Vector3 vec when vec.Equals(Vector3.up): return Colour.White;
                case Vector3 vec when vec.Equals(Vector3.down): return Colour.Blue;
                case Vector3 vec when vec.Equals(Vector3.forward): return Colour.Red;
                case Vector3 vec when vec.Equals(Vector3.back): return Colour.Orange;
                default: return Colour.Black;
            }
        }
    }

    class Pice : ICloneable
    {
        public Vector3 position;
        public Face[] faces;

        public Pice(Vector3 p, Face[] f)
        {
            position = p;
            faces = f;
        }

        public Pice(Vector3 pos)
        {
            position = pos;
            List<Face> FaceList = new List<Face>();
            for (int i = 0; i < 3; i++)
            {
                if (position[i] != 0)
                {
                    FaceList.Add(new Face(PositionToDirection((Axis)i)));
                }
            }
            faces = FaceList.ToArray();
        }

        public object Clone()
        {
            Face[] f = new Face[faces.Length];
            for (int i = 0; i < faces.Length; i++) { f[i] = faces[i]; }
            return new Pice(position, f);
        }

        public void rotate(Quaternion rotQuaternion)
        {
            position = Vector3Int.RoundToInt(rotQuaternion * position);
            foreach (Face face in faces) { face.rotate(rotQuaternion); }
        }

        private Vector3 PositionToDirection(Axis axis)
        {
            Vector3 direction = Vector3.zero;
            direction[(int)axis] = this.position[(int)axis];
            return direction;
        }

        public Vector3 SolvedPosition()
        {
            Vector3 solved = Vector3.zero;
            foreach (Face face in faces)
            {
                switch (face.colour)
                {
                    case Colour.White:
                        solved[1] = 1;
                        break;
                    case Colour.Green:
                        solved[0] = 1; 
                        break;                           
                    case Colour.Blue:
                        solved[1] = -1;
                        break;
                    case Colour.Red:
                        solved[2] = 1;
                        break;
                    case Colour.Yellow:
                        solved[0] = -1;
                        break;
                    case Colour.Orange:
                        solved[2] = -1;
                        break;
                }
            }
            return solved;
        }

        public bool containsColour(Colour c)
        {
            foreach(Face f in faces)
            {
                if (f.colour == c)
                {
                    return true;
                }
            }
            return false;
        }
    }

    class Cube : ICloneable
    {
        public Pice[] pices;

        public Cube(Pice[] p)
        {
            pices = p;
        }

        public Cube()
        {
            pices = new Pice[26];
            Vector3[] positions = genPositions();
            for (int i = 0; i < 26; i++)
            {
                pices[i] = new Pice(positions[i]);
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

        public object Clone()
        {
            Pice[] p = new Pice[pices.Length];
            for (int i = 0; i < pices.Length; i++) { p[i] = pices[i]; }
            return new Cube(p);
        }

        public void rotate(Axis axis, int slice, int quarterTurns)
        {
            Quaternion rotQuaternion = rotateQuaternion(axis, quarterTurns);
            List<Pice> Picelist = new List<Pice>();
            foreach (Pice p in pices)
            {
                //UnityEngine.Debug.Log(p.position[(int)axis]);
                if (p.position[(int)axis] == slice) { Picelist.Add(p); }
            }
            Pice[] rotatePices = Picelist.ToArray();
            //UnityEngine.Debug.Log("Rotating " + rotatePices.Length + " pices");
            foreach (Pice rp in rotatePices)
            {
                rp.rotate(rotQuaternion);
            }
        }

        public void rotate(string move)
        {
            string face = move.Substring(0, 1);
            int angle = 0;
            if (move.Length == 1) { angle = 1; }
            else if (move.Substring(1) == "'") { angle = -1; }
            else if (move.Substring(1) == "2") { angle = 2; }
            UnityEngine.Debug.Log(face);
            UnityEngine.Debug.Log(angle);
            rotate(new Slice(face), angle);
        }

        public void rotate(Slice s, int angle)
        {
            rotate(s.axis, s.slicePos, angle);
        }

        public void randomMove()
        {
            string[] facesArray = {"F", "U", "R", "B", "D", "L"};
            System.Random rnd = new System.Random();
            rotate(new Slice(facesArray[rnd.Next(6)]), rnd.Next(1, 4));
        }

        public void randomMoveSequence(int n = 25)
        {
            for (int i = 0; i < n; i++)
            {
                randomMove();
            }
        }

        private Quaternion rotateQuaternion(Axis axis, int quarterTurns)
        {
            Vector3 rotationVector = new Vector3(0, 0, 0);
            rotationVector[(int)axis] = 90 * quarterTurns;
            return Quaternion.Euler(rotationVector);
        }

        public void solve()
        {
            whiteCross();
        }

        public void whiteCross()
        {
            List<Pice> whiteEdges = getWhiteEdges();
            foreach(Pice whiteEdge in whiteEdges)
            {
                whiteEdgePosition(whiteEdge);
            }   
        }

        private void whiteEdgePosition(Pice whiteEdge)
        {
            Vector3 startPos = whiteEdge.position;
            Vector3 targetPos = whiteEdge.SolvedPosition();
            UnityEngine.Debug.Log(startPos);
            int yPos = (int)startPos.y;
            switch (yPos)
            {
                case 1:
                    if (startPos.x + targetPos.x == 0 && startPos.z + targetPos.z == 0) // if the target position is oposite the current position
                    {
                        Axis a;
                        int s;
                        if (targetPos.x != 0)
                        {
                            a = Axis.X;
                            s = (int)startPos.x;
                        }
                        else
                        {
                            a = Axis.Z;
                            s = (int)startPos.z;
                        }
                        rotate(new Slice(a, s), 2);
                        rotate(new Slice(Axis.Y, -1), 2);
                        rotate(new Slice(a, -s), 2);                                                   
                    }
                    else if (startPos != targetPos) //if its not oposite and its not solved then the pice must be diagonal from its goal position
                    {
                        
                    }
                    break;
                case 0:
                    break;
                case -1:
                    break;
                default:
                    UnityEngine.Debug.LogError("invalid y position");
                    break;
                }
            } 

        private List<Pice> getWhiteEdges()
        {
            List<Pice> whiteEdges = new List<Pice>();
            foreach(Pice p in pices)
            {
                if (p.position.ManhattanDistance() == 2 && p.containsColour(Colour.White))
                {
                    whiteEdges.Add(p);
                }
            }
            return whiteEdges;
        }
    }  
}
