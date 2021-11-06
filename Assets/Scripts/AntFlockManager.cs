using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntFlockManager : FlockManager
{
    public LayerMask layerMask;

    private GameObject target = null;
    public Goal goal;
    public Home home;

    private List<GameObject> groundClumps = new List<GameObject>();
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        CreatePath();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreatePath()
    {
        RaycastHit[] hits;
        Vector3 dir = Vector3.Normalize(goal.transform.position - home.transform.position); 

        hits = Physics.RaycastAll(home.transform.position, dir, Vector3.Distance(home.transform.position, goal.transform.position), layerMask);

        foreach(RaycastHit hit in hits) {
            var clump = hit.transform.parent.gameObject;
            if (!groundClumps.Contains(clump))
            {
                groundClumps.Add(clump);
            }
        }

        DebugGroundClumps();
    }

    private void DebugGroundClumps()
    {
        foreach(GameObject clump in groundClumps)
        {
            Debug.Log(clump.name);
        }
    }
}
