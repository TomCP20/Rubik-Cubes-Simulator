using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CubeAnalyser
{
    private Cube c;
    private Queue<Move> moves;

    private int type;

    public float[] count;

    public float[] moveCounts;


    public IEnumerator solveCube(int type)
    {
        Cube c = new Cube();
        c.randomMoveSequence();
        CubeSolver s;
        if (type == 0)
        {
            s = new LayerByLayer(c);
        }
        else
        {
            s = new CFOP(c);
        }
        yield return s.solve();
        moves = s.getSolution();
        count = new float[] {MoveCounter.getHTM(moves), MoveCounter.getQTM(moves), MoveCounter.getSTM(moves), MoveCounter.getQSTM(moves), MoveCounter.getATM(moves), (float)MoveCounter.get15HTM(moves)};
        yield return null;
    }

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