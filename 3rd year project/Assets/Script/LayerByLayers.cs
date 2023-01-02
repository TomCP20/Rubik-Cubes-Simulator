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
using WhiteCornersSolvers;
using MiddleLayerSolvers;
using YellowCrossSolvers;
using YellowEdgesSolvers;
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

            subCubeSolver(new WhiteCornersSolver(cube));

            subCubeSolver(new MiddleLayerSolver(cube));

            subCubeSolver(new YellowCrossSolver(cube));

            subCubeSolver(new YellowEdgesSolver(cube));
        }

        

        private void permuteYellowCorners()
        {

        }

        private void orientYellowCorners()
        {

        }
    }  
}