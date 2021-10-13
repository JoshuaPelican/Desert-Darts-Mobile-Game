using UnityEngine;

public class TargetSection : MonoBehaviour
{
    [SerializeField] float pointValue = 1;
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
        if (collision.CompareTag("Spine"))
        {
            Spine spine = collision.gameObject.GetComponent<Spine>();
            collision.enabled = false;

            //Do something with points
            GameManager.instance.ApplyPoints(pointValue * spine.pointMultiplier, GameManager.Operation.Add, true, transform.position, sectionColor);

            //Make a floating text with the points;

            //Do something with the spine
            spine.SpineHit(transform);
        }
    }
}
