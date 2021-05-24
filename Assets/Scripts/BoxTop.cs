using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTop : MonoBehaviour
{
    Logic_15 logic;

    public void DestroyCandies()
    {
        logic = GameObject.FindGameObjectWithTag("GameController").GetComponent<Logic_15>();
        for (int i = 0; i < logic.candies.Length; i++)
            Destroy(logic.candies[i]);
    }
}
