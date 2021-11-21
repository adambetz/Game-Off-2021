using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    //must be greater than 5,5
    public static Vector2Int MapSize = new Vector2Int(100, 100);
    public static Vector3Int BlockSize = new Vector3Int (1,1,1);

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
                Block.Data initialData = new Block.Data { currentBlockState = Block.BlockState.IDLE, blockType = Block.BlockType.DIRT };
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
                Debug.Log(x + " " +y);
                map[x,y].currentBlockState = Block.BlockState.DEPLETED;
            }
        }
    }

    public void BuildMap()
    {
        int offsetX = QueenLocation.x;
        int offsetZ = QueenLocation.y;

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
                    var block = Instantiate(DirtBlock, new Vector3(x * BlockSize.x - offsetX, 0, z * BlockSize.z - offsetZ), Quaternion.identity);
                    block.GetComponent<Block>().Initialize(map[x,z]);
                }
                else if (map[x, z].blockType == Block.BlockType.FOOD)
                {
                    // TODO
                }
            }
        }
    }
}
