using UnityEngine;

namespace Cocktailor
{
    public class DeactiveSelf : MonoBehaviour
    {
        public void DeactivateSelf()
        {
            gameObject.SetActive(false);
        }
    }
}