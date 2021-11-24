using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Home : MonoBehaviour, IPointerClickHandler
{
    public event Action<GameObject> AntSpawned;
    public event Action<Block> ChangeFlockTarget;
    
    public int numerOfAnts = 0;
    public List<Ant> antsArray;

    [SerializeField] private GameObject antPrefab = null;
    [SerializeField] private Transform unitSpawnPoint = null;

    public Block currentGoal;


    private void OnEnable()
    {
        Block.BlockedAddedToQueue += OnBlockAddedToQueue;
    }

    private void OnDisable()
    {
        Block.BlockedAddedToQueue -= OnBlockAddedToQueue;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        SpawnAnt();
    }

    private void SpawnAnt()
    {
        GameObject antInstance = Instantiate(antPrefab, unitSpawnPoint.position, unitSpawnPoint.rotation);

        var ant = antInstance.GetComponent<Ant>();
        ant.Initialize(this);
        //AntSpawned?.Invoke(antInstance);
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
}
