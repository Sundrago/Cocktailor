using UnityEngine;

namespace Cocktailor
{
    public class DestroyParent : MonoBehaviour
    {
        public GameObject prnt;

        public void DestroyP()
        {
            Destroy(prnt);
        }
    }
}