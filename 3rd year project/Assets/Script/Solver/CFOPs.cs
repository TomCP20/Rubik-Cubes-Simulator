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
using WhiteCrossSolvers;
using F2Ls;
using OLLs;
using PLLs;

namespace CFOPs
{
    class CFOP : CubeSolver
    {
        public CFOP(Cube cube)
        {
            this.cube = cube.Clone();
            moves = new Queue<Move>();
        }

        public override void solve()
        {
            moves = new Queue<Move>();
            //UnityEngine.Debug.Log("white cross");
            subCubeSolver(new WhiteCrossSolver(cube));
            //UnityEngine.Debug.Log("F2L");
            subCubeSolver(new F2L(cube));

            subCubeSolver(new OLL(cube));

            subCubeSolver(new PLL(cube));
        }
    }
}


