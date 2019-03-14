using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MineSweeper3D2
{
    public class Cell : MonoBehaviour
    {

        public int x, y, z; // Coordinate in 3D array of grid
        public bool isMine = false, isRevealed = false;
        public GameObject minePrefab, textPrefab;
        public Gradient textGradient;

        public Color flagColor;
        private Color originalColor;
        public bool isFlagged;

        private Material originalMat;
        public Material flagMat;

        [Range(0, 1)]
        public float mineChance = 0.15f;

        // Reference to components
        private Animator anim;
        private Collider col;
        private Renderer rend;
        private GameObject mine, text;

        void Awake()
        {
            rend = GetComponentInChildren<Renderer>();
            anim = GetComponent<Animator>();
            col = GetComponent<Collider>();
        }

        void Start()
        {
            originalMat = rend.material;
            originalColor = rend.material.color;
            // set mine chance
            isMine = Random.value < mineChance;
            // check is tile is mine
            if (isMine)
            {
                // Spawn mine gambeobject as child
                mine = SpawnChild(minePrefab);
            }
            else
            {
                // Spawn text gameobject as child
                text = SpawnChild(textPrefab);
            }
        }

        GameObject SpawnChild(GameObject prefab)
        {
            // Spawn prefab and attach to self (transform)
            GameObject child = Instantiate(prefab, transform);
            // Centres Child
            child.transform.localPosition = Vector3.zero;
            // Deactivates child
            child.SetActive(false);
            return child;
        }

        public void Flag()
        {
            Debug.Log(gameObject.transform.position);
            // Toggle flagged
            isFlagged = !isFlagged;
            // Change the material
            rend.material = isFlagged ? originalMat : flagMat;
            //rend.material.color = isFlagged ? flagColor : originalColor;
        }

        public void Reveal(int adjacentMines = 0)
        {
            isRevealed = true;
            anim.SetTrigger("Reveal");
            col.enabled = false;
            // is this a mine?
            if (isMine)
            {
                // activate
                mine.SetActive(true);
            }
            else
            {
                if (adjacentMines > 0)
                {
                    // Enable Text
                    text.SetActive(true);
                    TextMeshPro tmp = text.GetComponent<TextMeshPro>();
                    // Setting text color
                    float step = adjacentMines / 9f;
                    tmp.color = textGradient.Evaluate(step);
                    // Setting text value
                    tmp.text = adjacentMines.ToString();
                }
            }
        }
    }
}