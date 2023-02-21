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

namespace OLLs
{
    class OLL : CubeSolver
    {
        public OLL(Cube cube)
        {
            this.cube = cube.Clone();
            moves = new Queue<Move>();
        }

        public override void solve()
        {
            List<Piece> yellowEdges = cube.filter(Colour.Yellow, 2);
            int count = countYellowUp(yellowEdges);
            if (count == 0)
            {

            }
            else if (count == 2)
            {
                
            }
        }

        public bool isFacingUp(Piece p)
        {
            return p.getFaceByColour(Colour.Yellow).direction == Vector3.up;
        }

        public int countYellowUp(List<Piece> yellowEdges)
        {
            int count = 0;
            foreach (Piece p in yellowEdges)
            {
                if (isFacingUp(p)) { count++; }
            }
            return count;
        }
    }
}