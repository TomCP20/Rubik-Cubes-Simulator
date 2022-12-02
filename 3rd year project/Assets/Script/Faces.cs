using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Faces
{
    class Face : ICloneable
    {
        public Colour colour;
        public Vector3 direction;//unit vector

        public Face(Colour c, Vector3 d)
        {
            colour = c;
            direction = d;
        }

        public Face(Vector3 d)
        {
            direction = d;
            colour = defaultColour(direction);
        }

        public object Clone()
        {
            return new Face(colour, direction);
        }

        public void rotate(Quaternion rotQuaternion)
        {
            direction = Vector3Int.RoundToInt(rotQuaternion * direction);
        }

        private Colour defaultColour(Vector3 d)
        {
            switch (d)
            {
                case Vector3 vec when vec.Equals(Vector3.right): return Colour.Green;
                case Vector3 vec when vec.Equals(Vector3.left): return Colour.Yellow;
                case Vector3 vec when vec.Equals(Vector3.up): return Colour.White;
                case Vector3 vec when vec.Equals(Vector3.down): return Colour.Blue;
                case Vector3 vec when vec.Equals(Vector3.forward): return Colour.Red;
                case Vector3 vec when vec.Equals(Vector3.back): return Colour.Orange;
                default: return Colour.Black;
            }
        }
    }
}
