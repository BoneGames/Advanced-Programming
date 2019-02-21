using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Tile : MonoBehaviour {

        public int x, y;
        public bool isMine = false;
        public bool isRevealed = false;
        [Header("References")]
        public Sprite[] emptySprites;
        public Sprite[] mineSprites;
        public SpriteRenderer rend;
        public float mineChance;
        public bool flagged = false;
        Grid grid;

        void Awake()
        {
            rend = GetComponent<SpriteRenderer>();
            grid = FindObjectOfType<Grid>();
        }

        void Start () {
            isMine = Random.value < mineChance;
	    }

        public void Reveal(int adjacentMines, int mineState = 0)
        {
            isRevealed = true;

            if(isMine)
            {
                rend.sprite = mineSprites[mineState];
                grid.PlaySound(2);
            }
            else
            {
                rend.sprite = emptySprites[adjacentMines];
                grid.PlaySound(1);
            }
        }
    }
}


