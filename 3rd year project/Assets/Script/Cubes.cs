using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;
using System.Diagnostics;
using System.Collections.Specialized;
using ExtensionMethods;
using Moves;
using Faces;
using Pieces;
namespace Cubes
{
    class Cube
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
            for (int i = 0; i < pieces.Length; i++) { p[i] = pieces[i]; }
            return new Cube(p);
        }

        public void rotate(Axis axis, int slice, int quarterTurns)
        {
            //UnityEngine.Debug.Log("Rotating: " + axis + " axis, " + slice + " slice, " + quarterTurns + " quarter Turns");
            //UnityEngine.Debug.Log(new Slice(axis, slice, quarterTurns).getNotation());
            Quaternion rotQuaternion = rotateQuaternion(axis, quarterTurns);
            List<Piece> Piecelist = new List<Piece>();
            foreach (Piece p in pieces)
            {
                //UnityEngine.Debug.Log(p.position[(int)axis]);
                if (p.position[(int)axis] == slice) { Piecelist.Add(p); }
            }
            Piece[] rotatePieces = Piecelist.ToArray();
            //UnityEngine.Debug.Log("Rotating " + rotatePices.Length + " pices");
            foreach (Piece rp in rotatePieces)
            {
                rp.rotate(rotQuaternion);
            }
        }

        public void rotate(string move)
        {
            rotate(new Move(move));
        }

        public void rotate(Move move)
        {
            rotate(move.axis, move.slice, move.angle);
        }

        public void randomMove()
        {
            string[] movesArray = { "F", "U", "R", "B", "D", "L", "F'", "U'", "R'", "B'", "D'", "L'", "F2", "U2", "R2", "B2", "D2", "L2" };
            System.Random rnd = new System.Random();
            rotate(new Move(movesArray[rnd.Next(18)]));
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
    }
}
