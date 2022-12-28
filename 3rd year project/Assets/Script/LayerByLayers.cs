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

            middleLayer();
        }

        private void whiteCorners()
        {
            List<Piece> whiteCorners = cube.filter(Colour.White, 3);
            foreach(Piece whiteCorner in whiteCorners)
            {
                if (whiteCorner.position.y == -1)
                {
                    whiteCornerBottom(whiteCorner);
                }
                else
                {
                    whiteCornerTop(whiteCorner);
                }
            }
        }

        private void whiteCornerBottom(Piece whiteCorner)
        {
            if (whiteCorner.position != whiteCorner.SolvedPosition())
            {
                int shiftVal = getShiftVal(whiteCorner.position);
                shiftRotate(shiftVal, "R");
                rotate("U");
                shiftRotate(shiftVal, "R'");
                whiteCornerTop(whiteCorner);
            }  
        }

        private void whiteCornerTop(Piece whiteCorner)
        {

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