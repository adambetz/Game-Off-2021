using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AntFlockManager : FlockManager
{
    public event Action<Goal> ChangeFlockTarget;

    public LayerMask layerMask;

    private GameObject target = null;
    public Goal goal;
    public Clump currentGroundClump;
    public Home home;

    private Queue<Clump> groundClumps = new Queue<Clump>();

    


    protected void Start()
    {
        CreatePath();
        GetNextGroundClump();

        antsArray = new Ant[numerOfAnts];
        for (int i = 0; i < numerOfAnts; i++)
        {
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                                                                Random.Range(-swimLimits.y, swimLimits.y),
                                                                Random.Range(-swimLimits.z, swimLimits.z));
            var ant = Instantiate(antPrefab, pos, Quaternion.identity);
            antsArray[i] = ant.GetComponent<Ant>();
            antsArray[i].SetFlockManager(this);

            antsArray[i].Gather(currentGroundClump);
        }
    }

    private void OnDisable()
    {
        currentGroundClump.GroundClumpDepleted -= GetNextGroundClump;
    }

    private void CreatePath()
    {
        RaycastHit[] hits;
        Vector3 dir = Vector3.Normalize(goal.transform.position - home.transform.position); 

        hits = Physics.RaycastAll(home.transform.position, dir, Vector3.Distance(home.transform.position, goal.transform.position), layerMask);

        foreach(RaycastHit hit in hits) {
            var clump = hit.transform.GetComponent<Clump>();;
            if (!groundClumps.Contains(clump))
            {
                groundClumps.Enqueue(clump);
            }
        }

        DebugGroundClumps();
    }

    private void DebugGroundClumps()
    {
        foreach(Clump clump in groundClumps)
        {
            Debug.Log(clump.name);
        }
    }

    private void GetNextGroundClump()
    {
        if(currentGroundClump)
            currentGroundClump.GroundClumpDepleted -= GetNextGroundClump;

        currentGroundClump = groundClumps.Dequeue();
        currentGroundClump.GroundClumpDepleted += GetNextGroundClump;

        ChangeFlockTarget?.Invoke(currentGroundClump);
    }
}
