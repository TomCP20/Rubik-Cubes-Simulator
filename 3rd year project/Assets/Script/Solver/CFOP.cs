using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using ExtensionMethods;

class CFOP : CubeSolver
{
    public CFOP(Cube cube)
    {
        this.cube = cube.Clone();
        moves = new Queue<Move>();
    }
    public override IEnumerator solve()
    {
        moves = new Queue<Move>();
        //UnityEngine.Debug.Log("white cross");
        subCubeSolver(new WhiteCrossSolver(cube));
        //UnityEngine.Debug.Log("F2L");
        subCubeSolver(new F2L(cube));
        subCubeSolver(new OLL(cube));
        subCubeSolver(new PLL(cube));
        yield return null;
    }
}


