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
        //Assign all the references
        rig = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        rig.AddForce(-transform.up * Physics2D.gravity.magnitude, ForceMode2D.Force); //Constantly move downwards using gravity instead of normal rigidbody2d calculations
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.CompareTag("OutOfBounds")) //If the spine becomes out of bounds, notify the game a spine has been missed and disable this spine
        {
            GameManager.instance.MissedSpine();
            Disable();
        }
    }

    public void PlayHitAudio(float bonusPitch) //Event to play the spine's impact audio; Called by the 
    {
        AudioUtility.RandomizeSourceAndPlay(clips, source, 0.95f, 1 + bonusPitch, 0.05f);
    }

    public void Clear() //Make the spine useless and spin it out of existence over a short time
    {
        rig.velocity = Vector2.zero;
        rig.gravityScale = 1.5f;
        col.enabled = false;
        anim.SetTrigger("Fade Short");
        rig.AddForce(new Vector2((Random.value * 2) - 1, Random.value) * 2, ForceMode2D.Impulse);
        rig.AddTorque(1000);
    }

    public void Disable() //Prevent the spine from moving and fade it out over a long time
    {
        rig.velocity = Vector2.zero;
        rig.isKinematic = true;
        col.enabled = false;
        anim.SetTrigger("Fade Long");
    }
}
