using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cubes;
using Pieces;
using Faces;
using Moves;

public class CubeUpdater : MonoBehaviour
{

    public CubeComponent cube;

    void Start()
    {
        cube = GetComponent<CubeComponent>();
    }

    public Material[] materials; 

    public void colourCube()
    {
        Cube c = cube.getCube();
        foreach (Piece p in c.pieces)
        {
            foreach (Face f in p.faces)
            {
                Vector3 coords = p.position + f.direction / 2;
                Transform face = getTransform(coords);
                face.GetComponent<MeshRenderer>().material = materials[(int)f.colour];
            }
        }
    }

    public IEnumerator animateMove(Move m)
    {
        Cube c = cube.getCube();
        Transform[] transforms = getAllTransforms(c.getLayer(m.axis, m.slice));
        GameObject pivot = new GameObject("pivot");
        pivot.transform.position = Vector3.zero;
        foreach (Transform t in transforms) { t.parent = pivot.transform; }
        float duration = getDuration(m);
        Quaternion startrot = pivot.transform.rotation;
        for (float timer = 0; timer < duration; timer += Time.deltaTime)
        {
            pivot.transform.rotation = Quaternion.Slerp(startrot, m.getQuaternion(), timer/duration);
            yield return null;
        }
        pivot.transform.rotation = m.getQuaternion();
        foreach (Transform t in transforms)
        {
            t.parent = transform;
            snapToGrid(t);
        }
        Destroy(pivot);
        yield return null;
        
    }

    public Transform[] getAllTransforms(List<Piece> pieces)
    {
        List<Transform> output = new List<Transform>();
        foreach (Piece p in pieces)
        {
            foreach (Face f in p.faces)
            {
                output.Add(getTransform(p.position + f.direction / 2));
            }
        }
        return output.ToArray();
    }

    public Transform getTransform(Vector3 pos)
    {
        Collider[] hitColliders = Physics.OverlapSphere(pos, 0.1f);
        if (hitColliders.Length.Equals(1))
        {
            hitColliders[0].transform.position = pos;
            return hitColliders[0].transform;
        }
        else
        {
            UnityEngine.Debug.Log("colliders != 1");
            return null;
        }
    }

    public void snapToGrid(Transform t)
    {
            Vector3 currentPos = transform.position;
            transform.position = new Vector3(Mathf.Round(currentPos.x * 2)/2,
                                 Mathf.Round(currentPos.y * 2)/2,
                                 Mathf.Round(currentPos.z * 2)/2);

    }

    public int getDuration(Move m)
    {
        if (m.angle == 2)
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }
}
