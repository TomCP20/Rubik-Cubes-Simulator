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
using PermuteYellowCornersSolvers;
using OrientYellowCornersSolvers;

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
            //UnityEngine.Debug.Log("white cross");
            subCubeSolver(new WhiteCrossSolver(cube));
            //UnityEngine.Debug.Log(moves.Count);
            //UnityEngine.Debug.Log("white corners");
            subCubeSolver(new WhiteCornersSolver(cube));
            //UnityEngine.Debug.Log(moves.Count);
            //UnityEngine.Debug.Log("middle layer");
            subCubeSolver(new MiddleLayerSolver(cube));
            //UnityEngine.Debug.Log(moves.Count);
            //UnityEngine.Debug.Log("yellow cross");
            subCubeSolver(new YellowCrossSolver(cube));
            //UnityEngine.Debug.Log(moves.Count);
            //UnityEngine.Debug.Log("yellow edges");
            subCubeSolver(new YellowEdgesSolver(cube));
            //UnityEngine.Debug.Log(moves.Count);
            //UnityEngine.Debug.Log("permute yellow corners");
            subCubeSolver(new PermuteYellowCornersSolver(cube));
            //UnityEngine.Debug.Log(moves.Count);
            //UnityEngine.Debug.Log("orient yellow corners");
            subCubeSolver(new OrientYellowCornersSolver(cube));
            //UnityEngine.Debug.Log(moves.Count);
        }
        

        
    }  
}