using System.Collections.Generic;
using System.IO;
using System;

class GenerateData
{
    static void SampleMoveCount(int solverType, int metricType, int sampleSize, string name)
    {
        Console.WriteLine($"Generating and saving data for {name}.");
        float[] moveCounts = new float[sampleSize];
        for (int i = 0; i < sampleSize; i++)
        {
            Cube c = new Cube();
            c.randomMoveSequence();
            CubeSolver s;
            if (solverType == 0) { s = new LayerByLayer(c); }
            else { s = new CFOP(c); }
            s.solve();
            Queue<Move> moves = s.getSolution();
            moveCounts[i] = MoveCounter.countMoves(moves, metricType);
        }
        string path = $"/data/{name}.csv";
        File.WriteAllText(path, string.Join("\n", moveCounts));
        Console.WriteLine("Done saving data.");
    }

    public static void Main()
    {
        SampleMoveCount(0, 0, 1000, "LBL-FTM");
        SampleMoveCount(0, 1, 1000, "LBL-QTM");
        SampleMoveCount(0, 2, 1000, "LBL-STM");
        SampleMoveCount(0, 3, 1000, "LBL-QSTM");
        SampleMoveCount(0, 4, 1000, "LBL-ATM");
        SampleMoveCount(0, 5, 1000, "LBL-15HTM");
        SampleMoveCount(1, 0, 1000, "CFOP-FTM");
        SampleMoveCount(1, 1, 1000, "CFOP-QTM");
        SampleMoveCount(1, 2, 1000, "CFOP-STM");
        SampleMoveCount(1, 3, 1000, "CFOP-QSTM");
        SampleMoveCount(1, 4, 1000, "CFOP-ATM");
        SampleMoveCount(1, 5, 1000, "CFOP-15HTM");
    }
}
