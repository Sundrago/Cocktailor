using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactiveSelf : MonoBehaviour
{
    public void DeactivateSelf()
    {
        gameObject.SetActive(false);
    }
}
