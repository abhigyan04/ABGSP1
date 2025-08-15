using UnityEngine;

public class Node : MonoBehaviour
{
    public Block OccupiedBlock;
    public SpriteRenderer _renderer;

    public Vector2 Pos => transform.position;
}
