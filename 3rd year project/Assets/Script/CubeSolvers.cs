using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cubes;
using Pieces;
using Faces;
using Moves;
using ExtensionMethods;

namespace CubeSolvers
{
    class CubeSolver
    {
        private Cube cube;
        Queue<Move> moves;

        public CubeSolver(Cube cube)
        {
            //this.cube = cube.Clone();
            this.cube = cube;
        }

        private void rotate(Axis axis, int slice, int quarterTurns)
        {
            cube.rotate(axis, slice, quarterTurns);
            moves.Enqueue(new Move(axis, slice, quarterTurns));
        }

        public Queue<Move> getSolution()
        {
            solve();
            return moves;
        }
        public Cube getSlovedCube()
        {
            solve();
            return cube;
        }
        private void solve()
        {
            moves = new Queue<Move>();
            whiteCross();
        }

        private void whiteCross()
        {
            List<Piece> whiteEdges = getWhiteEdges();
            foreach(Piece whiteEdge in whiteEdges)
            {
                whiteEdgePosition(whiteEdge);    
                whiteOrientation(whiteEdge);          
            }        
        }

        private void whiteEdgePosition(Piece whiteEdge)
        {
            Vector3 startPos = whiteEdge.position;
            Vector3 targetPos = whiteEdge.SolvedPosition();
            //UnityEngine.Debug.Log(startPos);
            int yPos = (int)startPos.y;
            UnityEngine.Debug.Log("solving edge at y " + yPos);
            switch (yPos)
            {
                case -1:
                    whiteEdgePositionBottom(whiteEdge, targetPos, startPos);
                    break;          
                case 0:
                    whiteEdgePositionMiddle(whiteEdge, targetPos, startPos);
                    break;
                case 1:
                    whiteEdgePositionTop(whiteEdge, targetPos, startPos);
                    break;
                default:
                    UnityEngine.Debug.LogError("invalid y position");
                    break;
                }
            if (targetPos == whiteEdge.position)
            {
                UnityEngine.Debug.Log("edge solved");
            }
            else
            {
                UnityEngine.Debug.LogError("edge not solved: start: " + startPos + " target: " + targetPos + " current: " + whiteEdge.position);
            }

        } 

        private void whiteEdgePositionTop(Piece whiteEdge, Vector3 targetPos, Vector3 startPos)
        {
            UnityEngine.Debug.Log("solving edge at top layer");
            Vector3 midPos;
            UnityEngine.Debug.Log(startPos.x*targetPos.x + startPos.z * targetPos.z);
            switch (startPos.x*targetPos.x + startPos.z * targetPos.z)
            {
                case 1: //solved
                    UnityEngine.Debug.Log("solved");
                    break;
                case -1: //oposite
                    UnityEngine.Debug.Log("oposite");
                    rotate(Axis.Y, 1, 2);
                    break;
                case 0: //diagonal
                    UnityEngine.Debug.Log("diagonal");
                    if (startPos.x == 0)
                    {
                        rotate(Axis.Y, 1, (int)targetPos.x*(int)startPos.z);
                    }
                    else
                    {
                        rotate(Axis.Y, 1, -(int)targetPos.z*(int)startPos.x);
                    }
                    break;
                default:
                    UnityEngine.Debug.Log("error 1q2");
                    break;
            }

            midPos = whiteEdge.position;
            UnityEngine.Debug.Log("midPos: " + midPos);
            if (midPos.x == 0)
            {
                rotate(Axis.Z, (int)midPos.z, 2);
            }
            else
            {
                rotate(Axis.X, (int)midPos.x, 2);
            }
        }

        private void whiteEdgePositionMiddle(Piece whiteEdge, Vector3 targetPos, Vector3 startPos)
        {
            if (startPos.x == 1)
            {
                if (startPos.z == 1)
                {
                    if (targetPos.x == 1)
                    {
                        rotate(Axis.X, 1, 1);
                    }
                    else if (targetPos.x == -1) 
                    {
                        rotate(Axis.Y, -1, 1);
                        rotate(Axis.Z, 1, -1);
                        rotate(Axis.Y, -1, -1);
                    }
                    else if (targetPos.z == 1)
                    {
                        rotate(Axis.Z, 1, -1);
                    }
                    else if (targetPos.z == -1)
                    {
                        rotate(Axis.Y, -1, -1);
                        rotate(Axis.X, 1, 1);
                        rotate(Axis.Y, -1, 1);
                    }
                }
                else
                {
                    if (targetPos.x == 1)
                    {
                        rotate(Axis.X, 1, -1);
                    }
                    else if (targetPos.x == -1)
                    {
                        rotate(Axis.Y, -1, -1);
                        rotate(Axis.Z, -1, -1);
                        rotate(Axis.Y, -1, 1);
                    }
                    else if (targetPos.z == 1)
                    {
                        rotate(Axis.Y, -1, 1);
                        rotate(Axis.X, 1, -1);
                        rotate(Axis.Y, -1, -1);
                    }
                    else if (targetPos.z == -1)
                    {
                        rotate(Axis.Z, -1, -1);
                    }
                }
            }
            else
            {
                if (startPos.z == 1)
                {
                    if (targetPos.x == 1)
                    {
                        rotate(Axis.Y, -1, -1);
                        rotate(Axis.Z, 1, 1);
                        rotate(Axis.Y, -1, 1);
                    }
                    else if (targetPos.x == -1) 
                    {
                        rotate(Axis.X, -1, 1);
                    }
                    else if (targetPos.z == 1) 
                    {
                        rotate(Axis.Z, 1, 1);
                    }
                    else if (targetPos.z == -1)
                    {
                        rotate(Axis.Y, -1, 1);
                        rotate(Axis.X, -1, 1);
                        rotate(Axis.Y, -1, -1);
                    }
                }
                else //(-1, 1)
                {
                    if (targetPos.x == 1) 
                    {
                        rotate(Axis.Y, -1, 1);
                        rotate(Axis.Z, -1, 1);
                        rotate(Axis.Y, -1, -1);
                    }
                    else if (targetPos.x == -1) 
                    {
                        rotate(Axis.X, -1, -1);
                    }
                    else if (targetPos.z == 1) 
                    {
                        rotate(Axis.Y, -1, -1);
                        rotate(Axis.X, -1, -1);
                        rotate(Axis.Y, -1, 1);
                    }
                    else if (targetPos.z == -1) 
                    {
                        rotate(Axis.Z, -1, 1);
                    }
                }
            }
        }
        private void whiteEdgePositionBottom(Piece whiteEdge, Vector3 targetPos, Vector3 startPos)
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
        private void whiteOrientation(Piece whiteEdge)
        {
            bool good = false;
            foreach(Face f in whiteEdge.faces)
            {
                if (f.colour == Colour.White && f.direction == Vector3.down)
                {
                    good = true;
                }
            }
            if (!good)
            {
                if (whiteEdge.position.x != 0)
                {
                    if (whiteEdge.position.x == 1)
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
                    if (whiteEdge.position.z == 1)
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
        private List<Piece> getWhiteEdges()
        {
            List<Piece> whiteEdges = new List<Piece>();
            foreach(Piece p in cube.pieces)
            {
                if (p.position.ManhattanDistance() == 2 && p.containsColour(Colour.White))
                {
                    whiteEdges.Add(p);
                }
            }
            return whiteEdges;
        }
    }  
}
