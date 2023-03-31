using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using ExtensionMethods;

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
        subCubeSolver(new PermuteYellowCornersSolver(cube));
        subCubeSolver(new OrientYellowCornersSolver(cube));
    }
}  
