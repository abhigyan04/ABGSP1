using System.Collections.Generic;
using UnityEngine;

public class NewSceneSizeTest : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private SpriteRenderer _boardPrefab;
    [SerializeField] private Node _nodePrefab;
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private List<Node> _nodes = new();


    void Awake()
    {
        var center = new Vector2((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f);

        //var board = Instantiate(_boardPrefab, new Vector2(center.x, center.y), Quaternion.identity);
        //board.size = new Vector2((float)_width, (float)_height);
        
        //Camera.main.transform.position = new Vector3(center.x, center.y, -10);

        for (int x = 0; x < _width; x ++)
        {
            for (int y = 0; y < _height; y ++)
            {
                //var node = Instantiate(_nodePrefab, new Vector2(x, y), Quaternion.identity);
                //_nodes.Add(node);
            }
        }
    }


    void Update()
    {
        
    }
}

//private tk2dCamera thisCamera;
//private tk2dCameraResolutionOverride currOverride;
//private int previewWidth;
//private int previewHeight;
//private int ppm;
//private int nativeScreenWidth;
//private int nativeScreenHeight;
//private float scaledWidthToTarget;
//private float cameraDeltaScreenPixels;


//public static float scaledWidth { get; protected set; }
//public static float scaledWidthRatio { get; protected set; }
//public static float cameraDeltaSceneUnits { get; protected set; }

//void Start()
//{
//    thisCamera = gameObject.GetComponent<tk2dCamera>();
//    previewWidth = (int)thisCamera.TargetResolution.x;
//    previewHeight = (int)thisCamera.TargetResolution.y;

//    ppm = (int)thisCamera.CameraSettings.orthographicPixelsPerMeter;

//    nativeScreenWidth = thisCamera.nativeResolutionWidth;
//    nativeScreenHeight = thisCamera.nativeResolutionHeight;

//    // Used by scene elements for positioning themselves
//    scaledWidth = (previewWidth / (float)(previewHeight)) * nativeScreenHeight;
//    scaledWidthRatio = scaledWidth / nativeScreenWidth;

//    scaledWidthToTarget = (nativeScreenWidth / (float)nativeScreenHeight) * previewHeight;

//    cameraDeltaScreenPixels = (scaledWidthToTarget - previewWidth) / 2;
//    cameraDeltaSceneUnits = ((scaledWidth - nativeScreenWidth) / 2) / ppm;

//    currOverride = thisCamera.CurrentResolutionOverride;
//    currOverride.fitMode = tk2dCameraResolutionOverride.FitMode.Constant;
//    currOverride.offsetPixels = new Vector2(cameraDeltaScreenPixels, 0f);
