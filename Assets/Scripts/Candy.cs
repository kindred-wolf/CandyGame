using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    // Fixed position inside the box
    public Transform spawnPosition;
    public Transform fixedPosition;
    public bool isFixed = false;

    const float MAGNET_DISTANCE = 50f;

    // Check if draggable object is close enough to be magnetized
    // If it is -> magnetize it to its fixed position, change counter and play sound
    public bool CheckFix()
    {
        Vector3 screenFixedPosition = Camera.main.WorldToScreenPoint(fixedPosition.position);
        Vector3 screenCandyPosition = Camera.main.WorldToScreenPoint(transform.position);

        float distance = new Vector2(screenFixedPosition.x - screenCandyPosition.x, screenFixedPosition.y - screenCandyPosition.y).magnitude;

        if (distance < MAGNET_DISTANCE)
        {
            isFixed = true;
            transform.position = fixedPosition.position;
            FindObjectOfType<Logic_15>().fixCounter++;
            return true;
        }
        
        return false;
    }
}
