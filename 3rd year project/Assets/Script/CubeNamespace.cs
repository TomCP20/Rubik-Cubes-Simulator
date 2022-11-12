using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;
using System.Diagnostics;
using System.Collections.Specialized;

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
                    FaceList.Add(new Face(PositionToDirection(position, (Axis)i)));
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

        private Vector3 PositionToDirection(Vector3 p, Axis axis)
        {
            Vector3 direction = Vector3.zero;
            direction[(int)axis] = p[(int)axis];
            return direction;
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
            List<Pice> Picelist = new List<Pice>();
            Vector3 position;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    for (int k = -1; k <= 1; k++)
                    {
                        position = new Vector3(i, j, k);
                        if (position.magnitude >= 1)
                        {
                            Picelist.Add(new Pice(position));
                        }
                    }
                }
            }
            pices = Picelist.ToArray();
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
            if (move.Length == 1)
            {
                angle = 1;
            }
            else if (move.Substring(1) == "'")
            {
                angle = -1;
            }
            else if (move.Substring(1) == "2")
            {
                angle = 2;
            }
            UnityEngine.Debug.Log(face);
            UnityEngine.Debug.Log(angle);
            rotate(face, angle);
        }

        public void rotate(string face, int angle)
        {
            Axis axis;
            if (face == "F" || face == "B")
            {
                axis = Axis.X;
            }
            else if (face == "U" || face == "D")
            {
                axis = Axis.Y;
            }
            else
            {
                axis = Axis.Z;
            }
            int slice;
            if (face == "F" || face == "U" || face == "R")
            {
                slice = 1;
            }
            else
            {
                slice = -1;
                angle = -angle;
            }
            rotate(axis, slice, angle);
        }

        public void randomMove()
        {
            string[] faces = {"F", "U", "R", "B", "D", "L"};
            System.Random rnd = new System.Random();
            string face = faces[rnd.Next(6)];
            int angle = rnd.Next(1, 4);
            rotate(face, angle);
        }

        public void randomMoveSequence(int n = 20)
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
    }
}
