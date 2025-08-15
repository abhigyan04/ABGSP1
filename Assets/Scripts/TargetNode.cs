using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TargetNode : MonoBehaviour
{
    public Block OccupiedTargetBlock;
    [SerializeField] private GameManager gameManager;
    public string targetLetter;
    public Image dot;
    public Vector2 Pos => transform.position;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        Assign();
    }

    private void Update()
    {
        UpdateDot();
    }

    public void Assign()
    {
        foreach(var node in gameManager._targetNodes)
        {
            if(this.transform.position == node.transform.position)
            {
                targetLetter = gameManager.targetWordLetters[gameManager._targetNodes.IndexOf(node)];
                dot = gameManager.dots[gameManager._targetNodes.IndexOf(node)];
            }
        }
    }

    public void UpdateDot()
    {
        if(OccupiedTargetBlock == null && !gameManager.lettersOnBoard.Contains(targetLetter))
        {
            dot.color = Color.white;
        }
        else if(OccupiedTargetBlock == null && gameManager.lettersOnBoard.Contains(targetLetter))
        {
            dot.color = Color.yellow;
        }
        else if(OccupiedTargetBlock != null && targetLetter == OccupiedTargetBlock.Letter)
        {
            dot.color = Color.green;
        }
        else if(OccupiedTargetBlock != null && targetLetter != OccupiedTargetBlock.Letter)
        {
            dot.color = Color.yellow;
        }
        else if(OccupiedTargetBlock == null)
        {
            foreach(TargetNode node in gameManager._targetNodes)
            {
                if(node.OccupiedTargetBlock != null)
                {
                    if (node.OccupiedTargetBlock.Letter == targetLetter)
                    {
                        dot.color = Color.yellow;
                    }
                }
            }
        }
    }
}
