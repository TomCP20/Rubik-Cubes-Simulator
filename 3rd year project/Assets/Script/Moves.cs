using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moves
{
    readonly struct Move
    {
        public Axis axis { get; }
        public int movePos { get; }
        public int angle { get; }

        public Move(Axis a, int s, int an)
        {
            this.axis = a;
            this.movePos = s;
            this.angle = an;
        }

        public Move(string notation)
        {
            string face = notation.Substring(0, 1);
            if (notation.Length == 1) { angle = 1; }
            else if (notation.Substring(1) == "'") { angle = -1; }
            else if (notation.Substring(1) == "2") { angle = 2; }
            else { angle = 0; }
            switch (face)
            {
                case "F":
                    axis = Axis.X;
                    movePos = 1;
                    break;
                case "U":
                    axis = Axis.Y;
                    movePos = 1;
                    break;
                case "R":
                    axis = Axis.Z;
                    movePos = 1;
                    break;
                case "B":
                    axis = Axis.X;
                    movePos = -1;
                    angle = -angle;
                    break;
                case "D":
                    axis = Axis.Y;
                    movePos = -1;
                    angle = -angle;
                    break;
                case "L":
                    axis = Axis.Z;
                    movePos = -1;
                    angle = -angle;
                    break;
                default:
                    axis = Axis.X;
                    movePos = 0;
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
                    if (movePos == 1) { output = "F"; }
                    else { output =  "B"; }
                    break;
                case Axis.Y:
                    if (movePos == 1) { output = "U"; }
                    else { output =  "D"; }
                    break;
                case Axis.Z:
                    if (movePos == 1) { output = "R"; }
                    else { output = "L"; }
                    break;
                default:
                    output =  "";
                    break;
            }
            if (movePos == 1)
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
