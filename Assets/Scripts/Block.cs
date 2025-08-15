using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.Events;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Block : MonoBehaviour
{
    public string Letter;
    public Node Node;
    public TargetNode targetNode;
    public Block MergingBlock;
    public bool Merging;
    public Vector2 Pos => transform.position;

    [SerializeField] private TextMeshPro _text;
    //[SerializeField] private List<Node> snapTargets;

    private Vector3 offset;
    private Camera mainCamera;
    private readonly float snapRange = 0.5f;
    private Vector3 originalPos;
    private GameManager gameManager;

    public int dragCount = 0;
    public UnityEvent<Block> onDragStart;
    public UnityEvent<Block> onDrop;

    //[SerializeField] private List<string> targetWordLetters = new();
    //[SerializeField] private List<string> multipleLetters = new();


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        //snapTargets = gameManager._targetNodes;
        mainCamera = Camera.main;
    }


    public void Init(BlockType type)
    {
        Letter = type.Letter;
        _text.color = type.Color;
        _text.text = type.Letter;
    }


    public void SetBlock(Node node)
    {
        if (Node != null) Node.OccupiedBlock = null;
        Node = node;
        Node.OccupiedBlock = this;
    }


    public void SetBlockTargetNode(TargetNode node)
    {
        if (targetNode != null) targetNode.OccupiedTargetBlock = null;
        targetNode = node;
        targetNode.OccupiedTargetBlock = this;
        if(Node != null)
        {
            Node.OccupiedBlock = null;
            Node = null;
        }
    }


    void OnMouseDown()
    {
        originalPos = transform.position;
        offset = transform.position - GetMouseWorldPos();
        onDragStart?.Invoke(this);
    }


    void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + offset;
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 0.6f);
    }


    private void OnMouseUp()
    {
        TargetNode closestTarget = FindClosestSnapTarget();
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 1f);
        if (gameManager.targetWordLetters.Contains(Letter))
        {
            if (closestTarget != null)
            {
                if (closestTarget.OccupiedTargetBlock == null)
                {
                    if (gameManager.letterCount[Letter] > 0)
                    {
                        SnapToNode(closestTarget);
                        gameManager.letterCount[Letter] -= 1;
                        //Debug.Log(gameManager.letterCount[Letter]);
                    }
                    else if(gameManager._nodes.Contains(Node))
                    {
                        transform.position = originalPos;
                    }
                    else if(gameManager._targetNodes.Contains(targetNode))
                    {
                        SnapToNode(closestTarget);
                    }
                    dragCount++;
                    onDrop?.Invoke(this);
                }
                else if (gameManager._nodes.Contains(Node))
                {
                    transform.position = originalPos;
                }
                else
                {
                    ExchangeBlocks(closestTarget.OccupiedTargetBlock);
                }
            }
            else
            {
                transform.position = originalPos;
            }
        }
        else
        {
            transform.position = originalPos;
        }
        gameManager.WinConditionCheck();
    }


    //private bool CheckMultipleLetters()
    //{

    //}


    private void SnapToNode(TargetNode targetNode)
    {
        transform.position = targetNode.transform.position;
        SetBlockTargetNode(targetNode);
    }


    private void ExchangeBlocks(Block otherBlock)
    {
        // Temporarily store the current and other block's nodes
        TargetNode tempNodeThisBlock = this.targetNode;
        TargetNode tempNodeOtherBlock = otherBlock.targetNode;

        // Update the positions of the blocks
        this.transform.position = tempNodeOtherBlock.transform.position;
        otherBlock.transform.position = tempNodeThisBlock.transform.position;

        // Update the node references for each block
        this.SetBlockTargetNode(tempNodeOtherBlock);
        otherBlock.SetBlockTargetNode(tempNodeThisBlock);

        // Ensure the nodes now reference the correct blocks
        tempNodeThisBlock.OccupiedTargetBlock = otherBlock;
        tempNodeOtherBlock.OccupiedTargetBlock = this;
    }


    private TargetNode FindClosestSnapTarget()
    {
        TargetNode closest = null;
        float closestDistance = Mathf.Infinity;
        foreach(TargetNode target in gameManager._targetNodes)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < closestDistance && distance <= snapRange)
            {
                closest = target;
                closestDistance = distance;
            }
        }
        return closest;
    }


    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mainCamera.WorldToScreenPoint(gameObject.transform.position).z;
        return mainCamera.ScreenToWorldPoint(mousePoint);
    }


    public void MergeBlock(Block blockToMergeWith)
    {
        // Set the block we are merging with
        MergingBlock = blockToMergeWith;

        // Set current node as unoccupied to allow blocks to use it
        Node.OccupiedBlock = null;

        // Set the base block as merging, so it does not get used twice
        blockToMergeWith.Merging = true;
    }


    public bool CanMerge(string letter) => letter == Letter && !Merging && MergingBlock == null;
}

internal class SerealizeFieldAttribute : System.Attribute
{
}
