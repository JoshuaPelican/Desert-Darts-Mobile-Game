using System.Collections;
using UnityEngine;

public class Spine : MonoBehaviour
{
    [Header("Spine Settings")]
    public float pointMultiplier = 1;

    Rigidbody2D rig;

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    public void SpineHit(Transform hit)
    {
        rig.velocity = Vector2.zero;
        rig.isKinematic = true;
        transform.SetParent(hit);

        Destroy(gameObject, 1.5f);
    }
}
