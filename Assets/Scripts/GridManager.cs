using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    //must be greater than 5,5
    public static Vector2Int MapSize = new Vector2Int(100, 100);
    public static Vector3Int BlockSize = new Vector3Int (2,1,2);

    public GameObject Queen;
    public GameObject DirtBlock;
    public Vector2Int QueenLocation;

    private Block.Data[,] map = new Block.Data[MapSize.x, MapSize.y];

    private void Start()
    {
        GenerateMap();
        BuildMap();
    }

    public void GenerateMap()
    {
        Vector2Int middle = new Vector2Int(MapSize.x / 2, MapSize.y / 2);
        QueenLocation = middle;

        // initialize map
        for (int x=0; x<MapSize.x; x++)
        {
            for (int y=0; y<MapSize.y; y++)
            {
                Block.Data initialData = new Block.Data { currentBlockState = Block.BlockState.IDLE, blockType = Block.BlockType.DIRT, reachable = false };
                map[x,y] = initialData;
            }
        }

        // clear out queens location
        int xMax = middle.x + 2;
        int yMax = middle.y + 2;
        for (int x = middle.x-1; x < xMax; x++)
        {
            for (int y = middle.y - 1; y < yMax; y++)
            {
                map[x,y].currentBlockState = Block.BlockState.DEPLETED;
                SetNeighborsReachable(x,y);
            }
        }
    }

    public void BuildMap()
    {
        int offsetX = QueenLocation.x * BlockSize.x;
        int offsetZ = QueenLocation.y * BlockSize.z;

        for (int x = 0; x < MapSize.x; x++)
        {
            for (int z = 0; z < MapSize.y; z++)
            {
                if(QueenLocation.x == x && QueenLocation.y == z)
                {
                    Instantiate(Queen, new Vector3(x*BlockSize.x - offsetX, 0, z*BlockSize.z - offsetZ), Quaternion.identity);
                }
                else if( map[x,z].blockType == Block.BlockType.DIRT )
                {
                    var blockInstance = Instantiate(DirtBlock, new Vector3(x * BlockSize.x - offsetX, 0, z * BlockSize.z - offsetZ), Quaternion.identity);
                    var block = blockInstance.GetComponent<Block>();
                    block.Initialize(map[x, z]);
                    map[x,z].block = block; 
                }
                else if (map[x, z].blockType == Block.BlockType.FOOD)
                {
                    // TODO
                }
            }
        }
    }

    public void SetNeighborsReachable(int posX, int posY)
    {
        Vector2Int above = new Vector2Int(posX, posY + 1);
        Vector2Int below = new Vector2Int(posX, posY - 1);
        Vector2Int before = new Vector2Int(posX - 1, posY);
        Vector2Int after = new Vector2Int(posX + 1, posY);

        if(above.y < MapSize.y)
        {
            SetBlockReachable(map[above.x, above.y]);
        }
        if(below.y >= 0)
        {
            SetBlockReachable(map[below.x, below.y]);
        }
        if(before.x >= 0)
        {
            SetBlockReachable(map[before.x, before.y]);
        }
        if(after.x < MapSize.x)
        {
            SetBlockReachable(map[after.x, after.y]);
        }
    }

    private void SetBlockReachable(Block.Data block)
    {
        if (!block.reachable)
        {
            block.reachable = true;
        }
    }
}
