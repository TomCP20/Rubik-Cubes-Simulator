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

namespace WhiteCrossSolvers
{
    class WhiteCrossSolver : CubeSolver
    {
        public WhiteCrossSolver(Cube cube)
        {
            this.cube = cube.Clone();
            moves = new Queue<Move>();
        }

        public override void solve()
        {
            whiteCross();
        }

        private void whiteCross()
        {
            List<Piece> whiteEdges = cube.filter(Colour.White, 2);;
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
        } 

        private void whiteEdgePositionTop(Piece whiteEdge, Vector3 targetPos, Vector3 startPos)
        {
            Vector3 midPos;
            switch (startPos.x*targetPos.x + startPos.z * targetPos.z)
            {
                case 1: //piece is above the solved position
                    break;
                case -1: //piece is above the oposite position
                    rotate(Axis.Y, 1, 2);
                    break;
                case 0: //piece is above a diagonal position
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
                    break;
            }

            midPos = whiteEdge.position;
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
            if (whiteEdge.correctOrientation()) { return; }
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
    }
}
