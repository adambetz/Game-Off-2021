using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : MonoBehaviour
{
    // IDLE - no action on block
    // PENIND - in qction queue of a flock
    // ACTIVE - currant action of a flock
    // DEPLETED - inactive
    public enum BlockState { IDLE, PENDING, ACTIVE, DEPLETED };
    public enum BlockType { DIRT, FOOD };

    public struct Data
    {
        public BlockState currentBlockState;
        public BlockType blockType;
        public bool reachable;
        public Block block;
    }
    
    public Data blockData;
    
    public abstract void Initialize(Data data);
}
