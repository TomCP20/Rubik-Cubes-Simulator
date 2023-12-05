using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CubeAnalyser
{
    private Queue<Move> moves;

    public float[] moveCounts;

    public IEnumerator SampleMoveCount(int solverType, int metricType, int sampleSize)
    {
        moveCounts = new float[sampleSize];
        for (int i = 0; i < sampleSize; i++)
        {
            Cube c = new Cube();
            c.randomMoveSequence();
            CubeSolver s;
            if (solverType == 0) { s = new LayerByLayer(c); }
            else { s = new CFOP(c); }
            yield return s.solve();
            moves = s.getSolution();
            moveCounts[i] = MoveCounter.countMoves(moves, metricType);
        }
    }
}