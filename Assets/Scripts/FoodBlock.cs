using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBlock : Block
{
    public static event Action FoodAdded;

    private int foodUnits = 10;

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

        Home.FoodAmount++;
        foodUnits--;
        FoodAdded?.Invoke();
        return true;
    }
}
