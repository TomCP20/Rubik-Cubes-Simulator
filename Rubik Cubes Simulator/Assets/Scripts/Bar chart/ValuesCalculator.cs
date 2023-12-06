using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

/*
solver is a reference to the dropdown UI element that allows the user to select which solving method they would like to use to generate the bar chart.
metric is a reference to the dropdown UI element that allows the user to select which move metric they would like to use to generate the bar chart.
sampleSize is the number of cubes to sample to create the bar chart, set to 1000 by default.
BucketSize is the size of the bucket the move counts are sorted into for the bar chart.
chart holds a reference to the BarChart component.
StartCalculation starts the Calculate Coroutine.
Calculate calculates the values then displays them to the bar chart.
getVals takes the raw data and sorts it into buckets of size BucketSize and returns an array of the values in each bucket.
getLabels generates the labels for the bar chart.
getBucketCount calculates the required number of buckets.
*/

namespace BarChart
{
    public class ValuesCalculator : MonoBehaviour 
    {

        [SerializeField]
        private TMP_Dropdown solver;

        [SerializeField]
        private TMP_Dropdown metric;

        [SerializeField]
        private int sampleSize = 1000;

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
            float[] moveCounts = new float[sampleSize];
            yield return MoveCounter.allMoveCounts(solver.value, metric.value, sampleSize, moveCounts);
            int bucketCount = getBucketCount(moveCounts);
            int minBucket = Mathf.FloorToInt(moveCounts.Min() / BucketSize);
            Debug.Log("finnishing barchart calculation");
            Debug.Log("Displaying Values");
            chart.setValues(getVals(minBucket, bucketCount, moveCounts));
            chart.setLabels(getLabels(minBucket, bucketCount));
            chart.DisplayGraph();
            yield return null;

        }


        public int[] getVals(int minBucket, int bucketCount, float[] moveCounts)
        {
            int[] vals = new int[bucketCount - minBucket + 1];
            for (int i = 0; i < sampleSize; i++)
            {
                int bucket = Mathf.FloorToInt(moveCounts[i] / BucketSize);
                vals[bucket - minBucket]++;
            }
            return vals;
        }

        public string[] getLabels(int minBucket, int bucketCount)
        {
            string[] lables = new string[bucketCount - minBucket + 1];
            for (int i = minBucket; i <= bucketCount; i++)
            {
                lables[i - minBucket] = (i * BucketSize).ToString() + "-" + ((i + 1) * BucketSize - 1).ToString();
            }
            return lables;
        }

        public int getBucketCount(float[] moveCounts)
        {
            float maxMoveCount = moveCounts.Max();
            int bucketCount = Mathf.FloorToInt(maxMoveCount / BucketSize);
            if (maxMoveCount % BucketSize == 0)
            {
                bucketCount++;
            }
            return bucketCount;
        }
    }
}