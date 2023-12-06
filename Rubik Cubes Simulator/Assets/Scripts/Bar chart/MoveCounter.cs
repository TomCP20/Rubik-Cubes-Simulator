using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
static class for counting fow many moves a solution took.
countMoves takes a queue of moves and an int representing a move metric and returns the number of moves under that metric.
allMoveCounts creates a array of move counts under a specified metric and solving method that is returned using the reference moveCounts.
*/

namespace BarChart
{
    static class MoveCounter
    {

        static public float countMoves(Queue<Move> moves, int metricType)
        {
            switch (metricType)
            {
                case 0:
                    return getHTM(moves);
                case 1:
                    return getQTM(moves);
                case 2:
                    return getSTM(moves);
                case 3:
                    return getQSTM(moves);
                case 4:
                    return getATM(moves);
                case 5:
                    return (float)get15HTM(moves);
            }
            return 0;
        }

        static public int getHTM(Queue<Move> moves)
        {
            return moves.Count;
        }
        static public int getQTM(Queue<Move> moves)
        {
            int sum = 0;
            foreach (Move m in moves)
            {
                sum += Mathf.Abs(m.angle);
            }
            return sum;
        }
        static public int getSTM(Queue<Move> moves)
        {
            Queue<Move> newQueue = new Queue<Move>(moves);
            int sum = 0;
            Move current = newQueue.Dequeue();
            while (newQueue.Count > 0)
            {
                Move next = newQueue.Dequeue();
                if (current.axis == next.axis && current.angle == next.angle)
                {
                    sum += 1;
                    if (newQueue.Count > 0)
                    {
                        current = newQueue.Dequeue();
                    }

                }
                else
                {
                    sum += 1;
                    current = next;
                }
            }
            sum += 1;
            return sum;
        }
        static public int getQSTM(Queue<Move> moves)
        {
            Queue<Move> newQueue = new Queue<Move>(moves);
            int sum = 0;
            Move current = newQueue.Dequeue();
            while (newQueue.Count > 0)
            {
                Move next = newQueue.Dequeue();
                if (current.axis == next.axis && current.angle == next.angle)
                {
                    sum += Mathf.Abs(current.angle);
                    if (newQueue.Count > 0)
                    {
                        current = newQueue.Dequeue();
                    }
                }
                else
                {
                    sum += Mathf.Abs(current.angle);
                    current = next;
                }
            }
            sum += current.angle;
            return sum;
        }
        static public int getATM(Queue<Move> moves)
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
                    sum += 1;
                    current = next;
                }
            }
            sum += 1;
            return sum;
        }
        static public double get15HTM(Queue<Move> moves)
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

        static public IEnumerator allMoveCounts(int solverType, int metricType, int sampleSize, float[] moveCounts)
        {
            for (int i = 0; i < sampleSize; i++)
            {
                Cube c = new Cube();
                c.randomMoveSequence();
                CubeSolver s;
                if (solverType == 0) { s = new LayerByLayer(c); }
                else { s = new CFOP(c); }
                yield return s.solve();
                Queue<Move> moves = s.getSolution();
                moveCounts[i] = countMoves(moves, metricType);
            }
            yield return null;
        }
    }
}