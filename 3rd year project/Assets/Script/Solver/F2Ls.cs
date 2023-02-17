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
                Debug.Log("solving pair");
                solveCornerEdgePair(corner);
            }
        }

        public void solveCornerEdgePair(Piece corner)
        {
            Piece edge = getEdge(corner);
            
            // first get edge to the top unless correct x, z
            getEdgeInPosition(edge);
            // then get corner to top unless correct x, z
            getCornerInPosition(corner, edge);

            if (!propperPosition(edge))
            {
                Debug.LogError("edge not in valid position");
            }

            if (!propperPosition(corner))
            {
                Debug.LogError("corner not in valid position");
            }

            Vector3 cornerstartpos = corner.position;
            Vector3 edgestartpos = edge.position;
                
            // get corner to correct x, z
            rotateToCorrectPosition(corner, corner.SolvedPosition());
            // identify broad case

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

        public void getCornerInPosition(Piece corner, Piece edge)
        {
            if (propperPosition(corner)) { return; } 
            int shiftVal = getShiftVal(corner.position);
            if (badEdge(edge, corner))
            {
                Debug.Log("Bad edge");
                rotateSequence(shiftVal, new string[] {"R", "U2", "R'"});
            }
            else
            {
                rotateSequence(shiftVal, new string[] {"R", "U", "R'"});
            }
        }

        public void getEdgeInPosition(Piece edge)
        {
            if (propperPosition(edge)) { return; } 
            int shiftVal = getShiftVal(edge.position);
            if (edge.position.y == -1)
            {
                rotateSequence(shiftVal, new string[] {"F2", "U", "F2"});
            }
            else if (edge.position.y == 0)
            {
                rotateSequence(shiftVal, new string[] {"F'", "U", "F"});
            }
            else
            {
                Debug.LogError("edge in top layer");
                Debug.Log(edge.position);
            }
        }

        public bool propperPosition(Piece p)
        {
            return (p.correctPosition() || p.position.y == 1);
        }

        public bool badEdge(Piece edge, Piece corner)
        {
            int ex = (int)edge.position.x;
            int ez = (int)edge.position.z;
            int cx = (int)corner.position.x;
            int cz = (int)corner.position.z;
            if (cx == 1 && cz == 1 && ex == -1 && ez == 0) return true;
            if (cx == 1 && cz == -1 && ex == 0 && ez == 1) return true;
            if (cx == -1 && cz == 1 && ex == 0 && ez == -1) return true;
            if (cx == -1 && cz == -1 && ex == 1 && ez == 0) return true;
            return false;
        }
    }
}