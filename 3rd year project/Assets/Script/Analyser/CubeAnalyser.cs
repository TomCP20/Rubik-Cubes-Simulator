using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CubeAnalyser
{
    private Cube c;
    private Queue<Move> moves;

    private int type;

    public float[] count;

    public CubeAnalyser(int t)
    {
        type = t;      
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
    public int getQSTM()
    {
        Queue<Move> newQueue = new Queue<Move>(moves);
        int sum = 0;
        Move current = newQueue.Dequeue();
        while (newQueue.Count > 0)
        {
            Move next = newQueue.Dequeue();
            if (current.axis == next.axis && current.angle == next.angle)
            {
                sum+=Math.Abs(current.angle);
                current = newQueue.Dequeue();
            }
            else
            {
                sum+=Math.Abs(current.angle);
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

    public IEnumerator solveCuve()
    {
        Cube c = new Cube();
        c.randomMoveSequence();
        CubeSolver s;
        if (type == 0)
        {
            s = new LayerByLayer(c);
        }
        else
        {
            s = new CFOP(c);
        }
        yield return s.solve();
        moves = s.getSolution();
        count = new float[] {getHTM(), getQTM(), getSTM(), getQSTM(), getATM(), (float)get15HTM()};
        yield return null;
    }
}