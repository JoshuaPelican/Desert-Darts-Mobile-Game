using System.Collections;
using UnityEngine;

public class TiltMovement : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] float tiltThreshold = 0.1f;

    [Header("Movement Settings")]
    [SerializeField] float movementSpeed = 1f;
    [SerializeField] float smoothTime = 0.5f;

    bool isLocked = true;

    Rigidbody2D rig;
    Vector2 velocity;

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();

        StartCoroutine(LockMovement(0.25f));
    }

    private void Update()
    {
        if (!isLocked)
        {
            rig.velocity = Vector2.SmoothDamp(rig.velocity, CalculateTilt(), ref velocity, smoothTime);
        }
    }

    private Vector2 CalculateTilt()
    {
        Vector3 acceleration = Input.acceleration; //Getting the acceleration from the phone's accelerometer

        //if (acceleration.sqrMagnitude > 1) //If the 
        //    acceleration.Normalize();

        Vector3.ClampMagnitude(acceleration, 1);

        float xAcceleration = acceleration.x;

        if (xAcceleration < tiltThreshold && xAcceleration > -tiltThreshold)
            xAcceleration = 0;

        xAcceleration *= Mathf.Abs(xAcceleration);

        Vector2 movement = new Vector2(xAcceleration, 0.0f) * movementSpeed * Time.deltaTime * 100;
        return movement;
    }

    private IEnumerator LockMovement(float duration)
    {
        isLocked = true;

        yield return new WaitForSeconds(duration);

        isLocked = false;
    }
}
