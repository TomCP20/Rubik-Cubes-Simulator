using System;
using UnityEngine;
using UnityEngine.EventSystems;

/*
Script that handles user interaction with the cube in the interactive cube GUI.
*/

namespace InteractiveCube
{
    [RequireComponent(typeof(CubeComponent))]
    public class CubeController : MonoBehaviour
    {
        [SerializeField]
        private CubeComponent cube;

        void Start()
        {
            cube = GetComponent<CubeComponent>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && cube.isModifiable() && !EventSystem.current.IsPointerOverGameObject())
            {
                userMove(1);
            }
            if (Input.GetMouseButtonDown(1) && cube.isModifiable() && !EventSystem.current.IsPointerOverGameObject())
            {
                userMove(-1);
            }
        }

        public void userMove(int direction)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out hit)) { return; }
            String move = getFace(hit.transform.position);
            if (move == "") { return; }
            if (direction == -1) { move += "'"; }
            cube.rotateCube(new Move(move));
        }

        public string getFace(Vector3 pos)
        {
            if (pos.y >= 1.4)
            {
                return "U";
            }
            else if (pos.y <= -1.4)
            {
                return "D";
            }
            else if (pos.x >= 1.4)
            {
                return "L";
            }
            else if (pos.x <= -1.4)
            {
                return "R";
            }
            else if (pos.z >= 1.4)
            {
                return "F";
            }
            else if (pos.z <= -1.4)
            {
                return "B";
            }
            else
            {
                return "";
            }
        }
    }
}