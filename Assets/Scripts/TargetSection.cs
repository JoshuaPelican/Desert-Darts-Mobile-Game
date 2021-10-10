using UnityEngine;

public class TargetSection : MonoBehaviour
{
    [SerializeField] float pointValue = 1;
    [SerializeField] Transform socket;
    [SerializeField] [Range(-1, 1)] int direction = 1;

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

            //Do something with points
            GameManager.instance.AddPoints(pointValue * spine.pointMultiplier);

            //Do something with the spine
            StartCoroutine(spine.SpineHit(transform));
        }
    }
}
