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

    public IEnumerator SpineHit(Transform hit)
    {
        yield return new WaitForSeconds(Random.Range(0f, 0.02f));

        transform.SetParent(hit);

        rig.velocity = Vector2.zero;
        rig.isKinematic = true;

        Destroy(gameObject, 1.5f);
    }
}
