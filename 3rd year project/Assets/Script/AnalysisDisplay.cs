using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MultiCubeAnalysers;


public class AnalysisDisplay : MonoBehaviour
{
    public void Analysis()
    {
        MultiCubeAnalyser m = new MultiCubeAnalyser(10);
        m.printAnalysis();
    }
}
