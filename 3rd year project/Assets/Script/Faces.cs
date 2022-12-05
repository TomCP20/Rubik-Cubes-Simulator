using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Faces
{
    class Face
    {
        public Colour colour;
        public Vector3 direction;//unit vector

        public Face(Colour colour, Vector3 direction)
        {
            this.colour = colour;
            this.direction = direction;
        }

        public Face(Vector3 direction)
        {
            this.direction = direction;
            colour = defaultColour(direction);
        }

        public Face Clone()
        {
            return new Face(colour, direction);
        }

        public void rotate(Quaternion rotQuaternion)
        {
            direction = Vector3Int.RoundToInt(rotQuaternion * direction);
        }

        private Colour defaultColour(Vector3 direction) //somthing to remember is that the perspective of the user and the perspective of the cube is mirrored so left and right get flipped
        {
            switch (direction)
            {
                case Vector3 vec when vec.Equals(Vector3.right): return Colour.Red; //flipped
                case Vector3 vec when vec.Equals(Vector3.left): return Colour.Orange; //flipped
                case Vector3 vec when vec.Equals(Vector3.up): return Colour.Yellow;
                case Vector3 vec when vec.Equals(Vector3.down): return Colour.White;
                case Vector3 vec when vec.Equals(Vector3.forward): return Colour.Green;
                case Vector3 vec when vec.Equals(Vector3.back): return Colour.Blue;
                default: return Colour.Black;
            }
        }
    }
}
