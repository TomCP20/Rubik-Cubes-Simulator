using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;
using System.Diagnostics;

public class CubeController : MonoBehaviour
{
    static double Magnitude(Vector3 position)
    {
        return Math.Abs(position[0]) + Math.Abs(position[1]) + Math.Abs(position[2]);
    }

    enum Colour
    {
        Black,
        White,
        Green,
        Blue,
        Red,
        Yellow,
        Orange,
    }

    enum Axis
    {
        X = 0,
        Y = 1,
        Z = 2
    }

    class Face
    {
        public Colour colour;
        public Vector3 direction;

        public Face(Colour c, Vector3 d)
        {
            colour = c;
            direction = d;
        }

        public Face(Vector3 d)
        {
            direction = d;
            if (direction[0] > 0) { colour = Colour.Green; }
            else if (direction[0] < 0) { colour = Colour.Yellow; }
            else if (direction[1] > 0) { colour = Colour.White; }
            else if (direction[1] < 0) { colour = Colour.Blue; }
            else if (direction[2] > 0) { colour = Colour.Red; }
            else if (direction[2] < 0) { colour = Colour.Orange; }
            else { colour = Colour.Black; }
        }

    }

    class Pice
    {
        public Vector3 position;
        public Face[] faces;

        public Pice(Vector3 p, Face[] f)
        {
            position = p;
            faces = f;
        }

        public Pice(Vector3 pos)
        {
            position = pos;
            List<Face> FaceList = new List<Face>();
            for (int i = 0; i < 3; i++)
            {
                if (position[i] != 0)
                {
                    FaceList.Add(new Face(PositionToDirection(position, (Axis)i)));
                }
            }
            faces = FaceList.ToArray();
        }

        private Vector3 PositionToDirection(Vector3 p, Axis axis)
        {
            Vector3 direction = Vector3.zero;
            direction[(int)axis] = p[(int)axis] / 2;
            return direction;
        }
    }

    class Cube
    {
        public Pice[] pices;

        public Cube(Pice[] p)
        {
            pices = p;
        }

        public Cube()
        {
            List<Pice> Picelist = new List<Pice>();
            double magnitude;
            Vector3 position;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    for (int k = -1; k <= 1; k++)
                    {
                        position = new Vector3(i, j, k);                        
                        magnitude = Magnitude(position);
                        if (magnitude == 3 || magnitude == 2)
                        {
                            Picelist.Add(new Pice(position));
                        }
                    }
                }
            }
            pices = Picelist.ToArray();
        }
    }

    Cube c;
    bool cubeAltered;
    public Material Black;
    public Material White;
    public Material Green;
    public Material Blue;
    public Material Red;
    public Material Yellow;
    public Material Orange;

    // Start is called before the first frame update
    void Start()
    {
        
        c = new Cube();

        cubeAltered = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (cubeAltered) { updateCube(c); }
        cubeAltered = false;
        
    }

    void updateCube(Cube c)
    {
        UnityEngine.Debug.Log("updating cube");
        foreach (Pice p in c.pices)
        {
            foreach (Face f in p.faces)
            {
                Vector3 coords = p.position + f.direction;
                UnityEngine.Debug.Log(coords);
                Collider[] hitColliders = Physics.OverlapSphere(coords, 0.1f);
                foreach (var hitCollider in hitColliders)
                {
                    UnityEngine.Debug.Log(hitCollider);
                    hitCollider.GetComponent<MeshRenderer>().material = GetMaterial(f.colour);
                }
            }
        }
    }

    Material GetMaterial(Colour c)
    {
        switch(c)
        {
            case Colour.Blue:
                return Blue;
            case Colour.Green:
                return Green;
            case Colour.Red:
                return Red;
            case Colour.Yellow:
                return Yellow;
            case Colour.Orange:
                return Orange;
            case Colour.White:
                return White;
            case Colour.Black:
                return Black;
            default:
                return null;
        }
    }
}
