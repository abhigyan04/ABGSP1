using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SizeManagement : MonoBehaviour
{

    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private Node _nodePrefab;
    [SerializeField] private Button startPrefab;
    [SerializeField] private List<Node> _nodes = new();


    void Start()
    {
        var center = new Vector2((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f);

        float screenAspect = (float)Screen.width / (float)Screen.height;
        float targetAspect = 9f / 16f;
        float camScale = targetAspect / screenAspect;

        Camera.main.transform.position = new Vector3(center.x, center.y, -10);
        Camera.main.orthographicSize = (10f / 2f) * camScale;

        //float scale = (10 * screenAspect * 0.9f) / 5f;

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var node = Instantiate(_nodePrefab, new Vector2(x, y), Quaternion.identity);

                _nodes.Add(node);
            }
        }
        //InstantiateButtons();
    }


    void InstantiateButtons()
    {
        var Start = Instantiate(startPrefab, _nodes[12].Pos, Quaternion.identity);
        Start.transform.SetParent(_nodes[12].transform, true);
    }
}
