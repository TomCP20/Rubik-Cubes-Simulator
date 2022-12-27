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
        private Queue<Move> moves;

        public CubeSolver(Cube cube)
        {
            this.cube = cube.Clone();
        }

        private void rotate(Axis axis, int slice, int quarterTurns)
        {
            rotate(new Move(axis, slice, quarterTurns));
        }

        private void rotate(Move move)
        {
            cube.rotate(move);
            moves.Enqueue(move);
        }

        public Queue<Move> getSolution()
        {
            solve();
            return moves;
        }
        public Cube getSlovedCube() // for testing
        {
            solve();
            return cube;
        }
        private void solve()
        {
            moves = new Queue<Move>();
            whiteCross();
        }

        private string shiftFace(int shiftVal, string face)
        {
            string sideFaces = {"F", "R", "B", "L"};
            int pos = IndexOf(sideFaces, face);
            return sideFaces[(pos - shiftVal) % 4];
        }

        private shiftRotate(int shiftVal, string notation)
        {
            string face = shiftFace(shiftVal, notation.Substring(0, 1));
            string angle = notation.Substring(1);
            rotate(new Move(face + angle));
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
                case -1: // piece on bottom layer
                    whiteEdgePositionBottom(whiteEdge, targetPos, startPos);
                    break;          
                case 0: // piece on middle layer
                    whiteEdgePositionMiddle(whiteEdge, targetPos, startPos);
                    break;
                case 1: // piece on top layer
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
                case 1: //piece is above the solved position
                    UnityEngine.Debug.Log("solved");
                    break;
                case -1: //piece is above the oposite position
                    UnityEngine.Debug.Log("oposite");
                    rotate(Axis.Y, 1, 2);
                    break;
                case 0: //piece is above a diagonal position
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
            if (midPos.x == 0) // rotate face to go from top layer to bottom layer
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
                if (startPos.z == 1) // (1, 1)
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
                else  // (1, -1)
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
                if (startPos.z == 1)  // (-1, 1)
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
                else //(-1, -1)
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
            else if (startPos != targetPos) //if its not oposite and its not solved then the pice must be diagonal from its goal position, therefore swap the piece
            {
                swapBottomEdges(startPos, targetPos);
            }
        }

        private void swapBottomEdges(Vector3 edge1, Vector3 edge2)
        {
            Axis axis;
            int sumx = (int) (edge1.x + edge2.x);
            int sumz = (int) (edge1.z + edge2.z);
            if (sumx*sumz == 1) { axis = Axis.Z; }
            else { axis = Axis.X; }
            rotate(axis, sumx, sumx);
            rotate(Axis.Y, -1, -1);
            rotate(axis, sumx, -sumx);
            rotate(Axis.Y, -1, 1);
            rotate(axis, sumx, sumx);
        }

        private void whiteOrientation(Piece whiteEdge)
        {
            if (checkWhiteOrientation(whiteEdge)) { return; }
            Axis axis1;
            Axis axis2 ;
            int pos1;
            int pos2;
            if (whiteEdge.position.x != 0)
            {
                axis1 = Axis.X;
                axis2 = Axis.Z;
                pos1 = (int)whiteEdge.position.x;
                pos2 = -pos1;
            }
            else
            {
                axis1 = Axis.Z;
                axis2 = Axis.X;
                pos1 = (int)whiteEdge.position.z;
                pos2 = pos1;
            }
            rotate(axis1, pos1, pos1);
            rotate(Axis.Y, -1, 1);
            rotate(axis2, pos2, pos2);
            rotate(Axis.Y, -1, -1);            
        }

        private bool checkWhiteOrientation(Piece whiteEdge)
        {
            foreach(Face f in whiteEdge.faces)
            {
                if (f.colour == Colour.White && f.direction == Vector3.down)
                {
                    return true;
                }
            }
            return false;
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