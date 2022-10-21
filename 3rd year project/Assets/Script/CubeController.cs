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

    enum Axis
    {
        X = 0,
        Y = 1,
        Z = 2
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
        private double[] position; //x, y, z
        private Face[] faces;

        public Pice(double[] p, Face[] f)
        {
            position = p;
            faces = f;
        }

        public Pice(double[] p)
        {
            position = p;
            double magnitude = Magnitude(position);
            faces = new Face[(int)magnitude];
            int count = 0;
            for (int i = 0; i < 3; i++)
            {
                if (position[i] != 0)
                {
                    faces[count] = new Face(PositionToDirection(position, (Axis)i));
                    count++;
                }
            }
        }

        private double[] PositionToDirection(double[] p, Axis axis)
        {
            double[] direction = {0, 0, 0};
            direction[(int)axis] = p[(int)axis] / 2;
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
