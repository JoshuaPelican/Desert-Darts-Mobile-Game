// https://stackoverflow.com/questions/58941259/how-to-give-the-cameras-edge-collision-in-unity
// adds EdgeCollider2D colliders to screen edges
// only works with orthographic camera

using UnityEngine;
using System.Collections;

namespace UnityLibrary
{
    public class CameraEdgeCollision : MonoBehaviour
    {
        void Start()
        {
            AddCollider();
        }

        void AddCollider()
        {
            if (Camera.main == null) { Debug.LogError("Camera.main not found, failed to create edge colliders"); return; }

            var cam = Camera.main;
            if (!cam.orthographic) { Debug.LogError("Camera.main is not Orthographic, failed to create edge colliders"); return; }

            Vector2 offset = cam.orthographicSize * cam.rect.position;
            offset.y *= 2.334f;

            var bottomLeft =  (Vector2)cam.ScreenToWorldPoint(new Vector2(0,                    0));
            var topLeft =     (Vector2)cam.ScreenToWorldPoint(new Vector2(0,                    cam.pixelRect.height));
            var topRight =    (Vector2)cam.ScreenToWorldPoint(new Vector2(cam.pixelRect.width,  cam.pixelRect.height));
            var bottomRight = (Vector2)cam.ScreenToWorldPoint(new Vector2(cam.pixelRect.width,  0));

            // add or use existing EdgeCollider2D
            var edge = GetComponent<EdgeCollider2D>() == null ? gameObject.AddComponent<EdgeCollider2D>() : GetComponent<EdgeCollider2D>();

            var edgePoints = new[] { bottomLeft, topLeft, topRight, bottomRight, bottomLeft };
            edge.points = edgePoints;
            edge.offset = offset;
        }
    }
}