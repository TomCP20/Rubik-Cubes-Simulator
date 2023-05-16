using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BarChart : MonoBehaviour
{
    public Bar barPrefab;

    private List<Bar> bars = new List<Bar>();

    public int[] values;

    public string[] lables;

    public Color[] colors;
    
    private float chartHeight;

    void Start()
    {
        chartHeight = Screen.height + GetComponent<RectTransform>().sizeDelta.y;
        //DisplayGraph(values);
    }

    public void DisplayGraph(int[] vals)
    {
        ResetGraph();
        float maxValue = vals.Max();
        for (int i = 0; i < vals.Length; i++)
        {
            Bar newBar = Instantiate(barPrefab) as Bar;
            newBar.transform.SetParent(this.transform);
            RectTransform rt = newBar.bar.GetComponent<RectTransform>();
            float normalised = ((float)vals[i]/(float)maxValue) * 0.95f;
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, chartHeight * normalised);

            newBar.bar.color = colors[i % colors.Length];

            if (lables.Length <= i) { newBar.label.text = "UNDEFINED"; }
            else { newBar.label.text = lables[i]; }
            newBar.barValue.text = vals[i].ToString();   
        }
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
}
