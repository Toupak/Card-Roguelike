using UnityEngine;

namespace Character_Selection.Character
{
    public class CharacterSingleton : MonoBehaviour
    {
        public Animator animator;
        
        public static CharacterSingleton instance;

        private CharacterMovement characterMovement;
        
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

            characterMovement = GetComponent<CharacterMovement>();
        }

        public void LockPlayer()
        {
            isLocked = true;
            characterMovement.SetLockState(isLocked);
        }

        public void UnlockPlayer()
        {
            isLocked = false;
            characterMovement.SetLockState(isLocked);
        }
    }
}
