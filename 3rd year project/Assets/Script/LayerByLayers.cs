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
using ExtensionMethods;

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

            subCubeSolver(new WhiteCrossSolver(cube));

            UnityEngine.Debug.Log("working on White Corners");

            whiteCorners();
        }

        private void whiteCorners()
        {
            List<Piece> whiteCorners = cube.filter(Colour.White, 3);
            foreach(Piece whiteCorner in whiteCorners)
            {
                whiteCornerFun(whiteCorner);
            }
        }

        private void whiteCornerFun(Piece whiteCorner)
        {
            if (whiteCorner.position.y == -1 && !whiteCorner.correctPosition())
            {
                GoodLayerWrongPosition(whiteCorner);
            }
            else
            {
                rotateToCorrectPosition(whiteCorner);
            }

            Boolean solved = whiteCorner.correctOrientation();
            int count = 0;
            while (!solved)
            {
                int shiftVal = getShiftVal(whiteCorner.position);
                shiftRotate(shiftVal, "R");
                rotate("U");
                shiftRotate(shiftVal, "R'");
                rotate("U'");

                solved = whiteCorner.correctOrientation();
                count++;
                if (count >= 10)
                {
                    UnityEngine.Debug.LogError("loop took too long");
                    break;
                }
            }
            if (count < 10)
            {
                UnityEngine.Debug.Log("loop succsess");
            }
        }

        private void rotateToCorrectPosition(Piece whiteCorner)
        {
            Vector3 currentPos = whiteCorner.position;
            Vector3 targetPos = whiteCorner.SolvedPosition();
            Vector3 rot1 = Vector3Int.RoundToInt(Quaternion.Euler(0, 90, 0) * currentPos);
            Vector3 rot2 = Vector3Int.RoundToInt(Quaternion.Euler(0, 180, 0) * currentPos);
            Vector3 rot3 = Vector3Int.RoundToInt(Quaternion.Euler(0, -90, 0) * currentPos);
            if (correctXZ(rot1, targetPos))
            {
                rotate("U");
            }
            else if (correctXZ(rot2, targetPos))
            {
                rotate("U2");
            }
            else if (correctXZ(rot3, targetPos))
            {
                rotate("U'");
            }
            else if (!correctXZ(currentPos, targetPos))
            {
                UnityEngine.Debug.LogError("rotation error");
            }
        }

        private bool correctXZ(Vector3 current, Vector3 target)
        {
            return (current.x == target.x && current.z == target.z);
        }
        private void GoodLayerWrongPosition(Piece whiteCorner)
        {
            int shiftVal = getShiftVal(whiteCorner.position);
            shiftRotate(shiftVal, "R'");
            rotateToCorrectPosition(whiteCorner);
            shiftRotate(shiftVal, "R"); 
        }

        private void middleLayer()
        {

        }

        private void yellowCross()
        {

        }

        private void yellowEdges()
        {

        }

        private void permuteYellowCorners()
        {

        }

        private void orientYellowCorners()
        {

        }
    }  
}