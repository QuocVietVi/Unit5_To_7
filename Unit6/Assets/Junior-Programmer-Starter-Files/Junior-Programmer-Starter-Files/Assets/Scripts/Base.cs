using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A special building that hold a static reference so it can be found by other script easily (e.g. for Unit to go back
/// to it)
/// </summary>
public class Base : Building
{ 
    public static Base Instance { get; private set; }
    public List<Transform> listResourceHolder = new List<Transform>();
    public int ammountPotion;
    public int ammountMoney;

    private void Awake()
    {
        Instance = this;
    }
    
    public void SpawnResource(GameObject resource, Transform parent, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPos = new Vector3(resource.transform.position.x,
               resource.transform.position.y + i*0.8f, resource.transform.position.z);
            Vector3 resScale = new Vector3(2, 0.7f, 2);
            var r = Instantiate(resource, parent);
            r.transform.localPosition = spawnPos;
            r.transform.localScale = resScale;
        }


    }
}
