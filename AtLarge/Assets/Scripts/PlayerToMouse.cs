using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToMouse : MonoBehaviour
{
    public Vector2 screenPosition;
    public Vector2 worldPosition;

    void Update() {
        screenPosition = Input.mousePosition;

        worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        transform.position = worldPosition;
    }
}
