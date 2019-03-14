using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MineSweeper3D2
{
    public class Grid : MonoBehaviour
    {

        public GameObject cellPrefab;
        public int width, height, depth = 10;
        public float spacing = 1.1f;
        private Cell[,,] cells;

        // Use this for initialization
        void Start()
        {
            GenerateCells();
        }

        void Update()
        {
            MouseOver();
            //UpdateGrid();
        }

        Cell GetHitCell(Vector2 mousePosition)
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(camRay, out hit))
            {
                return hit.collider.GetComponent<Cell>();
            }
            return null;
        }

        void MouseOver()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Cell hitCell = GetHitCell(Input.mousePosition);
                if (hitCell)
                {
                    SelectTile(hitCell);
                }
            }
            if (Input.GetMouseButtonDown(2))
            {
                Debug.Log("1");
                Cell hitCell = GetHitCell(Input.mousePosition);
                if (hitCell)
                {
                    hitCell.Flag();
                }
            }
        }

        void UpdateGrid()
        {
            // Store half the size of the grid
            Vector3 halfSize = new Vector3(width * .5f, height * .5f, depth * .5f);

            // Offset
            Vector3 offset = new Vector3(.5f, .5f, .5f);

            // Loop through the entire list of tiles
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        // Generate position for current tile
                        Vector3 position = new Vector3(x - halfSize.x,
                                                       y - halfSize.y,
                                                       z - halfSize.z);
                        // Offset position to center
                        position += offset;
                        // Apply spacing
                        position *= spacing;
                        // Spawn a new tile
                        Cell cell = cells[x, y, z];


                        cell.transform.position = position;
                    }
                }
            }
        }

        void GenerateCells()
        {
            // Instantiate new 3D array of size width x height x depth
            cells = new Cell[width, height, depth];

            // Store half size of grid
            Vector3 halfSize = new Vector3(width, height, depth) * .5f;

            Vector3 offset = new Vector3(.5f, .5f, .5f);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        // Generate position for current Cell
                        Vector3 position = new Vector3(x, y, z) - halfSize;

                        // Apply Spacing
                        position *= spacing;
                        position += offset;

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

        Cell SpawnCell(Vector3 position)
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

        bool IsOutOfBounds(int x, int y, int z)
        {
            return x < 0 || x >= width ||
                   y < 0 || y >= height ||
                   z < 0 || z >= depth;
        }

        void UncoverMines(int mineState = 0)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        Cell cell = cells[x, y, z];
                        if (cell.isMine)
                        {
                            int adjacentMines = GetAdjacentMineCount(cell);
                            cell.Reveal (mineState);
                        } 
                    }
                }
            }
        }

        bool NoMoreEmptyTiles()
        {
            int emptyTileCount = 0;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        Cell cell = cells[x, y, z];

                        if (!cell.isRevealed && !cell.isMine)
                        {
                            emptyTileCount++;
                        } 
                    }
                }
            }
            return emptyTileCount == 0;
        }

        void FFuncover(int x, int y, int z, bool[,,] visited)
        {
            // Is x and y out of bounds of the grid?
            if (IsOutOfBounds(x, y, z))
            {
                // Exit
                return;
            }

            // Have the coordinates already been visited?
            if (visited[x, y, z])
            {
                // Exit
                return;
            }
            // Reveal that tile in that X and Y coordinate
            Cell cell = cells[x, y, z];
            // Get number of mines around that tile
            int adjacentMines = GetAdjacentMineCount(cell);
            // Reveal the tile
            cell.Reveal(adjacentMines);

            // If there are no adjacent mines around that tile
            if (adjacentMines == 0)
            {
                // This tile has been visited
                visited[x, y, z] = true;
                // Visit all other tiles around this tile
                
                FFuncover(x - 1, y, z, visited);
                FFuncover(x + 1, y, z, visited);

                FFuncover(x, y - 1, z, visited);
                FFuncover(x, y + 1, z, visited);

                FFuncover(x, y, z - 1, visited);
                FFuncover(x, y, z + 1, visited);
                
            }
        }

        public int GetAdjacentMineCount(Cell cell)
        {
            int count = 0;

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        int desiredX = cell.x + x;
                        int desiredY = cell.y + y;
                        int desiredZ = cell.z + z;
                        //Debug.Log("cell checked");

                        if (IsOutOfBounds(desiredX, desiredY, desiredZ))
                        {

                            continue;

                        }

                        Cell currentCell = cells[desiredX, desiredY, desiredZ];

                        if (currentCell.isMine)
                        {
                            count++;
                        }

                    }

                }
            }
            return count;
        }

        void SelectTile(Cell selected)
        {
            int adjacentMines = GetAdjacentMineCount(selected);

            selected.Reveal(adjacentMines);

            if (selected.isMine)
            {
                UncoverMines();
            }

            else if (adjacentMines == 0)
            {
                int x = selected.x;
                int y = selected.y;
                int z = selected.z;

                FFuncover(x, y, z, new bool[width, height, depth]);
            }

            if (NoMoreEmptyTiles())
            {
                UncoverMines(1);
            }
        }
       

    } 
}

