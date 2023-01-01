using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moves
{
    readonly struct Move
    {
        public Axis axis { get; }
        public int slice { get; }
        public int angle { get; }

        public Move(Axis axis, int slice, int angle)
        {
            this.axis = axis;
            this.slice = slice;
            this.angle = angle;
        }

        public Move(string notation)
        {
            string face = notation.Substring(0, 1);
            if (notation.Length == 1) { angle = 1; }
            else if (notation.Substring(1) == "'") { angle = -1; }
            else if (notation.Substring(1) == "2") { angle = 2; }
            else { angle = 0; }
            switch (face) // left right orientation is relavent here
            {
                case "F":
                    axis = Axis.Z;
                    slice = 1;
                    break;
                case "U":
                    axis = Axis.Y;
                    slice = 1;
                    break;
                case "R":
                    axis = Axis.X;
                    slice = -1;
                    angle = -angle;
                    break;
                case "B":
                    axis = Axis.Z;
                    slice = -1;
                    angle = -angle;
                    break;
                case "D":
                    axis = Axis.Y;
                    slice = -1;
                    angle = -angle;
                    break;
                case "L":
                    axis = Axis.X;
                    slice = 1;
                    break;
                default:
                    axis = Axis.Z;
                    slice = 0;
                    angle = 0;
                    break;
            }
        }

        public string getNotation()
        {
            string output = "";
            switch (axis)
            {
                case Axis.X:
                    if (slice == 1) { output = "F"; }
                    else { output =  "B"; }
                    break;
                case Axis.Y:
                    if (slice == 1) { output = "U"; }
                    else { output =  "D"; }
                    break;
                case Axis.Z:
                    if (slice == 1) { output = "R"; }
                    else { output = "L"; }
                    break;
                default:
                    output =  "";
                    break;
            }
            if (slice == 1)
            {
                if (angle == 2)
                {
                    output += "2";
                }
                else if (angle == -1)
                {
                    output += "'";
                }
            }
            else
            {
                if (angle == 2)
                {
                    output += "2";
                }
                else if (angle == 1)
                {
                    output += "'";
                }
            }
            return output;
        }
    }
}
