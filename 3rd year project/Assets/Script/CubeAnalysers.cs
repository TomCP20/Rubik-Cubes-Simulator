using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cubes;
using LayerByLayers;
using Moves;
using MovesNos;
namespace CubeAnalysers
{
    public class CubeAnalyser
    {
        Cube c;
        LayerByLayer s;

        Queue<Move> moves;

        public CubeAnalyser()
        {
            Cube c = new Cube();
            c.randomMoveSequence();
            LayerByLayer s = new LayerByLayer(c);
            moves = s.getSolution();
        }

        public int getHTM()
        {
            return moves.Count;
        }

        public int getQTM()
        {
            int sum = 0;
            foreach (Move m in moves)
            {
                sum += Math.Abs(m.angle);
            }
            return sum;
        }

        public int getSTM()
        {
            Queue<Move> newQueue = new Queue<Move>(moves);
            int sum = 0;
            Move current = newQueue.Dequeue();
            while (newQueue.Count > 0)
            {
                Move next = newQueue.Dequeue();
                if (current.axis == next.axis && current.angle == next.angle)
                {
                    sum+=1;
                    current = newQueue.Dequeue();
                }
                else
                {
                    sum+=1;
                    current = next;
                }
            }
            sum+=1;
            return sum;
        }

        public int getQSTM() //TODO fix QSTM
        {
            Queue<Move> newQueue = new Queue<Move>(moves);
            int sum = 0;
            Move current = newQueue.Dequeue();
            while (newQueue.Count > 0)
            {
                Move next = newQueue.Dequeue();
                if (current.axis == next.axis && current.angle == next.angle)
                {
                    sum+=current.angle;
                    current = newQueue.Dequeue();
                }
                else
                {
                    sum+=current.angle;
                    current = next;
                }
            }
            sum+=current.angle;
            return sum;
        }

        public int getATM()
        {
            Queue<Move> newQueue = new Queue<Move>(moves);
            int sum = 0;
            Move current = newQueue.Dequeue();
            while (newQueue.Count > 0)
            {
                Move next = newQueue.Dequeue();
                if (current.axis == next.axis)
                {
                    current = next;
                }
                else
                {
                    sum+=1;
                    current = next;
                }
            }
            sum+=1;
            return sum;
        }

        public double get15HTM()
        {
            double sum = 0;
            foreach (Move m in moves)
            {
                if (m.angle == 2)
                {
                    sum += 1.5;
                }
                else
                {
                    sum += 1;
                }
            }
            return sum;
        }

        public MovesNo getCount()
        {
            return new MovesNo(getHTM(), getQTM(), getSTM(), getQSTM(), getATM(), get15HTM());
        }
    }
}
