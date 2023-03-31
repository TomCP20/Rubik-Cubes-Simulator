using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeUpdater : MonoBehaviour
{

    public CubeComponent cube;

    public GameObject quad;

    public Vector3[] directions = {Vector3.back, Vector3.down, Vector3.forward, Vector3.left, Vector3.right, Vector3.up};

    void Start()
    {
        cube = GetComponent<CubeComponent>();
    }

    public Material[] materials; 

    public void spawnCube()
    {
        Cube c = cube.getCube();
        foreach (Piece p in c.pieces)
        {
            Transform pieceGO = new GameObject("pieceGO").transform;
            pieceGO.parent = transform;
            pieceGO.position = p.position;
            foreach (Vector3 direction in directions)
            {
                Transform newQuad = Instantiate(quad).transform;
                newQuad.parent = pieceGO;
                newQuad.position = p.position + direction / 2;
                Quaternion rotation = new Quaternion();
                rotation.SetFromToRotation(Vector3.back, direction);
                newQuad.rotation = rotation;
                newQuad.GetComponent<MeshRenderer>().material = materials[0];
            }
        }
    }

    public void colourCube()
    {
        Cube c = cube.getCube();
        foreach (Piece p in c.pieces)
        {
            Transform Piece = getChildByPos(p.position);
            foreach (Face f in p.faces)
            {
                Vector3 coords = p.position + f.direction / 2;
                Transform face = getChildByPos(coords, Piece);
                face.GetComponent<MeshRenderer>().material = materials[(int)f.colour];
            }
        }
    }

    public IEnumerator animateMove(Move m)
    {
        Cube c = cube.getCube();
        Transform[] Pieces = getAllTransforms(c.getLayer(m.axis, m.slice));
        GameObject pivot = new GameObject("pivot");
        pivot.transform.position = Vector3.zero;
        foreach (Transform t in Pieces) { t.parent = pivot.transform; }
        float duration = 1f;
        Quaternion startrot = pivot.transform.rotation;
        for (float timer = 0; timer < duration; timer += Time.deltaTime)
        {
            pivot.transform.rotation = Quaternion.Slerp(startrot, m.getQuaternion(), timer/duration);
            yield return null;
        }
        pivot.transform.rotation = m.getQuaternion();
        foreach (Transform t in Pieces)
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
            output.Add(getChildByPos(p.position));
        }
        return output.ToArray();
    }
    public Transform getChildByPos(Vector3 pos)
    {
        return getChildByPos(pos, transform);
    }

    public Transform getChildByPos(Vector3 pos, Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (Vector3.SqrMagnitude(pos - child.position) < 0.0001)
            {
                return child;
            }
        }
        Debug.Log("Could not find child at" + pos);
        return null;
    }

    public void snapToGrid(Transform t)
    {
        Vector3 currentPos = transform.position;
        transform.position = new Vector3(Mathf.Round(currentPos.x * 2)/2,
            Mathf.Round(currentPos.y * 2)/2,
            Mathf.Round(currentPos.z * 2)/2);

    }
}
