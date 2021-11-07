using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ant : MonoBehaviour
{
    private AntFlockManager flockManager = null;
    public float accuracy = 5.0f;

    [SerializeField] private NavMeshAgent agent;
    private Home home;

    //private Clump goal;
    private Clump targetGoal;

    public GameObject currentTarget;

    public Animator anim;

    private bool gathering = false;

    private void Awake()
    {
        home = FindObjectOfType<Home>();
    }

    private void Start()
    {
        //targetGoal = goals[Random.Range(0, goals.Length)].gameObject;
        //currentTarget = targetGoal;

       // Seek(currentTarget.transform.position);
    }

    private void Update()
    {
        if(gathering)
        {
            if(!anim.GetBool("Walk")) anim.SetBool("Walk", true);

            if (GetDistanceToTarget() <= accuracy)
            {
                var clump = currentTarget.GetComponent<Clump>();
                if (clump != null)
                {
                    if(clump.GrabBlock())
                    {

                    }
                    else
                    {

                    }
                }
                SetNextTarget(); 
            }
            Seek(currentTarget.transform.position);
        }
    }

    public void SetFlockManager(AntFlockManager manager)
    {
        flockManager = manager;
        flockManager.ChangeFlockTarget += SetNewGoal;
    }

    private void OnDisable()
    {
        flockManager.ChangeFlockTarget -= SetNewGoal;
    }

    // Got to target position
    public void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    public void Gather(Clump goal)
    {
        targetGoal = goal;
        currentTarget = goal.gameObject;
        gathering = true;
    }

    private void SetNewGoal(Goal goal)
    {
        targetGoal = goal.GetComponent<Clump>();
        // send ant to new goal if not headed home
        if(currentTarget != home) {
            currentTarget = targetGoal.gameObject;
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
            currentTarget = targetGoal.gameObject;
        }
        else
        {
            currentTarget = home.gameObject;
        }
    }

}
