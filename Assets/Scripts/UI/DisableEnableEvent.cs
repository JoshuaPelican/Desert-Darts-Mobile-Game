using UnityEngine;

public class DisableEnableEvent : MonoBehaviour
{
    public void DisableEnable(GameObject enable)
    {
        Debug.Log("Disabled");
        enable.SetActive(true);
        gameObject.SetActive(false);
    }
}
