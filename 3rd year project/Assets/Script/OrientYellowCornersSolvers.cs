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

namespace OrientYellowCornersSolvers
{
    class OrientYellowCornersSolver : CubeSolver
    {
        public OrientYellowCornersSolver(Cube cube)
        {
            this.cube = cube.Clone();
            moves = new Queue<Move>();
        }

        public override void solve()
        {
            orientYellowCorners();
        }

        private void orientYellowCorners()
        {
            List<Piece> yellowCorners = cube.filter(Colour.Yellow, 3);
            foreach (Piece yellowCorner in yellowCorners)
            {
                if (!yellowIsUp(yellowCorner))
                {
                    rotateToCorrectPosition(yellowCorner, new Vector3(-1, 1, 1));
                    while(!yellowIsUp(yellowCorner))
                    {
                        rotate("R'");
                        rotate("D'");
                        rotate("R");
                        rotate("D");

                        rotate("R'");
                        rotate("D'");
                        rotate("R");
                        rotate("D");
                    }
                }
            }
            rotateToCorrectPosition(yellowCorners[0]);
        }

        private Boolean yellowIsUp(Piece yellowCorner)
        {
            foreach (Face f in yellowCorner.faces)
            {
                if (f.colour == Colour.Yellow && f.direction == Vector3.up)
                {
                    return true;
                }
            }
            return false;
        }
    }
}