using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float minZoom = 5f;
    public float maxZoom = 17f;
    public float zoomSpeed = 0.75f;
    public float moveSpeed = 0.5f;
    public float padding = 2f;

    private Camera _cam;
    private Vector2 _worldMinBounds, _worldMaxBounds;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
        MatchManager.OnMatchStarting.AddListener(OnMatchStarting);
    }

    private void LateUpdate()
    {
        MoveCamera();
        ZoomCamera();
    }

    private void OnMatchStarting()
    {
        GetBounds();
    }

    private void GetBounds()
    {
        _worldMinBounds = Vector2.negativeInfinity;
        _worldMaxBounds = Vector2.positiveInfinity;

        List<GameObject> limits = GameObject.FindGameObjectsWithTag("StageLimits").ToList();
        if (limits.Count > 0)
        {
            _worldMinBounds.x = limits.Min(x => x.transform.position.x);
            _worldMinBounds.y = limits.Min(x => x.transform.position.y);

            _worldMaxBounds.x = limits.Max(x => x.transform.position.x);
            _worldMaxBounds.y = limits.Max(x => x.transform.position.y);
        }

        maxZoom = Mathf.Min((_worldMaxBounds.x - _worldMinBounds.x) / _cam.aspect, _worldMaxBounds.y - _worldMinBounds.y) /
                  2f;
        _cam.orthographicSize = maxZoom;
    }

    private void MoveCamera()
    {
        List<Vector2> players = GetAlivePlayersPositions();
        if (players.Count == 0)
            return;

        Vector2 averagePosition = Vector2.zero;
        foreach (Vector2 position in players) averagePosition += position;
        averagePosition /= players.Count;

        float camHalfHeight = _cam.orthographicSize;
        float camHalfWidth = camHalfHeight * _cam.aspect;

        Vector3 finalPosition =
            Vector3.Lerp(transform.position, averagePosition, 1 - Mathf.Exp(-Time.deltaTime * moveSpeed));

        finalPosition.x =
            Mathf.Clamp(finalPosition.x, _worldMinBounds.x + camHalfWidth, _worldMaxBounds.x - camHalfWidth);
        finalPosition.y = Mathf.Clamp(finalPosition.y, _worldMinBounds.y + camHalfHeight,
            _worldMaxBounds.y - camHalfHeight);

        transform.position = new Vector3(finalPosition.x, finalPosition.y, transform.position.z);
    }

    private void ZoomCamera()
    {
        List<Vector2> players = GetAlivePlayersPositions();
        if (players.Count == 0)
            return;

        float bounding = BoundingZoom(players, padding, _cam.aspect);
        float desiredZoom = Mathf.Clamp(bounding, 0f, maxZoom);

        Debug.Log($"Bounding: {bounding}, desired: {desiredZoom}, min: {minZoom}, max: {maxZoom}");
        _cam.orthographicSize =
            Mathf.Lerp(_cam.orthographicSize, desiredZoom, 1 - Mathf.Exp(-Time.deltaTime * zoomSpeed));
    }

    private List<Vector2> GetAlivePlayersPositions()
    {
        return MatchManager.GetActivePlayers()
            .Where(player => player.IsInGameObjectAlive()).ToList()
            .ConvertAll(player => (Vector2)player.InGameObject.transform.position);
    }

    private static float BoundingZoom(List<Vector2> points, float padding, float aspect)
    {
        float maxX = points.Max(p => p.x);
        float maxY = points.Max(p => p.y);
        float minX = points.Min(p => p.x);
        float minY = points.Min(p => p.y);

        return Zoom(maxX, maxY, minX, minY, padding, aspect);
    }

    private static float Zoom(float maxX, float maxY, float minX, float minY, float padding, float aspect)
    {
        float halfWidth = (maxX - minX) / 2f + padding;
        float halfHeight = (maxY - minY) / 2f + padding;

        return Mathf.Max(halfWidth / aspect, halfHeight);
    }
}