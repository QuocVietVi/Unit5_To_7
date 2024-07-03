using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Subclass of Unit that will transport resource from a Resource Pile back to Base.
/// </summary>
public class TransporterUnit : Unit
{
    public int MaxAmountTransported;
    public GameObject potionPrefab;
    public GameObject moneyPrefab;
    public Transform resourceHolder;

    private List<GameObject> resources = new List<GameObject>();
    private float yOffset = 0.5f;
    private Building m_CurrentTransportTarget;
    private Building.InventoryEntry m_Transporting = new Building.InventoryEntry();

    // We override the GoTo function to remove the current transport target, as any go to order will cancel the transport
    public override void GoTo(Vector3 position)
    {
        base.GoTo(position);
        m_CurrentTransportTarget = null;
    }
    
    protected override void BuildingInRange()
    {
        var dropPoint = Base.Instance;
        if (m_Target == dropPoint)
        {
            //we arrive at the base, unload!
            if (m_Transporting.Count > 0)
            {
                var data = MainManager.Instance.dropPointData;
                if (m_Transporting.ResourceId == "potion")
                {
                    dropPoint.ammountPotion += m_Transporting.Count;
                    if (dropPoint.ammountPotion <= data.resourceValue["potion"])
                    {
                        m_Target.AddItem(m_Transporting.ResourceId, m_Transporting.Count);

                        var slot1 = dropPoint.listResourceHolder[0];
                        var slot2 = dropPoint.listResourceHolder[1];
                        if (dropPoint.ammountPotion <= 8)
                        {
                            dropPoint.SpawnResource(potionPrefab, slot1, dropPoint.ammountPotion);
                        }
                        else
                        {
                            dropPoint.SpawnResource(potionPrefab, slot2, dropPoint.ammountPotion - 8);
                        }
                        Despawn();
                    }
                   
                }
                if (m_Transporting.ResourceId == "money")
                {
                    dropPoint.ammountMoney += m_Transporting.Count;
                    if (dropPoint.ammountPotion <= data.resourceValue["money"])
                    {
                        m_Target.AddItem(m_Transporting.ResourceId, m_Transporting.Count);
                        var slot1 = dropPoint.listResourceHolder[2];
                        var slot2 = dropPoint.listResourceHolder[3];
                        if (dropPoint.ammountMoney <= 8)
                        {
                            dropPoint.SpawnResource(moneyPrefab, slot1, dropPoint.ammountMoney);
                        }
                        else
                        {
                            dropPoint.SpawnResource(moneyPrefab, slot2, dropPoint.ammountMoney - 8);
                        }
                        Despawn();
                    }
                }
            }
            


         

            //we go back to the building we came from

        }
        else
        {
            if (m_Target.Inventory.Count > 0)
            {
                m_Transporting.ResourceId = m_Target.Inventory[0].ResourceId;
                m_Transporting.Count = m_Target.GetItem(m_Transporting.ResourceId, ammount);
                canMove = false;
                Debug.Log(m_Transporting.ResourceId);
                for (int i = 0; i < m_Transporting.Count; i++)
                {
                    if (m_Transporting.ResourceId == "potion")
                    {
                        Vector3 spawnPos = new Vector3(potionPrefab.transform.position.x,
                            potionPrefab.transform.position.y + i * yOffset, potionPrefab.transform.position.z);
                        GameObject p =Instantiate(potionPrefab, resourceHolder);
                        p.transform.localPosition = spawnPos;   
                        resources.Add(p);
                    }
                    else
                    {
                        Vector3 spawnPos = new Vector3(moneyPrefab.transform.position.x,
                            moneyPrefab.transform.position.y + i * yOffset, moneyPrefab.transform.position.z);
                        GameObject m = Instantiate(moneyPrefab, resourceHolder);
                        m.transform.localPosition = spawnPos;
                        resources.Add(m);
                    }
                }
                if (m_Transporting.Count > 0)
                {
                    isDelivering = true;
                }
                m_CurrentTransportTarget = m_Target;
                GoTo(Base.Instance);

            }

        }
    }
    
    //Override all the UI function to give a new name and display what it is currently transporting
    public override string GetName()
    {
        return "Transporter";
    }

    public override string GetData()
    {
        return $"Can transport up to {ammount}";
    }

    public override void GetContent(ref List<Building.InventoryEntry> content)
    {
        if (m_Transporting.Count > 0)
            content.Add(m_Transporting);
    }
    void Despawn()
    {
        for (int i = 0; i < resources.Count; i++)
        {
            Destroy(resources[i].gameObject);
        }
        resources.Clear();
        if (newPos != null && canMove)
        {
            GoTo(newPos);
        }
        GoTo(m_CurrentTransportTarget);
        m_Transporting.Count = 0;
        m_Transporting.ResourceId = "";
        isDelivering = false;

    }
}
