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
    }
    public override IEnumerator solve()
    {
        Debug.Log("Layer By Layer");
        addSection("white cross");
        yield return subCubeSolver(new WhiteCrossSolver(cube));
        addSection("white Corners");
        yield return subCubeSolver(new WhiteCornersSolver(cube));
        addSection("Middle Layer");
        yield return subCubeSolver(new MiddleLayerSolver(cube));
        addSection("Yellow Cross");
        yield return subCubeSolver(new YellowCrossSolver(cube));
        addSection("Yellow Edges");
        yield return subCubeSolver(new YellowEdgesSolver(cube));
        addSection("Permute Yellow Corners");
        yield return subCubeSolver(new PermuteYellowCornersSolver(cube));
        addSection("Orient Yellow Corners");
        yield return subCubeSolver(new OrientYellowCornersSolver(cube));
        yield return null;
    }
}  
