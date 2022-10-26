using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;

    float rotationSpeed = 7.0F;

    void Update()
    {
        float Xrot = Input.GetAxis("Horizontal"); 
        float Yrot = Mathf.Clamp(Input.GetAxis("Vertical"), -90, 90);
        transform.LookAt(target.transform);
        transform.Translate(new Vector3(Xrot, Yrot, 0) * Time.deltaTime * rotationSpeed);
    }
}
