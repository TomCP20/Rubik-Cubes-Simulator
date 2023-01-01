using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cubes;
using Pieces;
using Faces;
using Moves;
using CubeSolvers;
using WhiteCrossSolvers;
using ExtensionMethods;

namespace LayerByLayers
{
    class LayerByLayer : CubeSolver
    {
        public LayerByLayer(Cube cube)
        {
            this.cube = cube.Clone();
            moves = new Queue<Move>();
        }
        public override void solve()
        {
            moves = new Queue<Move>();

            subCubeSolver(new WhiteCrossSolver(cube));

            whiteCorners();
        }

        private void whiteCorners()
        {
            List<Piece> whiteCorners = cube.filter(Colour.White, 3);
            foreach(Piece whiteCorner in whiteCorners)
            {
                whiteCornerFun(whiteCorner);
            }
        }

        private void whiteCornerFun(Piece whiteCorner)
        {
            if (!whiteCorner.correctPosition())
            {
                if (whiteCorner.position.y == -1)
                {
                    int shiftVal = getShiftVal(whiteCorner.position);
                    shiftRotate(shiftVal, "R");
                    rotateToCorrectPosition(whiteCorner);
                    shiftRotate(shiftVal, "R'");
                    rotateToCorrectPosition(whiteCorner);
                }
                else
                {
                    rotateToCorrectPosition(whiteCorner);
                }
            }

            repeatPattern(whiteCorner);      
        }

        private void repeatPattern(Piece whiteCorner)
        {
            Boolean solved = whiteCorner.correctOrientation();
            int count = 0;
            while (!solved)
            {
                int shiftVal = getShiftVal(whiteCorner.position);
                shiftRotate(shiftVal, "R");
                rotate("U");
                shiftRotate(shiftVal, "R'");
                rotate("U'");

                solved = whiteCorner.correctOrientation();
                count++;
                if (count >= 10)
                {
                    UnityEngine.Debug.LogError("loop took too long");
                    break;
                }
            }
        }

        private void middleLayer()
        {

        }

        private void yellowCross()
        {

        }

        private void yellowEdges()
        {

        }

        private void permuteYellowCorners()
        {

        }

        private void orientYellowCorners()
        {

        }
    }  
}