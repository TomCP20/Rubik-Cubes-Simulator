using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MovesNos;
using CubeAnalysers;

namespace MultiCubeAnalysers
{
    public class MultiCubeAnalyser
    {
        int cubeNo;
        MovesNo[] results;

        public double avgHTM;
        public double avgQTM;
        public double avgSTM;
        public double avgQSTM;
        public double avgATM;
        public double avghalfHTM;

        public MultiCubeAnalyser(int cubeNo)
        {
            this.cubeNo = cubeNo;
            avgHTM = 0;
            avgQTM = 0;
            avgSTM = 0;
            avgQSTM = 0;
            avgATM = 0;
            avghalfHTM = 0;
            results = new MovesNo[cubeNo];
            for (int i = 0; i < cubeNo; i++)
            {
                CubeAnalyser c = new CubeAnalyser();
                results[i] = c.getCount();
                avgHTM += results[i].HTM;
                avgQTM += results[i].QTM;
                avgSTM += results[i].STM;
                avgQSTM += results[i].QSTM;
                avgATM += results[i].ATM;
                avghalfHTM += results[i].halfHTM;
                UnityEngine.Debug.Log(results[i].QTM);
            }
            avgHTM = avgHTM/cubeNo;
            avgQTM = avgQTM/cubeNo;
            avgSTM = avgSTM/cubeNo;
            avgQSTM = avgQSTM/cubeNo;
            avgATM = avgATM/cubeNo;
            avghalfHTM = avghalfHTM/cubeNo;
        }

        public void printAnalysis() //TODO add multi threading
        {
            UnityEngine.Debug.Log("Avearge HTM of: " + avgHTM);
            UnityEngine.Debug.Log("Avearge QTM of: " + avgQTM);
            UnityEngine.Debug.Log("Avearge STM of: " + avgSTM);
            UnityEngine.Debug.Log("Avearge QSTM of: " + avgQSTM);
            UnityEngine.Debug.Log("Avearge ATM of: " + avgATM);
            UnityEngine.Debug.Log("Avearge 1.5HTM of: " + avghalfHTM);
        }
    }
}
