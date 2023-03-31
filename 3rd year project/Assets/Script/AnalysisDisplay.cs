using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class AnalysisDisplay : MonoBehaviour
{
    private int cubeNo;
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private TMP_Dropdown d;

    int type;

    private void Start()
    {
        type = 0;
        text = GameObject.Find("output (TMP)").GetComponent<TextMeshProUGUI>();
        d = GameObject.Find("Dropdown").GetComponent<TMP_Dropdown>();
    }

    public void updateType(TMP_Dropdown dropdown)
    {
        type = dropdown.value;
    }
    public void Analysis()
    {
        cubeNo = 100;
        type = d.value;
        float[][] results = getResults(type);
        float[] avgs = calcAvg(results);
        float[] standardDeviation = calcSD(avgs, results);
        text.text = getOutput(avgs, standardDeviation);
    }

    private float[] calcAvg(float[][] results)
    {
        float[] sums = new float[6];
        float[] avgs = new float[6];
        
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

    private float[] calcSD(float[] avgs, float[][] results)
    {
        float[] variance = new float[6];
        float[] standardDeviation = new float[6];
        for (int i = 0; i < cubeNo; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                variance[j] += Mathf.Pow((results[i][j] - avgs[j]), 2)/cubeNo;
            }
        }
        for (int j = 0; j < 6; j++)
        {
            standardDeviation[j] = Mathf.Sqrt(variance[j]);
        }
        return standardDeviation;
    }

    private string getOutput(float[] avgs, float[] standardDeviation)
    {
        string output = "";
        output += "Avearge HTM of: " + avgs[0] + " and σ of " + standardDeviation[0] + "\n";
        output += "Avearge QTM of: " + avgs[1] + " and σ of " + standardDeviation[1] + "\n";
        output += "Avearge STM of: " + avgs[2] + " and σ of " + standardDeviation[2] + "\n";
        output += "Avearge QSTM of: " + avgs[3] + " and σ of " + standardDeviation[3] + "\n";
        output += "Avearge ATM of: " + avgs[4] + " and σ of " + standardDeviation[4] + "\n";
        output += "Avearge 1.5HTM of: " + avgs[5] + " and σ of " + standardDeviation[5] + "\n";
        return output;
    }

    private float[][] getResults(int type)
    {
        float[][] results = new float[cubeNo][];

        for (int i = 0; i < cubeNo; i++)
        {
            CubeAnalyser c = new CubeAnalyser(type);
            results[i] = c.getCount();
        }
        return results;
    }
}
