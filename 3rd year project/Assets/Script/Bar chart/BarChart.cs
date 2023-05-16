using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BarChart : MonoBehaviour
{
    public Bar barPrefab;

    private List<Bar> bars = new List<Bar>();

    public float[] values = {3f, 20f, 17f, 54f};

    public string[] lables = {"1", "2", "3", "4"};

    public Color[] colors;
    
    private float chartHeight;

    void Start()
    {
        chartHeight = Screen.height + GetComponent<RectTransform>().sizeDelta.y;
        DisplayGraph(values);
    }

    public void DisplayGraph(float[] vals)
    {
        float maxValue = vals.Max();
        for (int i = 0; i < vals.Length; i++)
        {
            Bar newBar = Instantiate(barPrefab) as Bar;
            newBar.transform.SetParent(this.transform);
            RectTransform rt = newBar.bar.GetComponent<RectTransform>();
            float normalised = (vals[i]/maxValue) * 0.95f;
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, chartHeight * normalised);

            newBar.bar.color = colors[i % colors.Length];

            if (lables.Length <= i) { newBar.label.text = "UNDEFINED"; }
            else { newBar.label.text = lables[i]; }
            newBar.barValue.text = vals[i].ToString();   
        }
    }
}
