using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public GameObject antPrefab = null;
    public int numerOfAnts = 20;
    public Ant[] antsArray;
    public Vector3 swimLimits = new Vector3(5,5,5);

}
