using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Ant : MonoBehaviour, IPointerClickHandler
{
    public enum AntState { IDLE, WORKING };
    public AntState currentState { get; private set; } = AntState.IDLE;

    private CameraMovement camScript;

    private bool waiting = false;
    private float waitTimer;

    //private AntFlockManager flockManager = null;
    public float accuracy = 5.0f;

    [SerializeField] private NavMeshAgent agent;
    private Home home;

    //private Clump goal;
    private Block targetGoal;

    public GameObject currentTarget;

    public Animator anim;

    private void Awake()
    {
        home = FindObjectOfType<Home>();
        camScript = GameObject.Find("Main Camera").GetComponent<CameraMovement>();
    }

    private void Update()
    {
        if(currentState == AntState.WORKING)
        {
            if (waiting)
            {
                if(Time.deltaTime > waitTimer)
                {
                    waiting = false;
                }
            } 

            if (!anim.GetBool("Walk")) anim.SetBool("Walk", true);

            if (GetDistanceToTarget() <= accuracy)
            {
                waiting = true;
                waitTimer = Time.deltaTime + 3f;

                var clump = currentTarget.GetComponent<DirtBlock>();
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

    //left click on an ant to follow it
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right) return;

        camScript.ZoomOnTarget(gameObject);
    }

    public void Initialize(Home h)
    {
        home = h;
        home.ChangeFlockTarget += SetNewGoal;

        if(home.currentGoal != null)
        {
            Gather(home.currentGoal);
        }
        else
        {
            agent.SetDestination(transform.position + new Vector3(Random.Range(-2.5f, 2.5f), 0, Random.Range(-2.5f, 2.5f)));
        }
    }

    private void OnDisable()
    {
        home.ChangeFlockTarget -= SetNewGoal;
    }

    // Got to target position
    public void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    public void Gather(Block goal)
    {
        targetGoal = goal;
        currentTarget = goal.gameObject;
        currentState = AntState.WORKING;
    }

    private void SetNewGoal(Block goal)
    {    
        if(goal == null)
        {
            // no new goal. send ants home
            currentState = AntState.IDLE;
            agent.SetDestination(home.transform.position + new Vector3(Random.Range(-2.5f, 2.5f), 0, Random.Range(-2.5f, 2.5f)));
        }
        else
        {
            if (currentState == AntState.WORKING)
            {
                targetGoal = goal;
                if (currentTarget.gameObject != home.gameObject)
                {
                    currentTarget = targetGoal.gameObject;
                }
            }
            else
            {
                Gather(goal);
            }
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
