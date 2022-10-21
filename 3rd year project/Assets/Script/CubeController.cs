using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    static double Magnitude(double[] position)
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

    enum PiceType
    {
        Corner,
        Edge,
        Face
    }

    class Face
    {
        private Colour colour;
        private double[] direction; //x, y, z from the pice

        public Face(Colour c, double[] d)
        {
            colour = c;
            direction = d;
        }

        public Face(double[] d)
        {
            direction = d;
            if (direction[0] != 0)
            {
                if (direction[0] > 0)
                {
                    colour = Colour.Green;
                }
                else
                {
                    colour = Colour.Yellow;
                }
            }
            else if (direction[1] != 0)
            {
                if (direction[1] > 0)
                {
                    colour = Colour.White;
                }
                else
                {
                    colour = Colour.Blue;
                }
            }
            else
            {
                if (direction[2] > 0)
                {
                    colour = Colour.Red;
                }
                else
                {
                    colour = Colour.Orange;
                }
            }
        }
    }

    class Pice
    {
        private double[] position; //x, y, z
        private Face[] faces = new Face[3];

        public Pice(double[] p, Face[] f)
        {
            position = p;
            faces = f;
        }

        public Pice(double[] p)
        {
            position = p;
            double magnitude = Magnitude(position);
            switch (magnitude)
            {
                case 3:
                    faces[0] = new Face(PositionToDirection(position, "x"));
                    faces[1] = new Face(PositionToDirection(position, "y"));
                    faces[2] = new Face(PositionToDirection(position, "z"));
                    break;
                case 2:
                    faces = new Face[2];
                    if (position[0] == 0)
                    {
                        faces[0] = new Face(PositionToDirection(position, "y"));
                        faces[1] = new Face(PositionToDirection(position, "z"));
                    }
                    else if (position[1] == 0)
                    {
                        faces[0] = new Face(PositionToDirection(position, "x"));
                        faces[1] = new Face(PositionToDirection(position, "z"));
                    }
                    else
                    {
                        faces[0] = new Face(PositionToDirection(position, "x"));
                        faces[1] = new Face(PositionToDirection(position, "y"));
                    }
                    break;
                case 1:
                    faces = new Face[1];
                    if (position[0] != 0)
                    {
                        faces[0] = new Face(PositionToDirection(position, "x"));
                    }
                    else if (position[1] != 0)
                    {
                        faces[0] = new Face(PositionToDirection(position, "y"));
                    }
                    else
                    {
                        faces[0] = new Face(PositionToDirection(position, "z"));
                    }
                    break;
            }
        }

        private double[] PositionToDirection(double[] p, string axis)
        {
            double[] direction = {0, 0, 0};

            switch(axis)
            {
                case "x":
                    direction[0] = p[0]/2;
                    break;
                case "y":
                    direction[0] = p[1] / 2;
                    break;
                case "z":
                    direction[0] = p[2] / 2;
                    break;
            }
            return direction;
        }
    }

    class Cube
    {
        private Pice[] pices;

        public Cube(Pice[] p)
        {
            pices = p;
        }

        public Cube()
        {
            pices = new Pice[20];
            int count = 0;
            double magnitude;
            double[] position;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    for (int k = -1; k <= 1; k++)
                    {
                        position = new double[] { i, j, k};
                        magnitude = Magnitude(position);
                        if (magnitude == 3 || magnitude == 2)
                        {
                            pices[count] = new Pice(position);
                            count++;
                        }
                    }
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Cube c = new Cube();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
