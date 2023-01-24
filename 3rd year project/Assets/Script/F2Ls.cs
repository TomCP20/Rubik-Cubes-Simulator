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

namespace F2Ls
{
    class F2L : CubeSolver
    {
        public F2L(Cube cube)
        {
            this.cube = cube.Clone();
            moves = new Queue<Move>();
        }

        public override void solve()
        {
            List<Piece> whiteCorners = cube.filter(Colour.White, 3);
            foreach (Piece corner in whiteCorners)
            {
                solveCornerEdgePair(corner);
            }
        }

        public void solveCornerEdgePair(Piece corner)
        {
            Piece edge = getEdge(corner);
                // first get edge to the top unless correct x, z
                getEdgeInPosition(edge);
                // then get corner to correct x, z
                getCornerInPosition(corner);
                // identify broad case
                Vector3 cornerstartpos = corner.position;
                Vector3 edgestartpos = edge.position;
                if (cornerstartpos.y == 1) //corner on top
                {
                    if (edgestartpos.y == 1) //edge on top
                    {
                        //case1
                        //case4
                        //case5
                    }
                    else//edge in middle
                    {
                        //case3
                    }
                }
                else //corner on bottom
                {
                    if (edgestartpos.y == 1) //edge on top
                    {
                        //case2
                    }
                    else  //edge in middle
                    {
                        //case6
                    }
                }
                // identify specific case
                // execute solution
        }

        public Piece getEdge(Piece corner)
        {
            foreach (Piece p in cube.pieces)
            {
                Vector3 solved1 = p.SolvedPosition();
                Vector3 solved2 = corner.SolvedPosition();
                if (solved1.y == 0 && solved1.x == solved2.x && solved1.z == solved2.z)
                {
                    return p;
                }
            }
            UnityEngine.Debug.LogError("couldn't find corresponding edge piece");
            return null;
        }

        public void getCornerInPosition(Piece corner)
        {
            Vector3 startpos = corner.position;
            Vector3 targetpos = corner.SolvedPosition();
            if (startpos.x == targetpos.x && startpos.z == targetpos.z)
            {
                return;
            }
            
            if (startpos.y == 1)
            {
                rotateToCorrectPosition(corner, targetpos);
            }
            else
            {
                int shiftVal = getShiftVal(startpos);
                shiftRotate(shiftVal, "R");
                rotateToCorrectPosition(corner, targetpos);
                shiftRotate(shiftVal, "R'");
            }
        }

        public void getEdgeInPosition(Piece edge)
        {
            Vector3 startpos = edge.position;
            Vector3 targetpos = edge.SolvedPosition();
            if (startpos.x == targetpos.x && startpos.z == targetpos.z)
            {
                return;
            } 
            if (startpos.y == 0)
            {
                int shiftVal = getShiftVal(startpos);
                rotateSequence(shiftVal, new string[] {"F", "U", "F'"});
            }
        }
    }
}