using UnityEngine;

public class DestroySelfEvent : MonoBehaviour
{
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
