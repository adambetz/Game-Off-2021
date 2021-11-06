using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ant : MonoBehaviour
{
    public float accuracy = 5.0f;

    [SerializeField] private NavMeshAgent agent;
    private Home home;

    private Goal[] goals;
    private GameObject targetGoal;

    private GameObject currentTarget;

    public Animator anim;

    private void Awake()
    {
        home = FindObjectOfType<Home>();
        goals = FindObjectsOfType<Goal>();
    }

    private void Start()
    {
        targetGoal = goals[Random.Range(0, goals.Length)].gameObject;
        currentTarget = targetGoal;

        Seek(currentTarget.transform.position);
    }

    private void Update()
    {
        Gather();
    }

    // Got to target position
    public void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    public void Gather()
    {
        if (GetDistanceToTarget() <= accuracy)
        {
            anim.SetBool("Walk", true);
            SetNextTarget();
            Seek(currentTarget.transform.position);
        }
    }

    private float GetDistanceToTarget()
    {
        Vector3 direction = currentTarget.transform.position - this.transform.position;
        return direction.magnitude;
    }

    private void SetNextTarget()
    {
        if(currentTarget == home.gameObject)
        {
            currentTarget = targetGoal;
        }
        else
        {
            currentTarget = home.gameObject;
        }
    }

}
