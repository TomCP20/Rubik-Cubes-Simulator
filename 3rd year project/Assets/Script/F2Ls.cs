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
                // then get corner to top unless correct x, z
                getCornerInPosition(corner);

                if (!propperPosition(edge))
                {
                    UnityEngine.Debug.LogError("edge not in valid position");
                }

                if (!propperPosition(corner))
                {
                    UnityEngine.Debug.LogError("corner not in valid position");
                }
                // get corner to correct x, z
                rotateToCorrectPosition(corner, corner.SolvedPosition());
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
            if (propperPosition(corner)) { return; } 
            int shiftVal = getShiftVal(corner.position);
            rotateSequence(shiftVal, new string[] {"R", "U", "R''"});
        }

        public void getEdgeInPosition(Piece edge)
        {
            if (propperPosition(edge)) { return; } 
            int shiftVal = getShiftVal(edge.position);
            rotateSequence(shiftVal, new string[] {"F", "U", "F'"});
        }

        public bool propperPosition(Piece p)
        {
            Vector3 startpos = p.position;
            Vector3 targetpos = p.SolvedPosition();
            return ((startpos.x == targetpos.x && startpos.z == targetpos.z) || startpos.y == 1);
        }
    }
}