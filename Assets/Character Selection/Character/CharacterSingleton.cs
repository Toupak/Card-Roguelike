using UnityEngine;

namespace Overworld.Character
{
    public class CharacterSingleton : MonoBehaviour
    {
        public Animator animator;
        
        public static CharacterSingleton instance;

        private bool isLocked;
        public bool IsLocked => isLocked;
        
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
                instance = this;
        }

        public void LockPlayer()
        {
            isLocked = true;
        }

        public void UnlockPlayer()
        {
            isLocked = false;
        }
    }
}
