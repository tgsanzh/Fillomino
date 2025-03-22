using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAnimation : MonoBehaviour
{
    [SerializeField] private GameObject[] settingsIcons;
    public void settingsOn()
    {
        foreach (var item in settingsIcons)
        {
            item.GetComponent<Animation>().Play();
        }
    }
}
