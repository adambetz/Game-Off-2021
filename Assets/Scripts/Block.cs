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
    
    public abstract void Initialize(BlockData data);
    public abstract void SetReachable(bool reachable);

    protected void AddBlockToQueue()
    {
        blockQueue.Enqueue(this);
        BlockedAddedToQueue?.Invoke();
    }

    protected void SetBlockDepleted()
    {
        BlockDepleted?.Invoke();
        blockData.currentBlockState = BlockState.DEPLETED;
        gameObject.SetActive(false);
    }
}
