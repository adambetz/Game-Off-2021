using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// IDLE - no action on block
// PENIND - in qction queue of a flock
// ACTIVE - currant action of a flock
// DEPLETED - inactive
public enum BlockState { IDLE, PENDING, ACTIVE, DEPLETED };
public enum BlockType { DIRT, FOOD };

public struct BlockData 
{
    public BlockState currentBlockState;
    public BlockType blockType;
    public Vector2Int position;
    public bool isReachable;
    public Block block;
}
