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

    private void FixedUpdate()
    {
        rig.AddForce(-transform.up * Physics2D.gravity.magnitude, ForceMode2D.Force);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("OutOfBounds"))
        {
            SpineSpawnManager.instance.AdjustIntensity(MathUtility.Operation.Subtract, 0.075f);
            Disable();
        }
    }

    public void Disable()
    {
        rig.velocity = Vector2.zero;
        rig.isKinematic = true;
        Destroy(gameObject, 1.5f);
    }
}
