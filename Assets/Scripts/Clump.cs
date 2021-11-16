using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clump : Goal
{
    public event Action GroundClumpDepleted;
    private bool depleted = false;

    public Queue<GameObject> blocks = new Queue<GameObject>();

    private void Awake()
    {
        for(int i =0; i<transform.childCount; i++)
        {
            blocks.Enqueue(transform.GetChild(i).gameObject);
        }
    }

    public bool GrabBlock()
    {
        if(blocks.Count == 0) {
            if(!depleted)
            {
                GroundClumpDepleted?.Invoke();
                depleted = true;
                gameObject.SetActive(false);
            }
            
            return false;
        }

        var block = blocks.Dequeue();
        Destroy(block.gameObject);
        return true;
    }
}
