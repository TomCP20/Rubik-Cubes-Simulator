using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct MovesNo
{
    public MovesNo(int HTM, int QTM, int STM, int QSTM, int ATM, double halfHTM)
    {
        this.HTM = HTM;
        this.QTM = QTM;
        this.STM = STM;
        this.QSTM = QSTM;
        this.ATM = ATM;
        this.halfHTM = halfHTM;
    }

    public int HTM { get; }
    public int QTM { get; }
    public int STM { get; }
    public int QSTM { get; }
    public int ATM { get; }
    public double halfHTM { get; }
}

