using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cubes;
using Pieces;
using Faces;

public class CubeUpdater : MonoBehaviour
{
    public CubeVariable CubeState;
    public Material[] materials; 

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
                    hitColliders[0].GetComponent<MeshRenderer>().material = materials[(int)f.colour];
                }
                else
                {
                    UnityEngine.Debug.Log("colliders != 1");
                }
            }
        }
    }
}
