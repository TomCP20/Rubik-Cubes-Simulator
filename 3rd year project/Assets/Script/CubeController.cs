using System;
using UnityEngine;
using LayerByLayers;
using CubeSolvers;
using Faces;
using Pieces;
using System.Collections.Specialized;
using System.Collections;
using Cubes;

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
        //InvokeRepeating("RotCube", 1.0f, 1.0f);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cubeAltered) { updateCube(c); }
        if (Input.GetKeyDown("r"))
        {
            cubeAltered = true;
            c.randomMoveSequence();
        }
        if (Input.GetKeyDown("l"))
        {
            StartCoroutine(solve(c));
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) { rotateFace(hit.transform.name, 1); }
            
        }
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) { rotateFace(hit.transform.name, -1); }
            
        }
    }

    void rotateFace(String name, int direction)
    {
        String move;
        switch(name)
        {
            case ("Quad 23"):
                move = "U";
                break;
            case ("Quad 32"):
                move = "D";
                break;
            case ("Quad 5"):
                move = "L";
                break;
            case ("Quad 14"):
                move = "R";
                break;
            case ("Quad 41"):
                move = "F";
                break;
            case ("Quad 50"):
                move = "B";
                break;
            default:
                return;
        }
        if (direction == -1) { move += "'"; }
        c.rotate(move);
        UnityEngine.Debug.Log(move);
        cubeAltered = true;
    }

    IEnumerator solve(Cube c)
    {
        CubeSolver solver = new LayerByLayer(c);
        c = solver.getSlovedCube();
        cubeAltered = true;
        yield return null;
    }
    void RotCube()
    {
        c.rotate(Axis.X, 1, 1);
        cubeAltered = true;
    }

    void updateCube(Cube c)
    {
        foreach (Piece p in c.pieces)
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
