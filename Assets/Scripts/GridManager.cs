using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    //must be greater than 5,5
    public static Vector2Int MapSize = new Vector2Int(100, 100);
    public static Vector3Int BlockSize = new Vector3Int (2,1,2);

    public static BlockData[,] map = new BlockData[MapSize.x, MapSize.y];

    public GameObject Queen;
    public GameObject DirtBlock;
    public GameObject FoodBlock;
    public GameObject DirtDropOff;
    public Vector2Int QueenLocation;
    public int foodAmout = 100;

    public Transform DirtHolder;

    private void Start()
    {
        GenerateMap();
        BuildMap();
    }

    public void GenerateMap()
    {
        Vector2Int middle = new Vector2Int(MapSize.x / 2, MapSize.y - 10);
        QueenLocation = middle;
        
        // initialize map
        for (int x=0; x<MapSize.x; x++)
        {
            for (int y=0; y<MapSize.y; y++)
            {
                BlockData initialData = new BlockData { currentBlockState = BlockState.IDLE, blockType = BlockType.DIRT, isReachable = false, position = new Vector2Int(x,y) };
                map[x,y] = initialData;
            }
        }

        // clear out queens location
        int xMax = middle.x + 2;
        int yMax = middle.y + 2;
        for (int x = middle.x-2; x <= xMax; x++)
        {
            for (int y = middle.y - 2; y <= yMax; y++)
            {
                map[x,y].currentBlockState = BlockState.DEPLETED;
            }
        }

        // clear tunnel to surface
        for (int y = MapSize.y-1; y >= yMax; y--)
        {
            map[middle.x, y].currentBlockState = BlockState.DEPLETED;
        }

        // add food 
        int foodRemaining = foodAmout;
        while (foodRemaining > 0)
        {
            int xRand = Random.Range(0, MapSize.x - 1);
            int yRand = Random.Range(0, MapSize.y - 1);
            if (map[xRand, yRand].currentBlockState == BlockState.IDLE && map[xRand, yRand].blockType != BlockType.FOOD)
            {
                map[xRand, yRand].blockType = BlockType.FOOD;
                foodRemaining--;
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
                    var queen = Instantiate(Queen, new Vector3(x*BlockSize.x - offsetX, 0, z*BlockSize.z - offsetZ), Quaternion.identity);
                    queen.GetComponent<Home>().DirtDropOff = Instantiate(DirtDropOff, new Vector3(x * BlockSize.x - offsetX, 0, MapSize.y * BlockSize.z - offsetZ), Quaternion.identity);
                }
                else if( map[x,z].blockType == BlockType.DIRT )
                {
                    var blockInstance = Instantiate(DirtBlock, new Vector3(x * BlockSize.x - offsetX, 0, z * BlockSize.z - offsetZ), Quaternion.identity);
                    var block = blockInstance.GetComponent<Block>();
                    block.Initialize(map[x, z]);
                    map[x,z].block = block;
                    blockInstance.transform.parent = DirtHolder;
                }
                else if (map[x, z].blockType == BlockType.FOOD)
                {
                    var blockInstance = Instantiate(FoodBlock, new Vector3(x * BlockSize.x - offsetX, 0, z * BlockSize.z - offsetZ), Quaternion.identity);
                    var block = blockInstance.GetComponent<Block>();
                    block.Initialize(map[x, z]);
                    map[x, z].block = block;
                    blockInstance.transform.parent = DirtHolder;
                }

                if(map[x,z].currentBlockState == BlockState.DEPLETED)
                {
                    SetNeighborsReachable(x, z);
                }
            }
        }

        // set reachable blocks
        for (int x = 0; x < MapSize.x; x++)
        {
            for (int z = 0; z < MapSize.y; z++)
            {
                if (map[x, z].currentBlockState == BlockState.DEPLETED)
                {
                    SetNeighborsReachable(x, z);
                }
            }
        }
    }

    public static void SetNeighborsReachable(int posX, int posY)
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

    private static void SetBlockReachable(BlockData block)
    {
        if(block.currentBlockState != BlockState.IDLE) return;
        if (!block.isReachable)
        {
            if(block.block != null)
            {
                block.block.SetReachable(true);
            }
            else
            {
                block.isReachable =  true;
            }
        }
    }
}
