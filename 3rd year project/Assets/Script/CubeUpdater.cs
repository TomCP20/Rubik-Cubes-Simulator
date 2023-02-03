using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cubes;
using Pieces;
using Faces;

public class CubeUpdater : MonoBehaviour
{
    public CubeVariable CubeState;

    public Material BlackMat;
    public Material WhiteMat;
    public Material GreenMat;
    public Material BlueMat;
    public Material RedMat;
    public Material YellowMat;
    public Material OrangeMat;

    public void updateCube()
    {
        foreach (Piece p in CubeState.CubeValue.pieces)
        {
            foreach (Face f in p.faces)
            {
                Vector3 coords = p.position + f.direction / 2;
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
    }


    Material GetMaterial(Colour col)
    {
        switch (col)
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
