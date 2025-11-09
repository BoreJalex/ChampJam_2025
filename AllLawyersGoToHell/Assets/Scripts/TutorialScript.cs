using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    [SerializeField] public GameObject blockPrefab;

    // Trackers
    [HideInInspector] public GameObject currentBox = null;
    [HideInInspector] public int currentIndex;

    // Sounds
    [SerializeField] private AudioClip stamp;
    [SerializeField] private AudioClip buttonSwap;

    private GameObject topBlock;
    private GameObject middleBlock;
    private GameObject bottomBlock;

    [SerializeField] public Sprite defaultBlock;
    [SerializeField] public Sprite outlineBlock;

    public GameObject[] blockList;

    private void Start()
    {
        topBlock = Instantiate(blockPrefab, new Vector3(4, 2.2f, 0f), Quaternion.identity);
        middleBlock = Instantiate(blockPrefab, new Vector3(4, 0f, 0f), Quaternion.identity);
        bottomBlock = Instantiate(blockPrefab, new Vector3(4, -2.2f, 0f), Quaternion.identity);
        topBlock.AddComponent<TutorialBoxScript>();
        middleBlock.AddComponent<TutorialBoxScript>();
        bottomBlock.AddComponent<TutorialBoxScript>();
        topBlock.GetComponent<TutorialBoxScript>().Initialize(this , 0);
        middleBlock.GetComponent<TutorialBoxScript>().Initialize(this, 1);
        bottomBlock.GetComponent<TutorialBoxScript>().Initialize(this, 2);
        blockList[2] = topBlock;
        blockList[1] = middleBlock;
        blockList[0] = bottomBlock;
        currentBox = blockList[1];
        currentIndex = 1;
    }

    private void Update()
    {
        if (currentBox == topBlock)
        {
            topBlock.GetComponent<SpriteRenderer>().sprite = outlineBlock;
            middleBlock.GetComponent<SpriteRenderer>().sprite = defaultBlock;
            bottomBlock.GetComponent<SpriteRenderer>().sprite = defaultBlock;
        }
        else if (currentBox == middleBlock)
        {
            topBlock.GetComponent<SpriteRenderer>().sprite = defaultBlock;
            middleBlock.GetComponent<SpriteRenderer>().sprite = outlineBlock;
            bottomBlock.GetComponent<SpriteRenderer>().sprite = defaultBlock;
        }
        else if (currentBox == bottomBlock)
        {
            topBlock.GetComponent<SpriteRenderer>().sprite = defaultBlock;
            bottomBlock.GetComponent<SpriteRenderer>().sprite = outlineBlock;
            middleBlock.GetComponent<SpriteRenderer>().sprite = defaultBlock;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentIndex != 2)
            {
                currentBox = blockList[currentIndex + 1];
                currentIndex++;

                float randomPitch = UnityEngine.Random.Range(.8f, 1.2f);
                GameManager.Instance.PlaySound(buttonSwap, randomPitch);
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentIndex != 0)
            {
                currentBox = blockList[currentIndex - 1];
                currentIndex--;

                float randomPitch = UnityEngine.Random.Range(.8f, 1.2f);
                GameManager.Instance.PlaySound(buttonSwap, randomPitch);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentBox.GetComponent<TutorialBoxScript>().stamped == true) return;

            currentBox.GetComponent<TutorialBoxScript>().stamped = true;
            currentBox.GetComponent<SpriteRenderer>().sortingOrder = -5;
            currentBox.transform.GetChild(1).GetComponent<Canvas>().sortingOrder = -4;
            currentBox.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = -3;

            float randomPitch = UnityEngine.Random.Range(.8f, 1.2f);
            GameManager.Instance.PlaySound(stamp, randomPitch);

            if (currentBox == topBlock)
            {
                topBlock = Instantiate(blockPrefab, new Vector3(4, 2.2f, 0f), Quaternion.identity);
                topBlock.AddComponent<TutorialBoxScript>();
                topBlock.GetComponent<TutorialBoxScript>().Initialize(this, 0);
                blockList[2] = topBlock;
                currentBox = topBlock;
            }
            else if (currentBox == middleBlock)
            {
                middleBlock = Instantiate(blockPrefab, new Vector3(4, 0f, 0f), Quaternion.identity);
                middleBlock.AddComponent<TutorialBoxScript>();
                middleBlock.GetComponent<TutorialBoxScript>().Initialize(this, 1);
                blockList[1] = middleBlock;
                currentBox = middleBlock;
            }
            else if (currentBox == bottomBlock)
            {
                bottomBlock = Instantiate(blockPrefab, new Vector3(4, -2.2f, 0f), Quaternion.identity);
                bottomBlock.AddComponent<TutorialBoxScript>();
                bottomBlock.GetComponent<TutorialBoxScript>().Initialize(this, 2);
                blockList[0] = bottomBlock;
                currentBox = bottomBlock;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentBox != null)
            {
                if (currentBox.GetComponent<TutorialBoxScript>().stamped) return;

                // Sorting orders
                currentBox.GetComponent<SpriteRenderer>().sortingOrder = 5;
                currentBox.transform.GetChild(1).GetComponent<Canvas>().sortingOrder = 6;

                // Random Values for speed and rotation
                float heightForce = UnityEngine.Random.Range(10f, 20f);
                float sidewaysForce = UnityEngine.Random.Range(10f, 20f);
                float rotationForce = UnityEngine.Random.Range(-80f, -120f);

                // Chucking
                Rigidbody2D rb = currentBox.GetComponent<Rigidbody2D>();
                SpriteRenderer rend = currentBox.GetComponent<SpriteRenderer>();
                Vector3 boxDirection = new Vector3(sidewaysForce, heightForce, 0);
                rb.AddForce(boxDirection, ForceMode2D.Impulse);
                rb.gravityScale = 5;
                rb.AddTorque(rotationForce);

                if (currentBox == topBlock)
                {
                    topBlock = Instantiate(blockPrefab, new Vector3(4, 2.2f, 0f), Quaternion.identity);
                    topBlock.AddComponent<TutorialBoxScript>();
                    topBlock.GetComponent<TutorialBoxScript>().Initialize(this, 0);
                    blockList[2] = topBlock;
                    currentBox = topBlock;
                }
                else if (currentBox == middleBlock)
                {
                    middleBlock = Instantiate(blockPrefab, new Vector3(4, 0f, 0f), Quaternion.identity);
                    middleBlock.AddComponent<TutorialBoxScript>();
                    middleBlock.GetComponent<TutorialBoxScript>().Initialize(this, 1);
                    blockList[1] = middleBlock;
                    currentBox = middleBlock;
                }
                else if (currentBox == bottomBlock)
                {
                    bottomBlock = Instantiate(blockPrefab, new Vector3(4, -2.2f, 0f), Quaternion.identity);
                    bottomBlock.AddComponent<TutorialBoxScript>();
                    bottomBlock.GetComponent<TutorialBoxScript>().Initialize(this, 2);
                    blockList[0] = bottomBlock;
                    currentBox = bottomBlock;
                }
            }
        }
    }
}
