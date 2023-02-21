using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cubes;
using Pieces;
using Faces;
using Moves;
using CubeSolvers;
using ExtensionMethods;

namespace OLLs
{
    class OLL : CubeSolver
    {
        public OLL(Cube cube)
        {
            this.cube = cube.Clone();
            moves = new Queue<Move>();
        }

        public override void solve()
        {
            orientYellowEdges();
            orientYellowCorners();
        }

        public bool isFacingUp(Piece p)
        {
            return getYellowDirection(p) == Vector3.up;
        }

        public int countYellowUp(List<Piece> yellowEdges)
        {
            int count = 0;
            foreach (Piece p in yellowEdges)
            {
                if (isFacingUp(p)) { count++; }
            }
            return count;
        }

        public List<Piece> yellowUpEdge(List<Piece> yellowEdges)
        {
            List<Piece> output = new List<Piece>();
            foreach (Piece p in yellowEdges)
            {
                if (isFacingUp(p))
                {
                    output.Add(p);
                }
            }
            return output;
        }

        public List<Piece> notYellowUpEdge(List<Piece> yellowEdges)
        {
            List<Piece> output = new List<Piece>();
            foreach (Piece p in yellowEdges)
            {
                if (!isFacingUp(p))
                {
                    output.Add(p);
                }
            }
            return output;
        }

        public Vector3 getYellowDirection(Piece P)
        {
            return P.getFaceByColour(Colour.Yellow).direction;
        }

        public Vector3 getYellowLeft(List<Piece> notUpYellow)
        {
            foreach (Piece p in notUpYellow)
            {
                if (faceletRelativeDirection(p, Colour.Yellow) == Vector3.left)
                {
                    return p.position;
                }
            }
            Debug.LogError("couldn't get a yellow face facing left");
            return Vector3.zero;
        }

        public int getPiShiftVal(List<Piece> notUpYellow)
        {
            foreach (Piece p1 in notUpYellow)
            {
                foreach (Piece p2 in notUpYellow)
                {
                    if (getYellowDirection(p1) + getYellowDirection(p2) == Vector3.zero && faceletRelativeDirection(p1, Colour.Yellow) == Vector3.forward)
                    {
                        return getShiftVal(p1.position);
                    }
                }
            }
            Debug.LogError("Couldn't find pi shift value");
            return 0;
        }

        public Vector3 sumYellowDirection(List<Piece> notUpYellow)
        {
            Vector3 sum = Vector3.zero;
            foreach (Piece p in notUpYellow)
            {
                sum += getYellowDirection(p);
            }
            return sum;
        }

        public void orientYellowEdges()
        {
            List<Piece> yellowEdges = cube.filter(Colour.Yellow, 2);
            int count = countYellowUp(yellowEdges);
            if (count == 0)
            {
                rotateSequence("F R U R' U' F' B U L U' L' B'");
            }
            else if (count == 2)
            {
                List<Piece> upYellow = yellowUpEdge(yellowEdges);
                if (upYellow[0].position + upYellow[1].position == new Vector3(0, 2, 0))
                {
                    int shiftVal = (getShiftVal(upYellow[0].position) + 1) % 4;
                    rotateSequence(shiftVal, "F R U R' U' F'");
                }
                else
                {
                    Vector3 sum = upYellow[0].position + upYellow[1].position;
                    int shiftVal = getShiftVal(sum);
                    rotateSequence(shiftVal, "B U L U' L' B'");
                }
            }
        }

        public void orientYellowCorners()
        {
            List<Piece> yellowCorners = cube.filter(Colour.Yellow, 3);
            List<Piece> upYellow = yellowUpEdge(yellowCorners);
            List<Piece> notUpYellow = notYellowUpEdge(yellowCorners);
            int count = countYellowUp(yellowCorners);
            if (count == 0)
            {
                if (sumYellowDirection(notUpYellow) == Vector3.zero)
                {
                    int shiftVal = getShiftVal(getYellowLeft(notUpYellow));
                    rotateSequence(shiftVal, "R U R' U R U' R' U R U2 R'");
                }
                else
                {
                    int shiftVal = getPiShiftVal(notUpYellow);
                    rotateSequence(shiftVal, "R U2 R2 U' R2 U' R2 U2 R");
                }
            }
            else if (count == 1)
            {
                if (faceletRelativeDirection(notUpYellow[0], Colour.Yellow) == Vector3.forward)
                {
                    int shiftVal = getShiftVal(sumYellowDirection(notUpYellow) + upYellow[0].position);
                    rotateSequence(shiftVal, "R U R' U R U2 R'");
                }
                else
                {
                    int shiftVal = getShiftVal(sumYellowDirection(notUpYellow) * 2 + upYellow[0].position);
                    rotateSequence(shiftVal, "R U2 R' U' R U' R'");
                }
            }
            else if (count == 2)
            {
                if (getYellowDirection(notUpYellow[0]) == getYellowDirection(notUpYellow[1]))
                {
                    int shiftVal;
                    if (relativePositionOfPieces(notUpYellow[0], notUpYellow[1]) == new Vector3(2, 0, 0))
                    {
                        shiftVal = getShiftVal(notUpYellow[0].position);
                    }
                    else
                    {
                        shiftVal = getShiftVal(notUpYellow[1].position);
                    }
                    rotateSequence(shiftVal, "R2 D R' U2 R D' R' U2 R'");
                }
                else if (getYellowDirection(notUpYellow[0]) == -getYellowDirection(notUpYellow[1]))
                {
                    int shiftVal;
                    if (relativePositionOfPieces(upYellow[0], upYellow[1]) == new Vector3(0, 0, -2))
                    {
                        shiftVal = getShiftVal(upYellow[0].position);
                    }
                    else
                    {
                        shiftVal = getShiftVal(upYellow[1].position);
                    }
                    rotateSequence(shiftVal, "L F R' F' L' F R F'");
                }
                else
                {
                    int shiftVal = getShiftVal(notUpYellow[0].position + 2 * getYellowDirection(notUpYellow[1]));
                    rotateSequence(shiftVal, "F R' F' L F R F' L'");
                }
            }
        }
    }
}