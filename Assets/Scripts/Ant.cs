using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ant : MonoBehaviour
{
    public GameObject Food;
    public GameObject Dirt;
    public bool HasDirt = false;
    public bool HasFood = false;

    public enum AntState { IDLE, WORKING };
    public AntState currentState { get; private set; } = AntState.IDLE;

    private CameraMovement camScript;

    private bool waiting = false;
    private float waitTimer;
    private float waitedTime = Mathf.Infinity;

    //private AntFlockManager flockManager = null;
    public float accuracy = 3.0f;

    [SerializeField] private NavMeshAgent agent;
    private Home home;
    private GameObject dirtDropOff;

    //private Clump goal;
    private Block targetGoal;

    public GameObject currentTarget;

    public Animator anim;

    public AudioSource WalkSound;
    public AudioSource EatSound;

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
                if(waitedTime > waitTimer)
                {
                    waiting = false;
                }
                else
                {
                    waitedTime += Time.deltaTime;
                }
            }

            if (currentTarget != null && GetDistanceToTarget() <= accuracy && !waiting)
            {
                waiting = true;
                waitedTime = Time.deltaTime;
                waitTimer = Time.deltaTime + 2f;

                var clump = currentTarget.GetComponent<Block>();
                if (clump != null)
                {
                    if(clump.GrabBlock())
                    {
                        if (currentTarget.GetComponent<FoodBlock>()) { HasFood = true; }
                        else { HasDirt = true; }
                        EatSound.Play();
                    }
                    else
                    {

                    }
                }
                SetNextTarget();
            }
            Seek(currentTarget.transform.position);
        }
        else if (currentState == AntState.IDLE)
        {
            waiting = false;
            if (GetDistanceToTarget() <= accuracy)
            {
                HasDirt = false;
                HasFood = false;
            }
        }

        if(agent.velocity.magnitude < 0.1f && !waiting)
        {
            Dirt.SetActive(false);
            anim.SetBool("Walk with food", false);
            anim.SetBool("Walk", false);
        }
        else
        {
            if (HasDirt)
            {
                Dirt.SetActive(true);
                anim.SetBool("Walk", false);
                anim.SetBool("Walk with food", true);
            }
            else if (HasFood)
            {
                Food.SetActive(true);
                anim.SetBool("Walk", false);
                anim.SetBool("Walk with food", true);
            }
            else
            {
                Dirt.SetActive(false);
                Food.SetActive(false);
                anim.SetBool("Walk with food", false);
                anim.SetBool("Walk", true);
            }
        }

        //if (anim.GetBool("Walk")) { WalkSound.Play(); }
        //else { WalkSound.Pause(); }
    }

    public void Initialize(Home h, GameObject dropOff)
    {
        home = h;
        home.ChangeFlockTarget += SetNewGoal;

        dirtDropOff = dropOff;

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
            agent.SetDestination(home.transform.position + new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f)));
            HasDirt = false;
            HasFood = false;
        }
        else
        {
            if (currentState == AntState.WORKING)
            {
                targetGoal = goal;
                if (currentTarget.gameObject != dirtDropOff.gameObject)
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
        if(currentTarget != null)
        {
            Vector3 direction = currentTarget.transform.position - this.transform.position;
            return direction.magnitude;
        }
        return Mathf.Infinity; 
    }

    private void SetNextTarget()
    {
        if(currentTarget == dirtDropOff.gameObject || currentTarget == home.gameObject)
        {
            HasDirt = false;
            HasFood = false;
            currentTarget = targetGoal.gameObject;
        }
        else
        {
            currentTarget = dirtDropOff;
        }
    }

}
