using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Menu.Save;

/*
Script that holds the cube data and handles animation in the interactive cube GUI.
*/

namespace InteractiveCube
{

    public class CubeComponent : MonoBehaviour
    {
        private Cube c;

        [SerializeField]
        private bool modifiable = true;

        [SerializeField]
        private TMP_Dropdown solver;

        [SerializeField]
        private TMP_Text status;

        private CubeUpdater updater;

        private void Start()
        {
            updater = GetComponent<CubeUpdater>();
            createCube();
        }

        public bool isModifiable()
        {
            return modifiable;
        }

        public void setModifiable(bool m)
        {
            modifiable = m;
        }

        public void createCube()
        {
            c = new Cube();
            updater.spawnCube();
            updater.colourCube();
        }

        public void resetCube()
        {
            setCube(new Cube());
        }

        public Cube getCube()
        {
            return c.Clone();
        }

        public void setCube(Cube cube)
        {
            if (modifiable)
            {
                c = cube;
                updater.colourCube();
            }
        }

        public void rotateCube(Move m)
        {
            if (modifiable)
            {
                modifiable = false;
                c.rotate(m);
                StartCoroutine(updater.animateMove(m, true));
            }
        }

        public IEnumerator animate()
        {
            modifiable = false;
            CubeSolver s;
            if (solver.value == 0) { s = new LayerByLayer(c); }
            else { s = new CFOP(c); }
            yield return s.solve();
            Queue<Move> moves = s.getSolution();
            int i = 0;
            string sectionName = "";
            while (moves.Count > 0)
            {
                if (s.sections.ContainsKey(i))
                {
                    sectionName = s.sections[i];
                }
                Move m = moves.Dequeue();
                c.rotate(m);
                status.text = "Section: " + sectionName + "\nMoves left: " + moves.Count + "\nCurrent move: " + m.getNotation();
                yield return StartCoroutine(updater.animateMove(m, false));
                i++;
            }
            yield return new WaitForSeconds(1);
            status.text = "";
            modifiable = true;
        }

        public void startAnimate()
        {
            if (modifiable)
            {
                StartCoroutine(animate());
            }

        }

        public void scramble()
        {
            c.randomMoveSequence();
            updater.colourCube();
        }

        public void saveCube(int saveID)
        {
            SaveSystem.SaveCube(c, saveID);
        }

        public void LoadCube(int saveID)
        {
            setCube(SaveSystem.LoadCube(saveID));
        }
    }
}