using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MultiCubeAnalysers;


public class AnalysisDisplay : MonoBehaviour
{
    public TextMeshProUGUI t;
    public void Analysis()
    {
        MultiCubeAnalyser m = new MultiCubeAnalyser(10);
        t.text = m.printAnalysis();
    }
}
