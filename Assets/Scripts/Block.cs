using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : MonoBehaviour
{
    public static event Action BlockedAddedToQueue;
    public static event Action<Block> BlockRemovedFromQueue;

    public event Action BlockDepleted;

    public static Queue<Block> blockQueue = new Queue<Block>();

    public BlockData blockData;

    [SerializeField] protected Color colorNotReachable = Color.white;
    [SerializeField] protected Color colorReachable = Color.white;
    [SerializeField] protected Color colorSelected = Color.white;
    [SerializeField] protected Color colorHover = Color.white;

    //protected Color lastColor = Color.white;

    public abstract bool GrabBlock();

    public void SetReachable(bool reachable)
    {
        blockData.isReachable = reachable;
        if (reachable)
        {
            if (blockData.currentBlockState == BlockState.IDLE)
            {
                var children = GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer child in children)
                {
                    child.material.color = colorReachable;
                }
            }
        }
    }

    public virtual void Initialize(BlockData data)
    {
        blockData = data;
        if (blockData.currentBlockState == BlockState.DEPLETED)
        {
            gameObject.SetActive(false);
        }
        if (blockData.isReachable)
        {
            SetReachable(true);
            //Debug.Log(transform.position);
        }
    }

    protected void AddBlockToGatherQueue()
    {
        blockData.currentBlockState = BlockState.PENDING;

        AddBlockToQueue();

        GridManager.SetNeighborsReachable(blockData.position.x, blockData.position.y);

        var children = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer child in children)
        {
            child.material.color = colorSelected;
        }
    }

    protected void SetBlockDepleted()
    {
        BlockDepleted?.Invoke();
        blockData.currentBlockState = BlockState.DEPLETED;
        gameObject.SetActive(false);
    }

    private void AddBlockToQueue()
    {
        blockQueue.Enqueue(this);
        BlockedAddedToQueue?.Invoke();
    }
}
