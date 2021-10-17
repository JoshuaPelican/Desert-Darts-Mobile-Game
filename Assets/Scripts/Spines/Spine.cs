using UnityEngine;

public class Spine : MonoBehaviour
{
    [Header("Spine Settings")]
    public float pointMultiplier = 1;

    Rigidbody2D rig;

    AudioSource source;
    [Header("Audio Settings")]
    [SerializeField] AudioClip[] clips;

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        source = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        rig.AddForce(-transform.up * Physics2D.gravity.magnitude, ForceMode2D.Force);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("OutOfBounds"))
        {
            SpineSpawnManager.instance.AdjustIntensity(MathUtility.Operation.Subtract, 0.334f);
            Disable();
        }
    }

    public void PlayHitAudio(float bonusPitch)
    {
        AudioUtility.RandomizeSourceAndPlay(clips, source, 1f, 1 + bonusPitch, 0.05f);
    }

    public void Disable()
    {
        rig.velocity = Vector2.zero;
        rig.isKinematic = true;
        Destroy(gameObject, 1.5f);
    }
}
