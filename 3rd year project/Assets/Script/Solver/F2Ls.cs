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
                
            
            // identify broad case from https://ruwix.com/the-rubiks-cube/advanced-cfop-fridrich/first-two-layers-f2l/
            // identify specific case
            // execute solution

            if (cornerstartpos.y == 1) //corner on top
            {
                rotateToCorrectPosition(corner, corner.SolvedPosition()); // get corner to correct x, z if not solved

                if (edgestartpos.y == 1) //edge on top
                {
                    //case1
                    //case4
                    //case5
                }
                else //edge solved
                {
                    //case3
                }
            }
            else //corner solved
            {
                if (edgestartpos.y == 1) //edge on top
                {
                    case2(edge, corner);
                }
                else  //edge solved
                {
                    //case6
                }
            }
            if (corner.correctOrientation() && edge.correctOrientation())
            {
                Debug.Log("Succsess");
            }
            else
            {
                Debug.LogError("Faliure");
            }
            
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
                //Debug.Log("Bad edge");
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

        public void machTopEdgeColour(Piece edge)
        {
            for (int i = 0; i < 4; i++)
            {
                if (doesColourEdgeMatch(edge))
                {
                    Debug.Log("succsessfull match");
                    return;
                }
                rotate("U");
            }
            Debug.LogError("could not match edge on top layer to middle layer face");

        }

        public bool doesColourEdgeMatch(Piece edge)
        {
            foreach (Face f in edge.faces)
            {
                if (f.correctOrientation())
                {
                    return true;
                } 
            }
            return false;
        }
    
        public void case2(Piece edge, Piece corner)
        {
            machTopEdgeColour(edge);
            Vector3 whiteCornerDirection = faceletRelativeDirection(corner, Colour.White);
            int shiftval = getShiftVal(edge.SolvedPosition());
            if (corner.correctOrientation()) // white face points down
            {
                if (edgeIsLeftOfSolved(edge))
                {
                    rotateSequence(shiftval, new string[] {"U", "R", "U'", "R'", "U'", "F'", "U", "F"});
                }
                else
                {
                    rotateSequence(shiftval, new string[] {"U'", "F'", "U", "F", "U", "R", "U'", "R'"});
                }
            }
            else if (whiteCornerDirection == Vector3.left) // white faces right due to mirroring issues
            {
                if (edgeIsLeftOfSolved(edge))
                {
                    rotateSequence(shiftval, new string[] {"F'", "U", "F", "U'", "F'", "U", "F"});
                }
                else
                {
                    rotateSequence(shiftval, new string[] {"R", "U", "R'", "U'", "R", "U", "R'"});
                }
            }
            else // white faces forward
            {
                if (edgeIsLeftOfSolved(edge))
                {
                    rotateSequence(shiftval, new string[] {"R", "U'", "R'", "U", "R", "U'", "R'"});
                }
                else
                {
                    rotateSequence(shiftval, new string[] {"F'", "U'", "F", "U", "F'", "U'", "F"});
                }
            }
        }

        public bool edgeIsLeftOfSolved(Piece edge)
        {
            return edge.position.x * edge.SolvedPosition().z + edge.position.z * edge.SolvedPosition().x == 1;
        }
    }
}