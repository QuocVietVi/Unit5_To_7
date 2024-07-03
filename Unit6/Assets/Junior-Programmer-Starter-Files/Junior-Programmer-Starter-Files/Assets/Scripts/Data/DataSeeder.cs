using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSeeder : MonoBehaviour
{

    private void Start()
    {
        List<TransporterData> trans = new List<TransporterData>() 
        {
            new TransporterData("Red",5,1),
            new TransporterData("Blue",4,2),
            new TransporterData("Green",3,3),
            new TransporterData("Yellow",2,4),

        };

        if (!DataManager.Instance.HasData<TransporterData>())
        {
            DataManager.Instance.SaveData(trans);
            Debug.Log("create succesful");
        }
        

    }
}
