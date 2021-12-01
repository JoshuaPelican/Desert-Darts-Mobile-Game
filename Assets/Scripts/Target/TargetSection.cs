using UnityEngine;

public class TargetSection : MonoBehaviour
{
    [Header("Target Section Settings")]
    [SerializeField] float pointValue = 1;
    [SerializeField] float intensityBoost = 0.01f;
    [SerializeField] float multiplierBoost = 0f;
    [Space()]
    [SerializeField] Transform socket;
    [SerializeField] [Range(-1, 1)] int direction = 1;
    [Space()]
    [SerializeField] AudioClip sectionClip;

    Color sectionColor;
    AudioSource source;

    private void Start()
    {
        sectionColor = GetComponent<SpriteRenderer>().color;
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(socket != null)
        {
            Vector2 pos = transform.position;
            pos.x = (transform.localScale.x * direction / 2) + socket.position.x;
            transform.position = pos;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Spine target trigger stuff
        if (collision.CompareTag("Spine"))
        {
            Spine spine = collision.gameObject.GetComponent<Spine>();
            collision.enabled = false;

            //Make a noise or something

            //Do something with the points
            //Make a floating text with the points;
            GameManager.instance.ApplyPoints(pointValue * spine.pointMultiplier, MathUtility.Operation.Add, true, transform.position, sectionColor);
            GameManager.instance.ChangeMultiplierProgress(multiplierBoost);

            //Increase spawn intensity
            SpineSpawnManager.instance.AdjustIntensity(MathUtility.Operation.Add, intensityBoost);

            //Do something with the spine
            spine.transform.SetParent(transform);
            if(sectionClip)
                AudioUtility.RandomizeSourceAndPlay(sectionClip, source, 0.35f, 3f, 0.05f);
            spine.PlayHitAudio(pointValue / 20f);
            spine.Disable();
        }
    }
}
