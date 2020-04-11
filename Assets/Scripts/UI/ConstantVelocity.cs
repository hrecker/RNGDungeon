using UnityEngine;

namespace UI
{
    public class ConstantVelocity : MonoBehaviour
    {
        public Vector2 velocity;

        void Update()
        {
            transform.Translate(velocity * Time.deltaTime);
        }
    }
}
