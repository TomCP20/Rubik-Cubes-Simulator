using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ValuesCalculator : MonoBehaviour
{

    public TMP_Dropdown solver;

    public TMP_Dropdown metric;

    public int sampleSize = 1000;

    public const int BucketSize = 10;

    private BarChart chart;

    private void Start()
    {
        chart = GetComponent<BarChart>();
    }
    public void StartCalculation()
    {
        StartCoroutine(Calculate());
    }

    
    IEnumerator Calculate()
    {
        Debug.Log("Starting barchart calculation");
        CubeAnalyser CA = new CubeAnalyser();
        yield return CA.SampleMoveCount(solver.value, metric.value, sampleSize);
        float[] moveCounts = CA.moveCounts;
        int bucketCount = getBucketCount(moveCounts);
        int minBucket = Mathf.FloorToInt(moveCounts.Min()/(float)BucketSize);
        Debug.Log("finnishing barchart calculation");
        Debug.Log("Displaying Values");
        chart.values = getVals(minBucket, bucketCount, moveCounts);
        chart.lables = getLabels(minBucket, bucketCount);
        chart.DisplayGraph();
        yield return null;
        
    }
    

    public int[] getVals(int minBucket, int bucketCount, float[] moveCounts)
    {
        int[] vals = new int[bucketCount - minBucket + 1];
        for (int i = 0; i < sampleSize; i++)
        {
            int bucket = Mathf.FloorToInt(moveCounts[i]/BucketSize);
            vals[bucket - minBucket]++;
        }
        return vals;
    }

    public string[] getLabels(int minBucket, int bucketCount)
    {
        string[] lables = new string[bucketCount - minBucket + 1];
        for (int i = minBucket; i <= bucketCount; i++)
        {
            lables[i - minBucket] = (i*BucketSize).ToString() + "-" + ((i+1)*BucketSize-1).ToString();
        }
        return lables;
    }

    public int getBucketCount(float[] moveCounts)
    {
        float maxMoveCount = moveCounts.Max();
        int bucketCount = Mathf.FloorToInt((float)maxMoveCount/(float)BucketSize);
        if (maxMoveCount % BucketSize == 0)
        {
            bucketCount++;
            Debug.Log("hello");
        }
        return bucketCount;
    }
}