using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DirtBlock : Block, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public event Action GroundClumpDepleted;
    private bool depleted = false;

    [SerializeField] Color colorNotReachable = Color.white;
    [SerializeField] Color colorReachable = Color.white;
    [SerializeField] Color colorSelected = Color.white;
    [SerializeField] Color colorHover= Color.white;

    private Color lastColor = Color.white;

    public Queue<GameObject> blocks = new Queue<GameObject>();

    private void Awake()
    {
        for(int i =0; i<transform.childCount; i++)
        {
            blocks.Enqueue(transform.GetChild(i).gameObject);
        }
    }

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

    public override void Initialize(BlockData data)
    {
        blockData = data;
        if(blockData.currentBlockState == BlockState.DEPLETED)
        {
            depleted = true;
            gameObject.SetActive(false);
        }
        if(blockData.isReachable)
        {
            SetReachable(true);
            Debug.Log(transform.position);
        }
    }

    public override void SetReachable(bool reachable)
    {
        blockData.isReachable = reachable;
        if (reachable)
        {
            if(blockData.currentBlockState == BlockState.IDLE)
            {
                var children = GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer child in children)
                {
                    child.material.color = colorReachable;
                }
            }
        }
    }

    private void AddBlockToGatherQueue()
    {
        blockData.currentBlockState = BlockState.PENDING;

        GridManager.SetNeighborsReachable(blockData.position.x, blockData.position.y);

        var children = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer child in children)
        {
            child.material.color = colorSelected;
        }
    }
}
