using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class GenerateData : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(SampleMoveCount(0, 0, 1000, "LBL-FTM"));
    }

    public IEnumerator SampleMoveCount(int solverType, int metricType, int sampleSize, string name)
    {
        Debug.Log("Generating and saving data");
        float[] moveCounts = new float[sampleSize];
        for (int i = 0; i < sampleSize; i++)
        {
            Cube c = new Cube();
            c.randomMoveSequence();
            CubeSolver s;
            if (solverType == 0) { s = new LayerByLayer(c); }
            else { s = new CFOP(c); }
            yield return s.solve();
            Queue<Move> moves = s.getSolution();
            moveCounts[i] = MoveCounter.countMoves(moves, metricType);
        }
        string path = Application.persistentDataPath + "/" + name + ".csv";
        File.WriteAllText(path, String.Join("\n", moveCounts));
        Debug.Log("Done saving data");
    }
}
