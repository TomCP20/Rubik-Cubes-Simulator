using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/*
Controlls the bar chart as a whole.
barPrefab holds a prefab for the barcharts bars.
values is an array containg the values for each bar.
labels is an array containg the labels for each bar.
Their indexies are alligned so that that the corresponding label and value share the same index.
colors hold the colors used in the bar chart.
chartHeight holds the height of the chart in pixles.
loading holds a reference to the loading UI element.
DisplayGraph takes an array of values and for each of them creates a bar and sets it's: height, color, label text, and value text.
ResetGraph resets the graph.
setValues sets the value of the values variable.
setLabels sets the value of labels.
*/

namespace BarChart
{
    public class BarChart : MonoBehaviour
    {
        [SerializeField]
        private Bar barPrefab;

        [SerializeField]
        private int[] values;

        [SerializeField]
        private string[] lables;

        [SerializeField]
        private Color[] colors;

        private float chartHeight;

        [SerializeField]
        private GameObject loading;

        void Start()
        {
            chartHeight = Screen.height + GetComponent<RectTransform>().sizeDelta.y;
        }

        private void Update()
        {
            chartHeight = Screen.height + GetComponent<RectTransform>().sizeDelta.y;
        }

        public void DisplayGraph(int[] vals)
        {
            ResetGraph();
            float maxValue = vals.Max();
            for (int i = 0; i < vals.Length; i++)
            {
                Bar newBar = Instantiate(barPrefab);
                newBar.transform.SetParent(transform);
                float normalised = 0.95f * (vals[i] / maxValue);
                newBar.setBarHeight(chartHeight * normalised);
                newBar.SetBarColor(colors[i % colors.Length]);
                newBar.SetLabelText(lables.Length <= i ? "UNDEFINED" : lables[i]);
                newBar.SetBarValueText(vals[i].ToString());
            }
            loading.SetActive(false);
        }

        public void DisplayGraph()
        {
            DisplayGraph(values);
        }

        public void ResetGraph()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
        
        public void setValues(int[] newValues)
        {
            values = newValues;
        }

        public void setLabels(string[] newLables)
        {
            lables = newLables;
        }
    }
}