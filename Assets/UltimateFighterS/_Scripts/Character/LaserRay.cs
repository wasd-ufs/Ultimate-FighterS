using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class LaserRay : MonoBehaviour
{
    [FormerlySerializedAs("shootPoint")] public GameObject ShootPoint;
    private readonly float _maximumSize = 100f;
    private readonly float _minimumSize = 0.1f;

    private void FixedUpdate()
    {
        transform.position = ShootPoint.transform.position +
                             new Vector3(DetermineSize() / 2, 0, 0) * Mathf.Sign(transform.lossyScale.x);
        transform.localScale = new Vector3(DetermineSize(), 0.1f, 1);
    }

    private float DetermineSize()
    {
        RaycastHit2D[] hits;
        RaycastHit2D hit;
        hits = Physics2D.RaycastAll(ShootPoint.transform.position, new Vector2(ShootPoint.transform.lossyScale.x, 0),
            _maximumSize);
        try
        {
            hit = hits.First(hitTemp => !hitTemp.transform.CompareTag("Laser") && !hitTemp.collider.isTrigger &&
                                        !hitTemp.collider.gameObject.CompareTag("Hitbox") &&
                                        !hitTemp.collider.gameObject.CompareTag("Hurtbox") &&
                                        !hitTemp.collider.gameObject.CompareTag("Player"));
        }
        catch
        {
            return _maximumSize;
        }

        float size = Mathf.Max(hit.distance, _minimumSize);
        return size;
    }
}