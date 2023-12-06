using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


/*
Script for handling the rendering and animation of the cube in the interactive cube GUI.
*/

namespace InteractiveCube
{
    public class CubeUpdater : MonoBehaviour
    {

        [SerializeField]
        private CubeComponent cube;

        [SerializeField]
        private GameObject quad;

        [SerializeField]
        private Material[] materials;

        readonly Vector3[] directions = { Vector3.back, Vector3.down, Vector3.forward, Vector3.left, Vector3.right, Vector3.up };

        void Start()
        {
            cube = GetComponent<CubeComponent>();
        }


        public void spawnCube()
        {
            Cube c = cube.getCube();
            foreach (Vector3 pos in c.pieces.Select(p => p.position))
            {
                Transform pieceGO = new GameObject("pieceGO").transform;
                pieceGO.parent = transform;
                pieceGO.position = pos;
                foreach (Vector3 direction in directions)
                {
                    Transform newQuad = Instantiate(quad).transform;
                    newQuad.parent = pieceGO;
                    newQuad.position = pos + direction / 2;
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
                    Transform face = GetChildByPos(coords, Piece);
                    face.GetComponent<MeshRenderer>().material = materials[(int)f.colour];
                }
            }
        }

        public IEnumerator animateMove(Move m, bool isSingle)
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
                pivot.transform.rotation = Quaternion.Slerp(startrot, m.getQuaternion(), timer / duration);
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
            if (isSingle)
            {
                cube.setModifiable(true);
            }
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
            return GetChildByPos(pos, transform);
        }

        public Transform GetChildByPos(Vector3 pos, Transform parent)
        {
            foreach (Transform child in parent)
            {
                if (Vector3.SqrMagnitude(pos - child.position) < 0.0001)
                {
                    return child;
                }
            }
            Debug.Log("Could not find child at" + pos);
            return parent;
        }

        public void snapToGrid(Transform t)
        {
            Vector3 currentPos = transform.position;
            transform.position = new Vector3(Mathf.Round(currentPos.x * 2) / 2,
                Mathf.Round(currentPos.y * 2) / 2,
                Mathf.Round(currentPos.z * 2) / 2);

        }
    }
}