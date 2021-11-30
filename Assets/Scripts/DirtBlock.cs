using System;
using System.Collections.Generic;
using UnityEngine;

public class DirtBlock : Block
{
    //public event Action GroundClumpDepleted;
    public static event Action DirtAdded;

    public Queue<GameObject> childBlocks = new Queue<GameObject>();

    private void Awake()
    {
        for (int i =0; i<transform.childCount; i++)
        {
            childBlocks.Enqueue(transform.GetChild(i).gameObject);
        }
    }

    public void MouseOverBlock()
    {
        if (blockData.isReachable && blockData.currentBlockState == BlockState.IDLE)
        {
            var children = GetComponentsInChildren<MeshRenderer>();
            //lastColor = children[0].material.color;
            foreach (MeshRenderer child in children)
            {
                child.material.color = colorHover;
            }
        }
    }

    public void MouseBlockClick()
    {
        if (blockData.isReachable && blockData.currentBlockState == BlockState.IDLE)
        {
            AddBlockToGatherQueue();
        }
    }

    public void MouseLeaveBlockHover()
    {        
        if (blockData.isReachable && blockData.currentBlockState == BlockState.IDLE)
        {
            var children = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer child in children)
            {
                child.material.color = colorReachable;
            }
        }
    }

    public override bool GrabBlock()
    {
        if(childBlocks.Count == 0) {
            if(blockData.currentBlockState != BlockState.DEPLETED)
            {
                SetBlockDepleted();
            }
            
            return false;
        }

        var block = childBlocks.Dequeue();
        Destroy(block.gameObject);
        DirtAdded?.Invoke();
        return true;
    }

}
