using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Minesweeper
{
    public class Grid : MonoBehaviour {

        public GameObject tilePrefab;
        public int width = 10, height = 10;
        public float spacing = .155f;

        private Tile[,] tiles;
        public Sprite flag;
        public Sprite noFlag;
        private bool flagged;
        AudioSource aS;
        public AudioClip explosion, relief, heartBeat;
        public Button playAgain;

        void Start()
        {
            aS = GetComponent<AudioSource>();
            GenerateTiles();
        }

        Tile SpawnTile(Vector3 pos)
        {
            GameObject clone = Instantiate(tilePrefab);
            clone.transform.position = pos;
            Tile currentTile = clone.GetComponent<Tile>();
            return currentTile;
        }

        void GenerateTiles()
        {
            tiles = new Tile[width, height];

            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    Vector2 halfSize = new Vector2(width * .5f, height * .5f);
                    Vector2 pos = new Vector2(x - halfSize.x, y - halfSize.y);
                    Vector2 offset = new Vector2(.5f, .5f);
                    pos += offset;
                    pos *= spacing;

                    Tile tile = SpawnTile(pos);

                    tile.transform.SetParent(transform);

                    tile.x = x;
                    tile.y = y;

                    tiles[x, y] = tile;
                }
            }
        }

        void FFuncover(int x, int y, bool[,] visited)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                if (visited[x, y])
                    return;

                Tile tile = tiles[x, y];
                int adjacentMines = GetAdjacentMineCount(tile);

                tile.Reveal(adjacentMines);

                if (adjacentMines == 0)
                {
                    visited[x, y] = true;

                    FFuncover(x - 1, y, visited);
                    FFuncover(x + 1, y, visited);
                    FFuncover(x, y - 1, visited);
                    FFuncover(x, y + 1, visited);
                }
            }
        }

        void UncoverMines(int mineState = 0)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Tile tile = tiles[x, y];
                    if(tile.isMine)
                    {
                        int adjacentMines = GetAdjacentMineCount(tile);
                        tile.Reveal(adjacentMines, mineState);
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
                    Tile tile = tiles[x, y];

                    if(!tile.isRevealed && !tile.isMine)
                    {
                        emptyTileCount++;
                    }
                }
            }
            return emptyTileCount == 0;
        }

        void SelectTile(Tile selected)
        {
            int adjacentMines = GetAdjacentMineCount(selected);

            selected.Reveal(adjacentMines);

            if(selected.isMine)
            {
                UncoverMines();
            }

            else if(adjacentMines == 0)
            {
                int x = selected.x;
                int y = selected.y;

                FFuncover(x, y, new bool[width, height]);
            }

            if(NoMoreEmptyTiles())
            {
                UncoverMines(1);
                playAgain.enabled = true;
            }
        }
        public void PlayAgain()
        {
            playAgain.enabled = false;
            SceneManager.LoadScene(0);
        }

     

        public int GetAdjacentMineCount(Tile tile)
        {
            int count = 0;

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int desiredX = tile.x + x;
                    int desiredY = tile.y + y;

                    if(desiredX < 0 || desiredX >= width || desiredY < 0 || desiredY >= height)
                    {
                        continue;
                    }

                    Tile currentTile = tiles[desiredX, desiredY];

                    if(currentTile.isMine)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public void PlaySound(int clip)
        {
            aS.Stop();
            switch(clip)
            {
                case 1:
                    aS.clip = relief;
                    break;
                case 2:
                    aS.clip = explosion;
                    break;
                case 3:
                    aS.clip = heartBeat;
                    break;
            }
            aS.Play();
        }

	    void Update () {
		    if(Input.GetMouseButtonUp(0))
            {
                aS.Stop();
                Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit2D hit = Physics2D.Raycast(mouseRay.origin, mouseRay.direction);
                if (hit.collider != null)
                {
                    Tile hitTile = hit.collider.GetComponent<Tile>();

                    if (hitTile != null)
                    {
                        if (!hitTile.flagged)
                        {
                            SelectTile(hitTile);
                        }
                    }
                }
            }
            if(Input.GetMouseButtonDown(0))
            {
                PlaySound(3);
            }
            if(Input.GetMouseButtonDown(1))
            {
                Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit2D hit = Physics2D.Raycast(mouseRay.origin, mouseRay.direction);
                if (hit.collider != null)
                {
                    Tile hitTile = hit.collider.GetComponent<Tile>();

                    if (hitTile != null)
                    {
                        if(!hitTile.flagged)
                        {
                            hitTile.rend.sprite = flag;
                            hitTile.flagged = true;
                        }
                        else
                        {
                            hitTile.rend.sprite = noFlag;
                            hitTile.flagged = false;
                        }
                    }
                }
            }
	    }
    }
}


