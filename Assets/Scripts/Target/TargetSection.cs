using UnityEngine;

public class TargetSection : MonoBehaviour
{
    [SerializeField] float pointValue = 1;
    [SerializeField] float intensityBoost = 0.01f;
    [SerializeField] Transform socket;
    [SerializeField] [Range(-1, 1)] int direction = 1;

    Color sectionColor;

    private void Start()
    {
        sectionColor = GetComponent<SpriteRenderer>().color;
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

            //Do something with the points
            //Make a floating text with the points;
            GameManager.instance.ApplyPoints(pointValue * spine.pointMultiplier, MathUtility.Operation.Add, true, transform.position, sectionColor);

            //Increase spawn intensity
            SpineSpawnManager.instance.AdjustIntensity(MathUtility.Operation.Add, intensityBoost);

            //Do something with the spine
            spine.transform.SetParent(transform);
            spine.Disable();
        }
    }
}
