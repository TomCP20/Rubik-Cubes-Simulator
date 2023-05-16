using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;


public class AnalysisDisplay : MonoBehaviour
{
    private int cubeNo;
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private TMP_Dropdown d;

    private float[][] results;

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


    public void StartAnalysis()
    {
        StartCoroutine(Analysis2());
    }
    
    public IEnumerator Analysis()
    {
        cubeNo = 100;
        type = d.value;
        yield return getResults(type);
        float[] avgs = calcAvg(results);
        float[] standardDeviation = calcSD(avgs, results);
        text.text = getOutput(avgs, standardDeviation);
        yield return null;
    }

    public IEnumerator Analysis2()
    {
        CubeAnalyser CA = new CubeAnalyser();
        yield return CA.SampleMoveCount(0, 0, 1000);
        float[] sample1 = CA.moveCounts;
        yield return CA.SampleMoveCount(1, 0, 1000);
        float[] sample2 = CA.moveCounts;
        text.text = TTest(sample1, sample2).ToString();
        yield return null;
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

    private IEnumerator getResults(int type)
    {
        float[][] r = new float[cubeNo][];

        for (int i = 0; i < cubeNo; i++)
        {
            CubeAnalyser c = new CubeAnalyser();
            yield return c.solveCube(type);
            r[i] = c.count;
        }
        results = r;
    }

    private float TTest(float[] sample1, float[] sample2)
    {
        float meanDelta = sample1.Average() - sample2.Average();
        float standardErrorDelta = Mathf.Sqrt(Mathf.Pow(StandardError(sample1), 2) + Mathf.Pow(StandardError(sample2), 2));
        Debug.Log(sample1.Average());
        Debug.Log(sample2.Average());
        float t = meanDelta/standardErrorDelta;
        return t;
    }

    private float StandardDeviation(float[] sample)
    {
        float variance = 0;
        for (int j = 0; j < 6; j++)
        {
            variance += Mathf.Pow((sample[j] - sample.Average()), 2);
        }
        variance /= sample.Length;
        return Mathf.Sqrt(variance);
    }

    private float StandardError(float[] sample)
    {
        return StandardDeviation(sample)/Mathf.Sqrt(sample.Length);
    }
}
