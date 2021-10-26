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

        StartCoroutine(LockMovement(0.25f)); //Start the target in a locked state to prevent major jerky snapping when loading the scene
    }

    private void Update()
    {
        if (!isLocked) //If the object is not locked, smoothly change the velocity of the rigidbody2d to match the calculated movement of the tilt over time
        {
            rig.velocity = Vector2.SmoothDamp(rig.velocity, CalculateTilt(), ref velocity, smoothTime);
        }
    }

    private Vector2 CalculateTilt() //Determines object movement using the devices accelerometer
    {
        Vector3 acceleration = Input.acceleration; //Getting the acceleration from the phone's accelerometer

        Vector3.ClampMagnitude(acceleration, 1); //Clamp the acceleration to "normalize" it

        float xAcceleration = acceleration.x; //We only need the x acceleration to move the object side to side on the screen

        if (xAcceleration < tiltThreshold && xAcceleration > -tiltThreshold) //Keep the acceleration limited to only past the minimum accelerometer threshold
            xAcceleration = 0;

        Vector2 movement = new Vector2(xAcceleration, 0.0f) * movementSpeed * Time.deltaTime * 100; //Calculate the movement of the object using a speed and normalizing it across time;
                                                                                                    //100 is used to keep movementSpeed closer to whole numbers
        return movement;
    }

    private IEnumerator LockMovement(float duration) //Restrict the movement of the object for a set amount of seconds
    {
        isLocked = true;

        yield return new WaitForSeconds(duration);

        isLocked = false;
    }
}
