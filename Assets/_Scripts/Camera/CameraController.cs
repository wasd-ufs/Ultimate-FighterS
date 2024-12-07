using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float minZoom = 5f;
    public float maxZoom = 17f;
    public float zoomSpeed = 5f;
    public float moveSpeed = 3f;
    public float padding = 2f;

    private Camera cam;
    private Vector2 worldMinBounds, worldMaxBounds;
    
    void Awake()
    {
        cam = GetComponent<Camera>();
        MatchManager.OnMatchStarting.AddListener(OnMatchStarting);
    }

    void OnMatchStarting()
    {
        GetBounds();
    }

    void GetBounds()
    {
        worldMinBounds = Vector2.negativeInfinity;
        worldMaxBounds = Vector2.positiveInfinity;
        
        var limits = GameObject.FindGameObjectsWithTag("StageLimits").ToList();
        if (limits.Count > 0)
        {
            worldMinBounds.x = limits.Min(x => x.transform.position.x);
            worldMinBounds.y = limits.Min(x => x.transform.position.y);

            worldMaxBounds.x = limits.Max(x => x.transform.position.x);
            worldMaxBounds.y = limits.Max(x => x.transform.position.y);
        }
        
        maxZoom = Mathf.Min((worldMaxBounds.x - worldMinBounds.x) / cam.aspect, worldMaxBounds.y - worldMinBounds.y) / 2f;
        Debug.Log(maxZoom);
    }

    void LateUpdate()
    {
        MoveCamera();
        ZoomCamera();
    }

    void MoveCamera()
    {
        var players = GetAlivePlayersPositions();
        if (players.Count == 0)
            return;
        
        Vector2 averagePosition = Vector2.zero;
        foreach (Vector2 position in players)
        {
            averagePosition += position;
        }
        
        averagePosition /= players.Count;
        Vector3 finalPosition = Vector3.Lerp(transform.position, averagePosition, Time.deltaTime * moveSpeed);
                
        float camHalfHeight = cam.orthographicSize;
        float camHalfWidth = camHalfHeight * cam.aspect;

        finalPosition.x = Mathf.Clamp(finalPosition.x, worldMinBounds.x + camHalfWidth, worldMaxBounds.x - camHalfWidth);
        finalPosition.y = Mathf.Clamp(finalPosition.y, worldMinBounds.y + camHalfHeight, worldMaxBounds.y - camHalfHeight);

        transform.position = new Vector3(finalPosition.x, finalPosition.y, transform.position.z);
    }

    void ZoomCamera()
    {
        var players = GetAlivePlayersPositions();
        if (players.Count == 0)
            return;
        
        var desiredZoom = BoundingZoom(players, padding, cam.aspect);
        
        minZoom = Mathf.Min(padding / cam.orthographicSize / cam.orthographicSize, maxZoom);
        
        cam.orthographicSize = Mathf.Clamp(Mathf.Lerp(cam.orthographicSize, desiredZoom, Time.deltaTime * zoomSpeed), 0f, maxZoom);
    }

    private List<Vector2> GetAlivePlayersPositions() => MatchManager.GetActivePlayers()
        .Where(player => player.IsInGameObjectAlive()).ToList()
        .ConvertAll(player => (Vector2)player.InGameObject.transform.position);
    
    private static float BoundingZoom(List<Vector2> points, float padding, float aspect)
    {
        float maxX = points.Max(p => p.x) + padding;
        float maxY = points.Max(p => p.y) + padding;
        float minX = points.Min(p => p.x) - padding;
        float minY = points.Min(p => p.y) - padding;

        return Zoom(maxX, maxY, minX, minY, padding, aspect);
    }
    
    private static float Zoom(float maxX, float maxY, float minX, float minY, float padding, float aspect)
    {
        float width = maxX - minX + padding;
        float height = maxY - minY + padding;

        return Mathf.Max(width / aspect, height) / 2f;
    }
}
