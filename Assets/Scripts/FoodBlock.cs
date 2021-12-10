using System;
using UnityEngine;

public class FoodBlock : Block
{
    public static event Action DirtAdded;

    private int foodUnits = 10;

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
        if (foodUnits <= 0)
        {
            if (blockData.currentBlockState != BlockState.DEPLETED)
            {
                SetBlockDepleted();
            }

            return false;
        }

        Home.AddFood();
        foodUnits--;
        DirtAdded?.Invoke();
        return true;
    }
}
