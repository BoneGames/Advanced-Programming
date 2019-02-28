using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MineSweeper3D
{
    public class Cell : MonoBehaviour
    {
        public int x, y, z;
        public bool isMine = false;
        public bool isRevealed = false;
        [Range(0, 1)]
        public float mineChance = 0.15f;
        public GameObject minePrefab, textPrefab;

        private GameObject mine, text;
        private Collider col;
        private Animator anim;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            col = GetComponent<Collider>();
        }

        void Start()
        {
            // Set Mine Chance
            isMine = Random.value < mineChance;
            if (isMine)
            {
                // Create instance of mine gameobject
                mine = Instantiate(minePrefab, transform);
                mine.SetActive(false);
            }
            else
            {
                // create instance of text object
                text = Instantiate(textPrefab, transform);
                text.SetActive(false);
            }
        }

        public void Reveal(int adjacentMines, int mineState = 0)
        {
            col.enabled = false;
            // Flags the tile as being revealed
            isRevealed = true;
            // Check is tile is mine
            anim.SetTrigger("Reveal");
            if (isMine)
            {
                // Run Reveal anim
                mine.SetActive(true);
            }
            else
            {
                // enable text
                text.SetActive(true);
                // setting text
                text.GetComponent<TextMeshPro>().text = adjacentMines.ToString();
            }
        }

        public void OnMouseDown()
        {
            Reveal(1);
            Debug.Log(transform.position);
        }
    }

}