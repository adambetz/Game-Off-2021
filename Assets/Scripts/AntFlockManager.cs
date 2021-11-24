using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AntFlockManager : MonoBehaviour
{
    public event Action<Block> ChangeFlockTarget;

    public GameObject antPrefab = null;
    public int numerOfAnts = 20;
    public List<Ant> antsArray;
    public Vector3 swimLimits = new Vector3(5, 5, 5);

    public LayerMask layerMask;

    private GameObject target = null;
    public Block goal;
    public DirtBlock currentGroundClump;
    public Home home;

    private Queue<DirtBlock> groundClumps = new Queue<DirtBlock>();

    


    protected void Start()
    {
        CreatePath();
       // GetNextGroundClump();

        antsArray = new List<Ant>();
        for (int i = 0; i < numerOfAnts; i++)
        {
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                                                                Random.Range(-swimLimits.y, swimLimits.y),
                                                                Random.Range(-swimLimits.z, swimLimits.z));
            var ant = Instantiate(antPrefab, pos, Quaternion.identity);
            antsArray.Add(ant.GetComponent<Ant>());
            //antsArray[i].SetFlockManager(this);

            antsArray[i].Gather(currentGroundClump);
        }
    }

    //private void OnEnable()
    //{
    //    home.AntSpawned += OnAntSpawned;
    //}

    //private void OnDisable()
    //{
    //    currentGroundClump.GroundClumpDepleted -= GetNextGroundClump;
    //    home.AntSpawned -= OnAntSpawned;
    //}

    private void CreatePath()
    {
        RaycastHit[] hits;
        Vector3 dir = Vector3.Normalize(goal.transform.position - home.transform.position); 

        hits = Physics.RaycastAll(home.transform.position, dir, Vector3.Distance(home.transform.position, goal.transform.position), layerMask);

        foreach(RaycastHit hit in hits) {
            var clump = hit.transform.GetComponent<DirtBlock>();;
            if (!groundClumps.Contains(clump))
            {
                groundClumps.Enqueue(clump);
            }
        }

        DebugGroundClumps();
    }

    private void DebugGroundClumps()
    {
        foreach(DirtBlock clump in groundClumps)
        {
            Debug.Log(clump.name);
        }
    }

    //private void GetNextGroundClump()
    //{
    //    if(currentGroundClump)
    //        currentGroundClump.GroundClumpDepleted -= GetNextGroundClump;

    //    if(groundClumps.Count > 0)
    //    {
    //        currentGroundClump = groundClumps.Dequeue();
    //        currentGroundClump.GroundClumpDepleted += GetNextGroundClump;
    //    }

    //    ChangeFlockTarget?.Invoke(currentGroundClump);
    //}

    private void OnAntSpawned(GameObject ant)
    {
        var a = ant.GetComponent<Ant>();
        //a.SetFlockManager(this);
        a.Gather(currentGroundClump);
    }
}
