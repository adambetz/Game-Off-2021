using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DirtBlock : Block, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // if block selectable
                // add to queue
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            // cancel
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
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

    public override void Initialize(Data data)
    {
        blockData = data;
        if(blockData.currentBlockState == BlockState.DEPLETED)
        {
            depleted = true;
            gameObject.SetActive(false);
        }
    }
}
