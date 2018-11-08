using UnityEngine;
using System.Collections.Generic;

namespace MultiPlayer
{
    
}
public class NewAtkRange : MonoBehaviour
{
    public List<NewGameUnit> detectedUnits = new List<NewGameUnit>();
    public BoxCollider attackRange;
    public Rigidbody colliderBody;
    public NewGameUnit parent;

    public void Start()
    {
        this.colliderBody = this.GetComponent<Rigidbody>();
        if (this.colliderBody == null)
        {
            Debug.LogError("Attack Range: Cannot detect Rigidbody.");
        }
        this.attackRange = this.GetComponent<BoxCollider>();
        if (this.attackRange == null)
        {
            Debug.LogError("Attack Range: Cannot assign sphere collider.");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        NewGameUnit unit = other.gameObject.GetComponent<NewGameUnit>();
        if (unit != null && !unit.hasAuthority && unit.properties.teamFactionID != this.parent.properties.teamFactionID)//
        {
            this.detectedUnits.Add(unit);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        NewGameUnit unit = other.GetComponent<NewGameUnit>();
        if (unit != null && !unit.hasAuthority && unit.properties.teamFactionID != this.parent.properties.teamFactionID)//
        {
            this.detectedUnits.Remove(unit);
        }
        /*if (this.detectedUnits.Count <= 0)
        {
            NewChanges changes = this.parent.CurrentProperty();
            changes.enemyHitPosition = changes.mousePosition;
            this.parent.CallCmdupdateProperty(changes);
        }*/
    }

    public void FixedUpdate()
    {
        this.colliderBody.WakeUp();

        if (this.parent != null)
        {
            if (this.detectedUnits.Count > 0)
            {
                NewChanges changes = this.parent.CurrentProperty();
                if (this.detectedUnits[0] != null && detectedUnits[0].properties.currentHealth > 0)  //should fix;
                {
                    Debug.Log("NiceTarget");
                    changes.targetUnit = this.detectedUnits[0].gameObject;
                    this.parent.CallCmdupdateProperty(changes);
                }
                else
                {
                    Debug.Log("CurrentTargetDead");
                    detectedUnits.Remove(detectedUnits[0]);
                    return;

                }
                    
                    
                    //changes.enemyHitPosition = this.detectedUnits[0].transform.position;
                    
                
            }
        }
    }
}

