using UnityEngine;

public class Spine : MonoBehaviour
{
    [Header("Spine Settings")]
    public float pointMultiplier = 1;

    Rigidbody2D rig;
    Collider2D col;
    Animator anim;

    AudioSource source;
    [Header("Audio Settings")]
    [SerializeField] AudioClip[] clips;

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
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
            GameManager.instance.MissedSpine();
            Disable();
        }
    }

    public void PlayHitAudio(float bonusPitch)
    {
        AudioUtility.RandomizeSourceAndPlay(clips, source, 0.95f, 1 + bonusPitch, 0.05f);
    }

    public void ClearSpine()
    {
        rig.velocity = Vector2.zero;
        rig.gravityScale = 1.5f;
        col.enabled = false;
        anim.SetTrigger("Fade Short");
        rig.AddForce(new Vector2((Random.value * 2) - 1, Random.value) * 2, ForceMode2D.Impulse);
        rig.AddTorque(1000);
    }

    public void Disable()
    {
        rig.velocity = Vector2.zero;
        rig.isKinematic = true;
        col.enabled = false;
        anim.SetTrigger("Fade Long");
    }
}
