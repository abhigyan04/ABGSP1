using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening.Core.Easing;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private Node _nodePrefab;
    [SerializeField] private TargetNode _targetNodePrefab;
    [SerializeField] private Block _blockPrefab;
    [SerializeField] private SpriteRenderer _boardPrefab;
    [SerializeField] private List<BlockType> _types;
    [SerializeField] private float _travelTime = 0.2f;
    public List<Node> _nodes = new();
    public List<TargetNode> _targetNodes = new();
    [SerializeField] private List<Block> _blocks;
    [SerializeField] private List<Block> _targetBlocks;
    [SerializeField] private TextMeshProUGUI _nextLetter;
    [SerializeField] private TextMeshProUGUI _score;
    [SerializeField] private TextMeshProUGUI _highScore;

    private GameState _state;
    private int _round;
    public static int difficulty = 0;
    [SerializeField] private string[] _startingLetters = new String[] { "A"};
    private readonly List<String> _letter = new() { "A" };
    private readonly System.Random _random = new();
    private Vector2 _startTouchPosition, _endTouchPosition;
    private readonly float _swipeThreshold = 50f;
    private Bounds _boardBounds;
    [SerializeField] private Camera _mainCamera;
    //private readonly Queue<string> _lettersQueue = new();
    //private readonly int _initialLettersCount = 5;
    //private readonly int _refillThreshold = 1;
    private bool merging = true;
    
    public string _targetWord;
    public int score;
    private int moveOrMerge = 0;
    public GameObject popupPanel;
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject homeButton;
    public GameObject blackOut;
    public TextMeshProUGUI winScore;
    public TextMeshProUGUI loseScore;
    public Image dot1;
    public Image dot2;
    public Image dot3;
    public Image dot4;
    public List<Image> dots;
    public AudioSource swipeSound;
    public AudioSource winSound;
    public AudioSource loseSound;

    public List<string> targetWordLetters = new();
    public Dictionary<string, int> letterCount = new();
    public List<string> lettersSpawned = new();
    public List<string> lettersOnBoard = new();
    private readonly List<String> updateOnLetters = new() { "D", "G", "J", "M", "P", "S", "V", "Y" };

    private BlockType GetBlockTypeByLetter(string letter) => _types.FirstOrDefault(t => t.Letter == letter);
    private readonly List<GridState> gridStates = new();


    private void Start()
    {
        //GenerateUpcomingLettersSequence(_initialLettersCount);

        ChangeState(GameState.GenerateLevel);
    }


    IEnumerator WinWaiter()
    {
        winSound.Play();
        yield return new WaitForSeconds(0.8f);
        ChangeState(GameState.Win);
    }

    IEnumerator LoseWaiter()
    {
        loseSound.Play();
        yield return new WaitForSeconds(0.8f);
        ChangeState(GameState.Lose);
    }


    private void ChangeState(GameState newState)
    {
        _state = newState;

        switch (newState)
        {
            case GameState.GenerateLevel:
                GenerateGrid();
                break;
            case GameState.SpawningBlocks:
                //Debug.Log("Spawning blocks");
                SpawnBlocks(_round++ == 0 ? 2 : 1);
                break;
            case GameState.WaitingInput:
                //Debug.Log("Waiting input");
                break;
            case GameState.Shifting:
                break;
            case GameState.MovingBlockToTarget:
                break;
            case GameState.Win:
                winScore.SetText("Score:" + score.ToString());
                winPanel.SetActive(true);
                homeButton.SetActive(false);
                blackOut.SetActive(true);
                break;
            case GameState.Lose:
                loseScore.SetText("Score:" + score.ToString());
                losePanel.SetActive(true);
                homeButton.SetActive(false);
                blackOut.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }


    void Update()
    {
        if (_state != GameState.WaitingInput) return;

        DetectTouch();

        if (!popupPanel.activeInHierarchy && !winPanel.activeInHierarchy && !losePanel.activeInHierarchy && !blackOut.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) Shift(Vector2.left);
            if (Input.GetKeyDown(KeyCode.RightArrow)) Shift(Vector2.right);
            if (Input.GetKeyDown(KeyCode.UpArrow)) Shift(Vector2.up);
            if (Input.GetKeyDown(KeyCode.DownArrow)) Shift(Vector2.down);
        }
    }


    private void DetectTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchWorldPos = _mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, _mainCamera.nearClipPlane));

            // Adjust Z to match the board for comparison
            touchWorldPos.z = _boardBounds.center.z;

            if (!_boardBounds.Contains(touchWorldPos))
            {
                // If the touch is outside the board, ignore this touch
                //Debug.Log("Invalid");
                return;
            }

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _startTouchPosition = touch.position;
                    //Debug.Log(_startTouchPosition);
                    break;

                case TouchPhase.Ended:
                    _endTouchPosition = touch.position;
                    //Debug.Log(_endTouchPosition);
                    DetectSwipe();
                    break;
            }
        }
    }


    private void DetectSwipe()
    {
        // Assuming _startTouchPosition and _endTouchPosition are set if within bounds
        if (Vector2.Distance(_startTouchPosition, _endTouchPosition) >= _swipeThreshold)
        {
            Vector2 swipeDirection = _endTouchPosition - _startTouchPosition;

            if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
            {
                // Horizontal swipe
                if (swipeDirection.x > 0)
                    OnSwipeRight();
                else
                    OnSwipeLeft();
            }
            else
            {
                // Vertical swipe
                if (swipeDirection.y > 0)
                    OnSwipeUp();
                else
                    OnSwipeDown();
            }
        }
    }


    private void OnSwipeRight()
    {
        // Call your function to shift blocks right
        Shift(Vector2.right);
    }


    private void OnSwipeLeft()
    {
        // Call your function to shift blocks left
        Shift(Vector2.left);
    }


    private void OnSwipeUp()
    {
        // Call your function to shift blocks up
        Shift(Vector2.up);
    }


    private void OnSwipeDown()
    {
        // Call your function to shift blocks down
        Shift(Vector2.down);
    }

    
    void GenerateGrid()
    {
        _round = 0;
        UpdateScoreUI(score = 0);
        UpdateHighScoreUI();
        _blocks = new List<Block>();

        var center = new Vector2((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f);

        float screenAspect = (float)Screen.width / (float)Screen.height;
        float targetAspect = 9f / 16f;
        float camScale = targetAspect / screenAspect;

        var board = Instantiate(_boardPrefab, new Vector2(center.x, center.y-0.25f), Quaternion.identity);
        board.size = new Vector2((float)_width, (float)_height);
        _boardBounds = board.GetComponent<SpriteRenderer>().bounds;
        board.gameObject.SetActive(false);
        //Debug.Log(_boardBounds);

        Camera.main.transform.position = new Vector3(center.x, center.y, -10);
        Camera.main.orthographicSize = (10f / 2f) * camScale;

        for (int x = 0; x < _width; x++)
        {
            for (float y = (float)-0.45; y < _height-1; y++)
            {
                var node = Instantiate(_nodePrefab, new Vector2(x, y-0.25f), Quaternion.identity);
                
                _nodes.Add(node);
            }
        }

        for(int x = (int)0.5f; x < _width - 1; x++)
        {
            var targetNode = Instantiate(_targetNodePrefab, new Vector2(x+0.5f, _height), Quaternion.identity);
            targetNode.tag = "TargetNode";
            _targetNodes.Add(targetNode);
        }

        ChangeState(GameState.SpawningBlocks);
    }


    //private void GenerateUpcomingLettersSequence(int count)
    //{
    //    for (int i = 0; i < count; i++)
    //    {
    //        UpdateLetterSpawnChancess();
    //        string letter = _startingLetters[_random.Next(_startingLetters.Length)];
    //        string letter = LetterToBeSpawned();
    //        _lettersQueue.Enqueue(letter);
    //    }
    //}


    void UpdateLetterSpawnChances(string letter)
    {
        //char highestLetter = lettersSpawned.Count != 0 ? char.Parse(lettersSpawned.LastOrDefault()) : 'B';
        //Debug.Log(highestLetter);
        switch (letter)
        {
            case "D":
                if (!_letter.Contains("B"))
                {
                    _letter.Add("B");
                }
                _startingLetters = _letter.ToArray();
                break;
            case "G":
                if (!_letter.Contains("C"))
                {
                    _letter.Add("C");
                }
                var countA = lettersOnBoard.Count(str => str == "A");
                if (countA % 2 == 0)
                {
                    _letter.Remove("A");
                    _startingLetters = _letter.ToArray();
                }
                else
                {
                    var freeNodes = _nodes.Where(n => n.OccupiedBlock == null).OrderBy(b => UnityEngine.Random.value).ToList();
                    foreach (var node in freeNodes.Take(1))
                    {
                        SpawnBlock(node, "A");
                        _letter.Remove("A");
                        _startingLetters = _letter.ToArray();
                        ChangeState(GameState.WaitingInput);
                    }
                }
                break;
            case "J":
                if (!_letter.Contains("D"))
                {
                    _letter.Add("D");
                }
                var countB = lettersOnBoard.Count(str => str == "B");
                if (countB % 2 == 0)
                {
                    _letter.Remove("B");
                    _startingLetters = _letter.ToArray();
                }
                else
                {
                    var freeNodes = _nodes.Where(n => n.OccupiedBlock == null).OrderBy(b => UnityEngine.Random.value).ToList();
                    foreach (var node in freeNodes.Take(1))
                    {
                        SpawnBlock(node, "B");
                        _letter.Remove("B");
                        _startingLetters = _letter.ToArray();
                        ChangeState(GameState.WaitingInput);
                    }
                }
                break;
            case "M":
                if (!_letter.Contains("E"))
                {
                    _letter.Add("E");
                }
                var countC = lettersOnBoard.Count(str => str == "C");
                if (countC % 2 == 0)
                {
                    _letter.Remove("C");
                    _startingLetters = _letter.ToArray();
                }
                else
                {
                    var freeNodes = _nodes.Where(n => n.OccupiedBlock == null).OrderBy(b => UnityEngine.Random.value).ToList();
                    foreach (var node in freeNodes.Take(1))
                    {
                        SpawnBlock(node, "C");
                        _letter.Remove("C");
                        _startingLetters = _letter.ToArray();
                        ChangeState(GameState.WaitingInput);
                    }
                }
                break;
            case "P":
                if (!_letter.Contains("F"))
                {
                    _letter.Add("F");
                }
                var countD = lettersOnBoard.Count(str => str == "D");
                if (countD % 2 == 0)
                {
                    _letter.Remove("D");
                    _startingLetters = _letter.ToArray();
                }
                else
                {
                    var freeNodes = _nodes.Where(n => n.OccupiedBlock == null).OrderBy(b => UnityEngine.Random.value).ToList();
                    foreach (var node in freeNodes.Take(1))
                    {
                        SpawnBlock(node, "D");
                        _letter.Remove("D");
                        _startingLetters = _letter.ToArray();
                        ChangeState(GameState.WaitingInput);
                    }
                }
                break;
            case "S":
                if (!_letter.Contains("G"))
                {
                    _letter.Add("G");
                }
                var countE = lettersOnBoard.Count(str => str == "E");
                if (countE % 2 == 0)
                {
                    _letter.Remove("E");
                    _startingLetters = _letter.ToArray();
                }
                else
                {
                    var freeNodes = _nodes.Where(n => n.OccupiedBlock == null).OrderBy(b => UnityEngine.Random.value).ToList();
                    foreach (var node in freeNodes.Take(1))
                    {
                        SpawnBlock(node, "E");
                        _letter.Remove("E");
                        _startingLetters = _letter.ToArray();
                        ChangeState(GameState.WaitingInput);
                    }
                }
                break;
            case "V":
                if (!_letter.Contains("H"))
                {
                    _letter.Add("H");
                }
                var countF = lettersOnBoard.Count(str => str == "F");
                if (countF % 2 == 0)
                {
                    _letter.Remove("F");
                    _startingLetters = _letter.ToArray();
                }
                else
                {
                    var freeNodes = _nodes.Where(n => n.OccupiedBlock == null).OrderBy(b => UnityEngine.Random.value).ToList();
                    foreach (var node in freeNodes.Take(1))
                    {
                        SpawnBlock(node, "F");
                        _letter.Remove("F");
                        _startingLetters = _letter.ToArray();
                        ChangeState(GameState.WaitingInput);
                    }
                }
                break;
            case "Y":
                if (!_letter.Contains("I"))
                {
                    _letter.Add("I");
                }
                var countG = lettersOnBoard.Count(str => str == "G");
                if (countG % 2 == 0)
                {
                    _letter.Remove("G");
                    _startingLetters = _letter.ToArray();
                }
                else
                {
                    var freeNodes = _nodes.Where(n => n.OccupiedBlock == null).OrderBy(b => UnityEngine.Random.value).ToList();
                    foreach (var node in freeNodes.Take(1))
                    {
                        SpawnBlock(node, "G");
                        _letter.Remove("G");
                        _startingLetters = _letter.ToArray();
                        ChangeState(GameState.WaitingInput);
                    }
                }
                break;
            default:
                break;
        }
    }

    //private void UpdateUpcomingLettersUI()
    //{
    //    _nextLetter.text = _lettersQueue.Peek();
    //}


     //private void CheckAndRefillLetters()
     //{
     //    if (_lettersQueue.Count == 0)
     //    {
     //        GenerateUpcomingLettersSequence(_initialLettersCount);
     //    }
     //    else if(_lettersQueue.Count == _refillThreshold)
     //    {
     //        GenerateUpcomingLettersSequence(_initialLettersCount - _refillThreshold);
     //    }
     //}


    void SpawnBlocks(int amount)
    {
        var freeNodes = _nodes.Where(n => n.OccupiedBlock == null).OrderBy(b => UnityEngine.Random.value).ToList();

        //CheckAndRefillLetters();
        //UpdateLetterSpawnChancess();

        switch (amount)
        {
            case 1:
                foreach (var node in freeNodes.Take(amount))
                {
                    string letter = _startingLetters[_random.Next(_startingLetters.Length)];
                    SpawnBlock(node, letter);
                    //UpdateUpcomingLettersUI();
                }
                break;
            case 2:
                foreach (var node in freeNodes.Take(amount))
                {
                    SpawnBlock(node, _startingLetters[_random.Next(_startingLetters.Length)]);
                    //UpdateUpcomingLettersUI();
                }
                break;
            default : break;
        }

        //var freeNodesAfterSpawning = _nodes.Where(n => n.OccupiedBlock == null).OrderBy(b => UnityEngine.Random.value).ToList();

        ChangeState(GameState.WaitingInput);
    }


    void SpawnBlock(Node node, string letter)
    {
        var block = Instantiate(_blockPrefab, node.Pos, Quaternion.identity);
        block.Init(GetBlockTypeByLetter(letter));
        block.SetBlock(node);
        _blocks.Add(block);
        SubscribeToBlockEvents(block);
        if (!lettersSpawned.Contains(block.Letter))
        {
            lettersSpawned.Add(block.Letter);
            lettersSpawned.Sort();
            if (updateOnLetters.Contains(block.Letter))
            {
                UpdateLetterSpawnChances(block.Letter);
            }
        }
        lettersOnBoard.Add(block.Letter);
        lettersOnBoard.Sort();
    }


    public void SubscribeToBlockEvents(Block block)
    {
        block.onDragStart.AddListener(HandleBlockPicked);
        block.onDrop.AddListener(HandleBlockDropped);
    }


    public void HandleBlockPicked(Block block)
    {
        //Handle block picked
    }


    public void HandleBlockDropped(Block block)
    {
        ChangeState(GameState.MovingBlockToTarget);

        if (_targetNodes.Contains(block.targetNode))
        {
            // Remove from _blocks and add to _targetBlocks
            if (!_targetBlocks.Contains(block))
            {
                _blocks.Remove(block);
                _targetBlocks.Add(block);
            }
            if(block.dragCount == 1)
            {
                ChangeState(GameState.SpawningBlocks);
            }
        }
    }


    //void Shift(Vector2 dir)
    //{
    //    ChangeState(GameState.Moving);

    //    var orderedBlocks = _blocks.OrderBy(b => b.Pos.x).ThenBy(b => b.Pos.y).ToList();
    //    if (dir == Vector2.right || dir == Vector2.up) orderedBlocks.Reverse();

    //    foreach (var block in orderedBlocks)
    //    {
    //        var previous = block.Node;
    //        //Debug.Log(previous.Pos);
    //        do
    //        {
    //            block.SetBlock(previous);
    //            //Debug.Log(previous.OccupiedBlock.Letter);
    //            var possibleNode = GetNodeAtPosition(previous.Pos + dir);
    //            if (possibleNode != null)
    //            {
    //                //Debug.Log(possibleNode.Pos);
    //                // We know a node is present
    //                // If it's possible to merge, set merge

    //                if (possibleNode.OccupiedBlock != null && possibleNode.OccupiedBlock.CanMerge(block.Letter))
    //                {
    //                    block.MergeBlock(possibleNode.OccupiedBlock);

    //                }
    //                // Otherwise, can we move to this spot?

    //                else if (possibleNode.OccupiedBlock == null) previous = possibleNode;

    //                // None hit? End do while loop
    //            }

    //        } while (previous != block.Node);
    //    }

    //    var sequence = DOTween.Sequence();

    //    foreach (var block in orderedBlocks)
    //    {
    //        var movePoint = block.MergingBlock != null ? block.MergingBlock.Node.Pos : block.Node.Pos;

    //        sequence.Insert(0, block.transform.DOMove(movePoint, _travelTime));
    //    }

    //    sequence.OnComplete(() =>
    //    {
    //        foreach (var block in orderedBlocks.Where(b => b.MergingBlock != null))
    //        {
    //            MergeBlocks(block.MergingBlock, block);
    //        }

    //        ChangeState(GameState.SpawningBlocks);
    //    });
    //}


    bool AreAllNodesOccupied()
    {
        return _nodes.All(node => node.OccupiedBlock != null);
    }


    bool CanAnyBlockMerge()
    {
        foreach (var block in _blocks)
        {
            var currentPos = block.Node.Pos;
            var directions = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

            foreach (var dir in directions)
            {
                var neighborNode = GetNodeAtPosition(currentPos + dir);
                if (neighborNode != null && neighborNode.OccupiedBlock != null &&
                    neighborNode.OccupiedBlock.CanMerge(block.Letter))
                {
                    return true;
                }
            }
        }
        return false;
    }


    void CheckForPossibleMoves()
    {
        if (!AreAllNodesOccupied())
        {
            return; // There are still free nodes, no need to check further
        }

        if (!CanAnyBlockMerge())
        {
            Debug.Log("No possible moves left, game over!");
            StartCoroutine(LoseWaiter());
        }
    }


    void Shift(Vector2 dir)
    {
        if (swipeSound != null && swipeSound.clip != null)
        {
            swipeSound.Play();
        }
        ChangeState(GameState.Shifting);

        moveOrMerge = 0;

        var orderedBlocks = OrderBlocksForShift(dir);

        foreach (var block in orderedBlocks)
        {
            MoveBlock(block, dir);
        }

        var sequence = DOTween.Sequence();

        foreach (var block in orderedBlocks)
        {
            var movePoint = block.MergingBlock != null ? block.MergingBlock.Node.Pos : block.Node.Pos;
            sequence.Insert(0, block.transform.DOMove(movePoint, _travelTime));
        }

        sequence.OnComplete(() =>
        {
            HandleMerges(orderedBlocks);
            if (merging == true)
            {
                ChangeState(GameState.SpawningBlocks);
            }
            else if (merging == false)
            {
                merging = true;
                CheckForPossibleMoves();
                ChangeState(GameState.WaitingInput);
            }

            CaptureGridState();
        });
    }


    private List<Block> OrderBlocksForShift(Vector2 direction)
    {
        var orderedBlocks = _blocks.OrderBy(b => b.Pos.x).ThenBy(b => b.Pos.y).ToList();
        if (direction == Vector2.right || direction == Vector2.up)
        {
            orderedBlocks.Reverse();
        }
        return orderedBlocks;
    }


    private void MoveBlock(Block block, Vector2 direction)
    {
        var next = block.Node;
        var originalNode = block.Node;
        do
        {
            block.SetBlock(next);
            var possibleNode = GetNodeAtPosition(next.Pos + direction);
            if (possibleNode != null)
            {
                if (possibleNode.OccupiedBlock != null && possibleNode.OccupiedBlock.CanMerge(block.Letter))
                {
                    block.MergeBlock(possibleNode.OccupiedBlock);
                }
                else if (possibleNode.OccupiedBlock == null)
                {
                    next = possibleNode;
                }
            }
        } while (next != block.Node);
        if (originalNode != block.Node) moveOrMerge++;
    }


    private void HandleMerges(List<Block> blocks)
    {
        foreach (var block in blocks.Where(b => b.MergingBlock != null))
        {
            MergeBlocks(block.MergingBlock, block);
        }
        if(moveOrMerge == 0) merging = false;
    }


    void MergeBlocks(Block baseBlock, Block mergingBlock)
    {
        score += GetScore(GetNextLetter(baseBlock.Letter));
        CheckHighScore();
        UpdateScoreUI(score);

        SpawnBlock(baseBlock.Node, GetNextLetter(baseBlock.Letter));

        RemoveBlock(baseBlock);
        RemoveBlock(mergingBlock);
        moveOrMerge++;
    }


    void CheckHighScore()
    {
        if(score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
            UpdateHighScoreUI();
        }
    }


    void RemoveBlock(Block block)
    {
        _blocks.Remove(block);
        lettersOnBoard.Remove(block.Letter);
        Destroy(block.gameObject);
    }


    private void CaptureGridState()
    {
        GridState currentState = new(_blocks);
        gridStates.Add(currentState);
        //Debug.Log("Captured Grid State: " + JsonUtility.ToJson(currentState));
    }
    

    private void UpdateScoreUI(int score)
    {
        _score.text = score.ToString();
    }


    private void UpdateHighScoreUI()
    {
        _highScore.text = PlayerPrefs.GetInt("HighScore", 0).ToString();

    }


    public void WinConditionCheck()
    {   
        string guessWord = null;
        List<Block> targetBlocks = _targetBlocks.OrderBy(b => b.transform.position.x).ToList();
        foreach (var block in targetBlocks)
        {
            guessWord += block.Letter;
        }
        //Debug.Log(guessWord);
        if(guessWord == _targetWord)
        {
            StartCoroutine(WinWaiter());
        }
        else
        {
            ChangeState(GameState.WaitingInput);
        }
    }


    Node GetNodeAtPosition(Vector2 pos)
    {
        return _nodes.FirstOrDefault(n => n.Pos == pos);
    }


    string GetNextLetter(string letter)
    {
        return letter switch
        {
            "A" => "B",
            "B" => "C",
            "C" => "D",
            "D" => "E",
            "E" => "F",
            "F" => "G",
            "G" => "H",
            "H" => "I",
            "I" => "J",
            "J" => "K",
            "K" => "L",
            "L" => "M",
            "M" => "N",
            "N" => "O",
            "O" => "P",
            "P" => "Q",
            "Q" => "R",
            "R" => "S",
            "S" => "T",
            "T" => "U",
            "U" => "V",
            "V" => "W",
            "W" => "X",
            "X" => "Y",
            "Y" => "Z",
            "Z" => "Z",
            _ => letter,
        };
    }


    int GetScore(string letter)
    {
        return letter switch
        {
            "B" => 1,
            "C" => 2,
            "D" => 3,
            "E" => 4,
            "F" => 5,
            "G" => 6,
            "H" => 7,
            "I" => 8,
            "J" => 9,
            "K" => 10,
            "L" => 11,
            "M" => 12,
            "N" => 13,
            "O" => 14,
            "P" => 15,
            "Q" => 16,
            "R" => 17,
            "S" => 18,
            "T" => 19,
            "U" => 20,
            "V" => 21,
            "W" => 22,
            "X" => 23,
            "Y" => 24,
            "Z" => 25,
            _ => 0,
        };
    }
}


[Serializable]
public struct BlockType
{
    public string Letter;
    public Color Color;
}


public enum GameState
{
    GenerateLevel,
    SpawningBlocks,
    WaitingInput,
    Shifting,
    MovingBlockToTarget,
    Win,
    Lose
}

[System.Serializable]
public class GridState
{
    public List<BlockState> blocks;

    public GridState(List<Block> blocks)
    {
        this.blocks = new List<BlockState>();
        foreach (var block in blocks)
        {
            this.blocks.Add(new BlockState(block.Letter, block.Node.Pos));
        }
    }
}

[System.Serializable]
public class BlockState
{
    public string letter;
    public Vector2 position;

    public BlockState(string letter, Vector2 position)
    {
        this.letter = letter;
        this.position = position;
    }
}
