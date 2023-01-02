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

namespace PermuteYellowCornersSolvers
{
    class PermuteYellowCornersSolver : CubeSolver
    {
        public PermuteYellowCornersSolver(Cube cube)
        {
            this.cube = cube.Clone();
            moves = new Queue<Move>();
        }

        public override void solve()
        {
            permuteYellowCorners();
        }

        private void permuteYellowCorners()
        {
            List<Piece> yellowCorners = cube.filter(Colour.Yellow, 3);
            Piece solved = getSolvedCorner(yellowCorners);
            for (int i = 0; i < 2; i++)
            {
                if (!correctCorners(yellowCorners))
                {
                    cornersAlgorithm(getShiftVal(solved.position));
                }
            }
            if (!correctCorners(yellowCorners))
            {
                UnityEngine.Debug.LogError("corner out of place");
            }
        }

        private void cornersAlgorithm(int shiftVal)
        {
            rotate("U");
            shiftRotate(shiftVal, "R");
            rotate("U'");
            shiftRotate(shiftVal, "L'");
            rotate("U");
            shiftRotate(shiftVal, "R'");
            rotate("U'");
            shiftRotate(shiftVal, "L");
        }

        private Piece getSolvedCorner(List<Piece> yellowCorners)
        {
            foreach (Piece yellowCorner in yellowCorners)
            {
                if (yellowCorner.correctPosition())
                {
                    return yellowCorner;
                }
            }
            cornersAlgorithm(0);
            return getSolvedCorner(yellowCorners);
        }

        private Boolean correctCorners(List<Piece> yellowCorners)
        {
            foreach (Piece yellowCorner in yellowCorners)
            {
                if (!yellowCorner.correctPosition()) { return false; }
            }
            return true;
        }
    }
}


