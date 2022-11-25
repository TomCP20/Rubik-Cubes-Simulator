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
        public int angle { get; }

        public Slice(Axis a, int s, int an)
        {
            this.axis = a;
            this.slicePos = s;
            this.angle = an;
        }

        public Slice(string notation)
        {
            string face = notation.Substring(0, 1);
            if (notation.Length == 1) { angle = 1; }
            else if (notation.Substring(1) == "'") { angle = -1; }
            else if (notation.Substring(1) == "2") { angle = 2; }
            else { angle = 0; }
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
                    angle = -angle;
                    break;
                case "D":
                    axis = Axis.Y;
                    slicePos = -1;
                    angle = -angle;
                    break;
                case "L":
                    axis = Axis.Z;
                    slicePos = -1;
                    angle = -angle;
                    break;
                default:
                    axis = Axis.X;
                    slicePos = 0;
                    angle = 0;
                    break;
            }
        }

        public string getNotation()
        {
            string output = "";
            switch (axis)
            {
                case Axis.X:
                    if (slicePos == 1) { output = "F"; }
                    else { output =  "B"; }
                    break;
                case Axis.Y:
                    if (slicePos == 1) { output = "U"; }
                    else { output =  "D"; }
                    break;
                case Axis.Z:
                    if (slicePos == 1) { output = "R"; }
                    else { output = "L"; }
                    break;
                default:
                    output =  "";
                    break;
            }
            if (slicePos == 1)
            {
                if (angle == 2)
                {
                    output += "2";
                }
                else if (angle == -1)
                {
                    output += "'";
                }
            }
            else
            {
                if (angle == 2)
                {
                    output += "2";
                }
                else if (angle == 1)
                {
                    output += "'";
                }
            }
            return output;
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
            //UnityEngine.Debug.Log("Rotating: " + axis + " axis, " + slice + " slice, " + quarterTurns + " quarter Turns");
            //UnityEngine.Debug.Log(new Slice(axis, slice, quarterTurns).getNotation());
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
            rotate(new Slice(move));
        }

        public void rotate(Slice s)
        {
            rotate(s.axis, s.slicePos, s.angle);
        }

        public void randomMove()
        {
            string[] movesArray = {"F", "U", "R", "B", "D", "L", "F'", "U'", "R'", "B'", "D'", "L'", "F2", "U2", "R2", "B2", "D2", "L2"};
            System.Random rnd = new System.Random();
            rotate(new Slice(movesArray[rnd.Next(18)]));
        }

        public void randomMoveSequence(int n = 100)
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
            blueCross();
        }

        public void blueCross()
        {
            List<Pice> blueEdges = getblueEdges();
            foreach(Pice blueEdge in blueEdges)
            {
                blueEdgePosition(blueEdge);              
            }        
        }

        private void blueEdgePosition(Pice blueEdge)
        {
            Vector3 startPos = blueEdge.position;
            Vector3 targetPos = blueEdge.SolvedPosition();
            //UnityEngine.Debug.Log(startPos);
            int yPos = (int)startPos.y;
            UnityEngine.Debug.Log("solving edge at y " + yPos);
            switch (yPos)
            {
                case -1:
                    blueEdgePositionBottom(blueEdge, targetPos, startPos);
                    break;          
                case 0:
                    blueEdgePositionMiddle(blueEdge, targetPos, startPos);
                    break;
                case 1:
                    blueEdgePositionTop(blueEdge, targetPos, startPos);
                    break;
                default:
                    UnityEngine.Debug.LogError("invalid y position");
                    break;
                }
            if (targetPos == blueEdge.position)
            {
                UnityEngine.Debug.Log("edge solved");
            }
            else
            {
                UnityEngine.Debug.LogError("edge not solved: start: " + startPos + " target: " + targetPos + " current: " + blueEdge.position);
            }

        } 

        private void blueEdgePositionTop(Pice blueEdge, Vector3 targetPos, Vector3 startPos)
        {
            Vector3 midPos;
            switch (startPos.x*targetPos.x + startPos.z * targetPos.z)
            {
                case 1: //solved
                    break;
                case 0: //oposite
                    rotate(Axis.Y, 1, 2);
                    break;
                case -1: //diagonal
                    if (startPos.x == 0)
                    {
                        rotate(Axis.Y, 1, (int)startPos.z);
                    }
                    else
                    {
                        rotate(Axis.Y, 1, (int)startPos.x);
                    }
                    break;
                default:
                    UnityEngine.Debug.Log("error 1q2");
                    break;
            }
            midPos = blueEdge.position;
            if (midPos.x == 0)
            {
                rotate(Axis.Z, (int)midPos.z, 2);
            }
            else
            {
                rotate(Axis.X, (int)midPos.x, 2);
            }
        }

        private void blueEdgePositionMiddle(Pice blueEdge, Vector3 targetPos, Vector3 startPos) // fix this thing
        {
            UnityEngine.Debug.Log("solving edge at middle layer");
            if (startPos.x == targetPos.x)
            {
                rotate(Axis.X, (int)startPos.x, (int)startPos.z);
            }
            else if (startPos.z == targetPos.z)
            {
                rotate(Axis.Z, (int)startPos.z, (int)startPos.x);
            }
            else if (targetPos.x == 0)
            {
                UnityEngine.Debug.Log("solving weird diagonal");
                rotate(Axis.Y, -1, -(int)startPos.x*(int)startPos.z);
                rotate(Axis.X, (int)startPos.x, (int)startPos.z);
                rotate(Axis.Y, -1, (int)startPos.x*(int)startPos.z);
            }
            else
            {
                UnityEngine.Debug.Log("solving weird diagonal");
                rotate(Axis.Y, -1, -(int)startPos.z*(int)startPos.x);
                rotate(Axis.Z, (int)startPos.z, (int)startPos.x);
                rotate(Axis.Y, -1, (int)startPos.z*(int)startPos.x);
            }
            /*
            Vector3 midPos;
            midPos = new Vector3(startPos.x, -1, 0);
            switch (midPos.x*targetPos.x + midPos.z * targetPos.z)
            {
                case 1:
                    rotate(Axis.X, (int)startPos.x, (int)startPos.z);
                    break;
                case 0:
                    rotate(Axis.Y, -1, 2);
                    rotate(Axis.X, (int)startPos.x, (int)startPos.z);
                    rotate(Axis.Y, -1, 2);
                    break;
                case -1:
                    if (startPos.x == 0)
                    {
                        rotate(Axis.Y, -1, (int)targetPos.x);
                        rotate(Axis.X, (int)startPos.x, (int)startPos.z);
                        rotate(Axis.Y, -1, -(int)targetPos.x);
                    }
                    else
                    {
                        rotate(Axis.Y, -1, (int)targetPos.z);
                        rotate(Axis.X, (int)startPos.x, (int)startPos.z);
                        rotate(Axis.Y, -1, -(int)targetPos.z);
                    }
                    break;
                default:
                    break;
            }
            */

        }
        private void blueEdgePositionBottom(Pice blueEdge, Vector3 targetPos, Vector3 startPos)
        {
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
                        rotate(a, s, 2);
                        rotate(Axis.Y, 1, 2);
                        rotate(a, -s, 2);                                                   
                    }                    
                    else if (startPos != targetPos) //if its not oposite and its not solved then the pice must be diagonal from its goal position
                    {
                        if (startPos.x + targetPos.x == 1)
                        {
                            if (startPos.z + targetPos.z == 1)
                            {
                                rotate(Axis.Z, 1, 1);
                                rotate(Axis.Y, -1, -1);
                                rotate(Axis.Z, 1, -1);
                                rotate(Axis.Y, -1, 1);
                                rotate(Axis.Z, 1, 1);                                
                            }
                            else
                            {
                                rotate(Axis.X, 1, 1);
                                rotate(Axis.Y, -1, -1);
                                rotate(Axis.X, 1, -1);
                                rotate(Axis.Y, -1, 1);
                                rotate(Axis.X, 1, 1);
                            }
                        }
                        else
                        {
                            if (startPos.z + targetPos.z == 1)
                            {
                                rotate(Axis.X, -1, -1);
                                rotate(Axis.Y, -1, -1);
                                rotate(Axis.X, -1, 1);
                                rotate(Axis.Y, -1, 1);
                                rotate(Axis.X, -1, -1);
                            }
                            else
                            {
                                rotate(Axis.Z, -1, -1);
                                rotate(Axis.Y, -1, -1);
                                rotate(Axis.Z, -1, 1);
                                rotate(Axis.Y, -1, 1);
                                rotate(Axis.Z, -1, -1);
                            }
                        }
                    }
        }
        private void blueOrientation(Pice blueEdge)
        {
            bool good = false;
            foreach(Face f in blueEdge.faces)
            {
                if (f.colour == Colour.Blue && f.direction == Vector3.down)
                {
                    good = true;
                }
            }
            if (!good)
            {
                if (blueEdge.position.x != 0)
                {
                    if (blueEdge.position.x == 1)
                    {
                        rotate(Axis.X, 1, 1);
                        rotate(Axis.Y, -1, 1);
                        rotate(Axis.Z, -1, -1);
                        rotate(Axis.Y, -1, -1);
                    }
                    else
                    {
                        rotate(Axis.X, -1, -1);
                        rotate(Axis.Y, -1, 1);
                        rotate(Axis.Z, 1, 1);
                        rotate(Axis.Y, -1, -1);
                    }
                }
                else
                {
                    if (blueEdge.position.z == 1)
                    {
                        rotate(Axis.Z, 1, 1);
                        rotate(Axis.Y, -1, 1);
                        rotate(Axis.X, 1, 1);
                        rotate(Axis.Y, -1, -1);
                    }
                    else
                    {
                        rotate(Axis.Z, -1, -1);
                        rotate(Axis.Y, -1, 1);
                        rotate(Axis.X, -1, -1);
                        rotate(Axis.Y, -1, -1);
                    }
                }
            }
        }
        private List<Pice> getblueEdges()
        {
            List<Pice> blueEdges = new List<Pice>();
            foreach(Pice p in pices)
            {
                if (p.position.ManhattanDistance() == 2 && p.containsColour(Colour.Blue))
                {
                    blueEdges.Add(p);
                }
            }
            return blueEdges;
        }
    }  
}
