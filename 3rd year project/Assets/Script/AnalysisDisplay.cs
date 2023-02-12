using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MovesNos;
using CubeAnalysers;


public class AnalysisDisplay : MonoBehaviour
{
    private int cubeNo;
    public TextMeshProUGUI t;
    public void Analysis()
    {
        cubeNo = 100;
        float[] avgs = calcAvg();
        t.text = getOutput(avgs);
    }

    private float[] calcAvg()
    {
        float[] sums = new float[6];
        float[] avgs = new float[6];
        float[][] results = getResults();
        for (int i = 0; i < cubeNo; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                sums[j] += results[i][j];
            }
        }
        for (int j = 0; j < 6; j++)
        {
            avgs[j] += sums[j]/cubeNo;
        }
        return avgs;
    }

    private string getOutput(float[] avgs)
    {
        string output = "";
        output += "Avearge HTM of: " + avgs[0] + "\n";
        output += "Avearge QTM of: " + avgs[1] + "\n";
        output += "Avearge STM of: " + avgs[2] + "\n";
        output += "Avearge QSTM of: " + avgs[3] + "\n";
        output += "Avearge ATM of: " + avgs[4] + "\n";
        output += "Avearge 1.5HTM of: " + avgs[5] + "\n";
        return output;
    }

    private float[][] getResults()
    {
        float[][] results = new float[cubeNo][];

        for (int i = 0; i < cubeNo; i++)
        {
            CubeAnalyser c = new CubeAnalyser();
            results[i] = c.getCount();
        }
        return results;
    }
}
