using System;
using UnityEngine;
using CubeNamespace;
using System.Collections.Specialized;

public class CubeController : MonoBehaviour
{
    Cube c;
    bool cubeAltered;
    public Material BlackMat;
    public Material WhiteMat;
    public Material GreenMat;
    public Material BlueMat;
    public Material RedMat;
    public Material YellowMat;
    public Material OrangeMat;

    // Start is called before the first frame update
    void Start()
    {
        
        c = new Cube();
        updateCube(c);
        //c.rotate(Axis.X, 1, 1);
        //updateCube(c);
        //c.rotate(Axis.Y, 1, 2);
        //updateCube(c);
        //cubeAltered = true;
        //InvokeRepeating("RotCube", 1.0f, 1.0f);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cubeAltered) { updateCube(c); }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                cubeAltered = true;
                Vector3 pos = hit.transform.localPosition;
                Debug.Log(hit.transform.name);
                switch(hit.transform.name)
                {
                    case ("Quad 23"):
                        c.rotate(Axis.Y, 1, 1);
                        break;
                    case ("Quad 32"):
                        c.rotate(Axis.Y, -1, 1);
                        break;
                    case ("Quad 5"):
                        c.rotate(Axis.X, 1, 1);
                        break;
                    case ("Quad 14"):
                        c.rotate(Axis.X, -1, 1);
                        break;
                    case ("Quad 41"):
                        c.rotate(Axis.Z, 1, 1);
                        break;
                    case ("Quad 50"):
                        c.rotate(Axis.Z, -1, 1);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    void RotCube()
    {
        c.rotate(Axis.X, 1, 1);
        cubeAltered = true;
    }

    void updateCube(Cube c)
    {
        foreach (Pice p in c.pices)
        {
            foreach (Face f in p.faces)
            {
                Vector3 coords = p.position + f.direction/2;
                Collider[] hitColliders = Physics.OverlapSphere(coords, 0.1f);
                if (hitColliders.Length.Equals(1))
                {
                    //UnityEngine.Debug.Log(hitColliders[0]);
                    hitColliders[0].GetComponent<MeshRenderer>().material = GetMaterial(f.colour);
                }
                else
                {
                    UnityEngine.Debug.Log("colliders != 1");
                }
            }
        }
        cubeAltered = false;
    }

    Material GetMaterial(Colour c)
    {
        switch(c)
        {
            case Colour.Blue:
                return BlueMat;
            case Colour.Green:
                return GreenMat;
            case Colour.Red:
                return RedMat;
            case Colour.Yellow:
                return YellowMat;
            case Colour.Orange:
                return OrangeMat;
            case Colour.White:
                return WhiteMat;
            case Colour.Black:
                return BlackMat;
            default:
                return null;
        }
    }
}
