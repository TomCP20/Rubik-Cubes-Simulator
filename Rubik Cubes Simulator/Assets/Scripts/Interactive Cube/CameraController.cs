using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Script for controlling the camera in the interactive cube GUI.
Takes input through WASD and arrow keys.
*/

namespace InteractiveCube
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private GameObject target;

        [SerializeField]
        private float rotationSpeed = 7.0F;

        void Update()
        {
            float Xrot = Input.GetAxis("Horizontal");
            float Yrot = Mathf.Clamp(Input.GetAxis("Vertical"), -89, 89);
            transform.LookAt(target.transform);
            transform.Translate(new Vector3(Xrot, Yrot, 0) * Time.deltaTime * rotationSpeed);
        }
    }
}