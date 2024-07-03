using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DropPointData 
{
    public Dictionary<string, int> resourceValue;

    public DropPointData()
    {
        resourceValue = new Dictionary<string, int>
        {
            { "potion", 16 },
            { "money", 16 },
        };
    }
}
