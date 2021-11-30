using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DirtBlock : Block//, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
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

    /*
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (blockData.isReachable && blockData.currentBlockState == BlockState.IDLE)
            {
                AddBlockToGatherQueue();
            }  
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            // cancel
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(blockData.isReachable && blockData.currentBlockState == BlockState.IDLE)
        {
            var children = GetComponentsInChildren<MeshRenderer>();
            lastColor = children[0].material.color;
            foreach (MeshRenderer child in children)
            {
                child.material.color = colorHover;
            }
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (blockData.isReachable && blockData.currentBlockState == BlockState.IDLE)
        {
            var children = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer child in children)
            {
                child.material.color = lastColor;
            }
        }
    }
    */

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
        /*var children = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer child in children)
        {
            child.material.color = lastColor;
        }*/

        
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
