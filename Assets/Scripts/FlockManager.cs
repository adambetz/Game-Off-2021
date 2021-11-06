using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public GameObject antPrefab = null;
    public int numerOfAnts = 20;
    public GameObject[] antsArray;
    public Vector3 swimLimits = new Vector3(5,5,5);

    protected virtual void Start()
    {
        antsArray = new GameObject[numerOfAnts];
        for(int i=0; i<numerOfAnts; i++)
        {
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                                                                Random.Range(-swimLimits.y, swimLimits.y),
                                                                Random.Range(-swimLimits.z, swimLimits.z));
            antsArray[i] = (GameObject) Instantiate(antPrefab, pos, Quaternion.identity);
        }
    }
}
