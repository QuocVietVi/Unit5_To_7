using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TransporterData 
{
    public string color;
    public float speed;
    public int ammount;

    public TransporterData() 
    {

    }

    public TransporterData(string color, float speed, int ammount)
    {
        this.color = color;
        this.speed = speed;
        this.ammount = ammount;
    }
}

public enum TransporterColor
{
    Red = 0,
    Blue = 1,
    Green = 2,
    Yellow = 3,
}
