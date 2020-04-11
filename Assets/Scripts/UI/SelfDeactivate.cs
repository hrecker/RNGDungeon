using UnityEngine;

namespace UI
{
    public class SelfDeactivate : MonoBehaviour
    {
        public float lifetime;
        private float timer;

        void Update()
        {
            timer += Time.deltaTime;
            if (timer >= lifetime)
            {
                gameObject.SetActive(false);
                ResetTimer();
            }
        }

        public void ResetTimer()
        {
            timer = 0.0f;
        }
    }
}
