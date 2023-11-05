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
    }
    public override IEnumerator solve()
    {
        addSection("white cross");
        yield return subCubeSolver(new WhiteCrossSolver(cube));
        addSection("F2L");
        yield return subCubeSolver(new F2L(cube));
        addSection("OLL");
        yield return subCubeSolver(new OLL(cube));
        addSection("PLL");
        yield return subCubeSolver(new PLL(cube));
        yield return null;
    }
}


