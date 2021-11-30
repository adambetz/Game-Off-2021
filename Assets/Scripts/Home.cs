using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Home : MonoBehaviour, IPointerClickHandler
{
    private SceneMenu menu;

    public event Action<GameObject> AntSpawned;
    public event Action<Block> ChangeFlockTarget;

    public static event Action FoodAdded;
    public static event Action FoodRemoved;
    public static event Action InsufficientFood;

    public static int FoodAmount { get; private set; } = 10;
    
    public int numerOfAnts = 0;
    public List<Ant> antsArray;

    [SerializeField] private GameObject antPrefab = null;
    [SerializeField] private Transform unitSpawnPoint = null;

    public Block currentGoal;

    public GameObject DirtDropOff = null;

    public AudioSource audioSource;

    private void OnEnable()
    {
        Block.BlockedAddedToQueue += OnBlockAddedToQueue;
    }

    private void OnDisable()
    {
        Block.BlockedAddedToQueue -= OnBlockAddedToQueue;
    }

    private void Awake()
    {
        menu = GameObject.Find("Canvas").GetComponent<SceneMenu>();

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        if (FoodAmount > 0)
        {
            SpawnAnt();
            RemoveFood();
        }
        else
        {
            InsufficientFood?.Invoke();
        }
    }

    private void SpawnAnt()
    {
        GameObject antInstance = Instantiate(antPrefab, unitSpawnPoint.position, unitSpawnPoint.rotation, menu.antHolder);

        var ant = antInstance.GetComponent<Ant>();
        ant.Initialize(this, DirtDropOff);

        menu.addAnt();
        AntSpawned?.Invoke(antInstance);

        audioSource.pitch = UnityEngine.Random.Range(1f, 2f);
        audioSource.Play();
    }

    private void OnBlockAddedToQueue()
    {
        if(currentGoal == null)
        {
            GetNextGoal();
        }
    }

    private void GetNextGoal()
    {
        if(currentGoal) 
            currentGoal.BlockDepleted -= GetNextGoal;

        if (Block.blockQueue.Count > 0)
        {
            currentGoal = Block.blockQueue.Dequeue();
            currentGoal.BlockDepleted += GetNextGoal;
        }
        else
        {
            currentGoal = null;
        }
       
        ChangeFlockTarget?.Invoke(currentGoal);
    }

    public static void AddFood()
    {
        FoodAmount++;
        FoodAdded?.Invoke();
    }

    public static void RemoveFood()
    {
        FoodAmount--;
        FoodRemoved?.Invoke();
    }
}
