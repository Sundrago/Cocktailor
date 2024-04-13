using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParent : MonoBehaviour
{
    public GameObject prnt;
    public void DestroyP()
    {
        Destroy(prnt);
    }
}
