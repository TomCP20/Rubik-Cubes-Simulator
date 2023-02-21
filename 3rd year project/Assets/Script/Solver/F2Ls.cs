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
                //Debug.Log("solving pair");
                solveCornerEdgePair(corner);
            }
        }

        public void solveCornerEdgePair(Piece corner)
        {
            Piece edge = getEdge(corner);
            string CubeCase = "";
            
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
                
            
            // identify broad case from https://ruwix.com/the-rubiks-cube/advanced-cfop-fridrich/first-two-layers-f2l/
            // identify specific case
            // execute solution

            if (corner.position.y == 1) //corner on top
            {
                rotateToCorrectPosition(corner); // get corner to correct x, z if not solved

                if (edge.position.y == 1) //edge on top
                {
                    Vector3 whiteCornerDirection = faceletRelativeDirection(corner, Colour.White);
                    if (whiteCornerDirection == Vector3.up) // white faelet points up
                    {
                        CubeCase = "5";
                        case5(edge, corner);
                    }
                    else
                    {
                        CubeCase = "1 or 4";
                        case1or4(edge, corner);
                    }
                }
                else if (edge.correctPosition()) //edge solved
                {
                    CubeCase = "3";
                    case3(edge, corner);
                }
                else
                {
                    Debug.LogError("edge not in position");
                }
            }
            else if (corner.correctPosition()) //corner solved
            {
                if (edge.position.y == 1) //edge on top
                {
                    CubeCase = "2";
                    case2(edge, corner);
                }
                else if (edge.correctPosition())  //edge solved
                {
                    CubeCase = "6";
                    case6(edge, corner);
                }
                else
                {
                    Debug.LogError("edge not in position");
                }
            }
            else
            {
                Debug.LogError("corner not in position");
            }
            
            if (!(corner.correctOrientation() && edge.correctOrientation()))
            {
                Debug.LogError("Faliure, Case: " + CubeCase);
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

        public bool doesColourEdgeMatchOposite(Piece edge)
        {
            foreach (Face f in edge.faces)
            {
                if (f.defaultDirection() + f.direction == Vector3.zero)
                {
                    return true;
                } 
            }
            return false;
        }
    
        public void case1or4(Piece edge, Piece corner)
        {
            Vector3 relativePosition = relativePositionOfPieces(corner, edge);
            Vector3 whiteCornerDirection = faceletRelativeDirection(corner, Colour.White);
            int shiftval = getShiftVal(corner.SolvedPosition());
            if (relativePosition == Vector3.right)
            {
                if (whiteCornerDirection == Vector3.left)
                {
                    if (doesColourEdgeMatch(edge))
                    {
                        rotateSequence(shiftval, "U' F' U F");
                    }
                    else
                    {
                        rotateSequence(shiftval, "U F' U2 F U' R U R'");
                    }
                }
                else if (whiteCornerDirection == Vector3.forward)
                {
                    if (doesColourEdgeMatch(edge))
                    {
                        rotateSequence(shiftval, "U F' U F U' F' U' F");
                    }
                    else
                    {
                        rotateSequence(shiftval, "F' U F U2 R U R'");
                    }
                }
                else
                {
                    Debug.LogError("bad white corner direction: " + whiteCornerDirection);
                }
            }
            else if (relativePosition == Vector3.back)
            {
                if (whiteCornerDirection == Vector3.left)
                {
                    if (doesColourEdgeMatch(edge))
                    {
                        rotateSequence(shiftval, "U' R U' R' U R U R'");
                    }
                    else
                    {
                        rotateSequence(shiftval, "R U' R' U2 F' U' F");
                    }
                }
                else if (whiteCornerDirection == Vector3.forward)
                {
                    if (doesColourEdgeMatch(edge))
                    {
                        rotateSequence(shiftval, "U R U' R'");
                    }
                    else
                    {
                        rotateSequence(shiftval, "U' R U2 R' U F' U' F");
                    }
                }
                else
                {
                    Debug.LogError("bad white corner direction: " + whiteCornerDirection);
                }
            }
            else if (relativePosition == new Vector3(2, 0, -1))
            {
                if (whiteCornerDirection == Vector3.left)
                {
                    if (doesColourEdgeMatchOposite(edge))
                    {
                        rotateSequence(shiftval, "U' R U R' U R U R'");
                    }
                    else
                    {
                        rotateSequence(shiftval, "U F' U' F U F' U2 F"); // U F' U' F U F' U2 F
                    }
                }
                else if (whiteCornerDirection == Vector3.forward)
                {
                    if (doesColourEdgeMatchOposite(edge))
                    {
                        rotateSequence(shiftval, "U' R U2 R' U' R U2 R'");
                    }
                    else
                    {
                        rotateSequence(shiftval, "F' U' F");
                    }
                }
                else
                {
                    Debug.LogError("bad white corner direction: " + whiteCornerDirection);
                }
            }
            else if (relativePosition == new Vector3(1, 0, -2))
            {
                if (whiteCornerDirection == Vector3.left)
                {
                    if (doesColourEdgeMatchOposite(edge))
                    {
                        rotateSequence(shiftval, "U F' U2 F U F' U2 F"); // U F' U2 F U F' U2 F
                    }
                    else
                    {
                        rotateSequence(shiftval, "R U R'");
                    }
                }
                else if (whiteCornerDirection == Vector3.forward)
                {
                    if (doesColourEdgeMatchOposite(edge))
                    {
                        rotateSequence(shiftval, "U F' U' F U' F' U' F"); // U F' U' F U' F' U' F
                    }
                    else
                    {
                        rotateSequence(shiftval, "U' R U R' U' R U2 R'");
                    }
                }
                else
                {
                    Debug.LogError("bad white corner direction: " + whiteCornerDirection);
                }
            }
            else
            {
                Debug.LogError("Bad relative position, " + relativePosition);
            }
        }
        public void case2(Piece edge, Piece corner)
        {
            machTopEdgeColour(edge);
            Vector3 whiteCornerDirection = faceletRelativeDirection(corner, Colour.White);
            Vector3 relativePosition = relativePositionOfPieces(corner, edge);
            int shiftval = getShiftVal(edge.SolvedPosition());
            if (corner.correctOrientation()) // white face points down
            {
                if (relativePosition == new Vector3(1, 2, 0))
                {
                    rotateSequence(shiftval, new string[] {"U", "R", "U'", "R'", "U'", "F'", "U", "F"});
                }
                else if (relativePosition == new Vector3(0, 2, -1))
                {
                    rotateSequence(shiftval, new string[] {"U'", "F'", "U", "F", "U", "R", "U'", "R'"});
                }
                else
                {
                    Debug.LogError("bad relative position: " + relativePosition);
                }
            }
            else if (whiteCornerDirection == Vector3.left) // white faces right due to mirroring issues
            {
                if (relativePosition == new Vector3(1, 2, 0))
                {
                    rotateSequence(shiftval, new string[] {"F'", "U", "F", "U'", "F'", "U", "F"});
                }
                else if (relativePosition == new Vector3(0, 2, -1))
                {
                    rotateSequence(shiftval, new string[] {"R", "U", "R'", "U'", "R", "U", "R'"});
                }
                else
                {
                    Debug.LogError("bad relative position: " + relativePosition);
                }
            }
            else if (whiteCornerDirection == Vector3.forward) // white faces forward
            {
                if (relativePosition == new Vector3(1, 2, 0))
                {
                    rotateSequence(shiftval, new string[] {"F'", "U'", "F", "U", "F'", "U'", "F"});
                }
                else if (relativePosition == new Vector3(0, 2, -1))
                {
                    rotateSequence(shiftval, "R U' R' U R U' R'");
                }
                else
                {
                    Debug.LogError("bad relative position: " + relativePosition);
                }
            }
            else
            {
                Debug.LogError("bad white Corner Direction: " + whiteCornerDirection);
            }
        }
        public void case3(Piece edge, Piece corner)
        {
            Vector3 whiteCornerDirection = faceletRelativeDirection(corner, Colour.White);
            int shiftval = getShiftVal(edge.SolvedPosition());
            if (whiteCornerDirection == Vector3.up)
            {
                if (edge.correctOrientation())
                {
                    rotateSequence(shiftval, new string[] {"R", "U", "R'", "U'", "R", "U", "R'", "U'", "R", "U", "R'"});
                }
                else
                {
                    rotateSequence(shiftval, "R U' R' U F' U F");
                }
            }
            else if (whiteCornerDirection == Vector3.left)
            {
                if (edge.correctOrientation())
                {
                    rotateSequence(shiftval, new string[] {"U", "F'", "U", "F", "U", "F'", "U2", "F"});
                }
                else
                {
                    rotateSequence(shiftval, "U F' U' F U' R U R'");
                }
            }
            else if (whiteCornerDirection == Vector3.forward)
            {
                if (edge.correctOrientation())
                {
                    rotateSequence(shiftval, new string[] {"U'", "R", "U'", "R'", "U'", "R", "U2", "R'"});
                }
                else
                {
                    rotateSequence(shiftval, "U' R U R' U F' U' F");
                }
            }
            else
            {
                Debug.LogError("bad white Corner Direction: " + whiteCornerDirection);
            }
        }
        public void case5(Piece edge, Piece corner)
        {
            Vector3 relativePosition = relativePositionOfPieces(corner, edge);
            int shiftval = getShiftVal(corner.position);
            if (relativePosition == Vector3.right)
            {
                if (doesColourEdgeMatch(edge))
                {
                    rotateSequence(shiftval, "F' U2 F U F' U' F");
                }
                else
                {
                    rotateSequence(shiftval, "R U R' U' U' R U R' U' R U R'");
                }
            }
            else if (relativePosition == Vector3.back)
            {
                if (doesColourEdgeMatch(edge))
                {
                    rotateSequence(shiftval, "R U2 R' U' R U R'");
                }
                else
                {
                    rotateSequence(shiftval, "F' U' F U2 F' U' F U F' U' F");
                }
            }
            else if (relativePosition == new Vector3(2, 0, -1))
            {
                if (doesColourEdgeMatchOposite(edge))
                {
                    rotateSequence(shiftval, "U2 R U R' U R U' R'");
                }
                else
                {
                    rotateSequence(shiftval, "U' F' U2 F U' F' U F");
                }
            }
            else if (relativePosition == new Vector3(1, 0, -2))
            {
                if (doesColourEdgeMatchOposite(edge))
                {
                    rotateSequence(shiftval, "U2 F' U' F U' F' U F");
                }
                else
                {
                    rotateSequence(shiftval, "U R U2 R' U R U' R'");
                }
            }
            else
            {
                Debug.LogError("Bad relative position, " + relativePosition);
            }
        }
        public void case6(Piece edge, Piece corner)
        {
            Vector3 whiteCornerDirection = faceletRelativeDirection(corner, Colour.White);
            int shiftval = getShiftVal(edge.SolvedPosition());
            if (whiteCornerDirection == Vector3.down)
            {
                if (edge.correctOrientation())
                {
                    return; // the edge and corner are solved
                }
                else
                {
                    rotateSequence(shiftval, "R U' R' U F' U2 F U F' U2 F");
                }
            }
            else if (whiteCornerDirection == Vector3.left)
            {
                if (edge.correctOrientation())
                {
                    rotateSequence(shiftval, "R U' R' U R U2 R' U R U' R'");
                }
                else
                {
                    rotateSequence(shiftval, "R U R' U' R U' R' U2 F' U' F");
                }
            }
            else if (whiteCornerDirection == Vector3.forward)
            {
                if (edge.correctOrientation())
                {
                    rotateSequence(shiftval, "R U' R' U' R U R' U' R U2 R'");
                }
                else
                {
                    rotateSequence(shiftval, "R U' R' U F' U' F U' F' U' F");
                }
            }
            else
            {
                Debug.LogError("bad white corner direction: " + whiteCornerDirection);
            }
        }
        public bool edgeIsLeftOfSolved(Piece edge)
        {
            return edge.position.x * edge.SolvedPosition().z + edge.position.z * edge.SolvedPosition().x == 1;
        }

        
    }
}