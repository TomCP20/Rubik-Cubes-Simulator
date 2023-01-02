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

namespace YellowCrossSolvers
{
    class YellowCrossSolver : CubeSolver
    {
        public YellowCrossSolver(Cube cube)
        {
            this.cube = cube.Clone();
            moves = new Queue<Move>();
        }

        public override void solve()
        {
            yellowCross();
        }

        private void yellowCross()
        {
            Boolean backYellow = isYellowUp(cube.getPiece(new Vector3(0, 1, -1)));
            Boolean frontYellow = isYellowUp(cube.getPiece(new Vector3(0, 1, 1)));
            Boolean leftYellow = isYellowUp(cube.getPiece(new Vector3(1, 1, 0)));
            Boolean rightYellow = isYellowUp(cube.getPiece(new Vector3(-1, 1, 0)));
            if (backYellow && frontYellow && leftYellow && rightYellow)
            {
                return;
            }
            else if (!backYellow && !frontYellow && !leftYellow && !rightYellow)
            {
                yellowDot();
            }
            else if ((backYellow && frontYellow) || (leftYellow && rightYellow))
            {
                yellowLine();
            }
            else
            {
                yellowL();
            }

        }

        private bool isYellowUp(Piece p)
        {
            foreach (Face f in p.faces)
            {
                if (f.colour == Colour.Yellow)
                {
                    if (f.direction == Vector3.up)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void yellowDot()
        {
            yellowAlgorithim(0);
            yellowL();
        }

        private void yellowL()
        {
            Boolean backYellow = isYellowUp(cube.getPiece(new Vector3(0, 1, -1)));
            Boolean frontYellow = isYellowUp(cube.getPiece(new Vector3(0, 1, 1)));
            Boolean leftYellow = isYellowUp(cube.getPiece(new Vector3(1, 1, 0)));
            Boolean rightYellow = isYellowUp(cube.getPiece(new Vector3(-1, 1, 0)));
            if (backYellow && leftYellow)
            {
                yellowAlgorithim(0);
            }
            else if (frontYellow && leftYellow)
            {
                yellowAlgorithim(1);
            }
            else if (frontYellow && rightYellow)
            {
                yellowAlgorithim(2);
            }
            else if (backYellow && rightYellow)
            {
                yellowAlgorithim(3);
            }
            yellowLine();
        }

        private void yellowLine()
        {
            Boolean backYellow = isYellowUp(cube.getPiece(new Vector3(0, 1, -1)));
            Boolean frontYellow = isYellowUp(cube.getPiece(new Vector3(0, 1, 1)));
            Boolean leftYellow = isYellowUp(cube.getPiece(new Vector3(1, 1, 0)));
            Boolean rightYellow = isYellowUp(cube.getPiece(new Vector3(-1, 1, 0)));
            if (leftYellow && rightYellow)
            {
                yellowAlgorithim(0);
            }
            else
            {
                yellowAlgorithim(1);
            }
        }

        private void yellowAlgorithim(int shiftVal)
        {
            shiftRotate(shiftVal, "F");
            shiftRotate(shiftVal, "R");
            rotate("U");
            shiftRotate(shiftVal, "R'");
            rotate("U'");
            shiftRotate(shiftVal, "F'");
        }
    }
}
