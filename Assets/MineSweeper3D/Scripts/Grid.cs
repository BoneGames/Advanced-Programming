using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MineSweeper3D
{
    public class Grid : MonoBehaviour
    {

        public GameObject cellPrefab;
        public int width, height, depth = 10;
        public Vector3 spacing;

        private Cell[,,] cells;

        // Use this for initialization
        void Start()
        {
            GenerateCells();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void GenerateCells()
        {
            // Instantiate new 3D array of size width x height x depth
            cells = new Cell[width, height, depth];

            // Store half size of grid
            Vector3 halfSize = new Vector3(width, height, depth) * .5f;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        // Generate position for current Cell
                        Vector3 position = new Vector3(x, y, z) - halfSize;

                        // Apply Spacing
                        position += spacing;

                        // Spawn the Cell
                        Cell newCell = SpawnCell(position);
                        // Store Coordinates
                        newCell.x = x;
                        newCell.y = y;
                        newCell.z = z;
                        // Store tile in array at those coordinates
                        cells[x, y, z] = newCell;
                    }
                }
            }
        }

        Cell SpawnCell (Vector3 position)
        {
            // Clone ile prefab
            GameObject clone = Instantiate(cellPrefab);
            // Edit it's properties
            clone.transform.position = position;
            // Set cell parent to Grid Object
           // clone.transform.SetParent(transform);
            // Return the tile component of clone
            return clone.GetComponent<Cell>();
        }
    } 
}
