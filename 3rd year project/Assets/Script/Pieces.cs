using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Faces;
using System;

namespace Pieces
{
    class Piece
    {
        public Vector3 position;
        public Face[] faces;

        public Piece(Vector3 p, Face[] f)
        {
            position = p;
            faces = f;
        }

        public Piece(Vector3 pos)
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

        public Piece Clone()
        {
            Face[] f = new Face[faces.Length];
            for (int i = 0; i < faces.Length; i++) { f[i] = faces[i]; }
            return new Piece(position, f);
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
                        solved[1] = -1;
                        break;
                    case Colour.Green:
                        solved[2] = 1; 
                        break;                           
                    case Colour.Blue:
                        solved[2] = -1;
                        break;
                    case Colour.Red:
                        solved[0] = 1;
                        break;
                    case Colour.Yellow:
                        solved[1] = -2;
                        break;
                    case Colour.Orange:
                        solved[0] = -1;
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
}
