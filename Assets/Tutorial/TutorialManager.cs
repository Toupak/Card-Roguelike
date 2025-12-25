using UnityEngine;

namespace Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        public static TutorialManager instance;

        private void Awake()
        {
            instance = this;
        }

        public void StartTutorial()
        {
            Debug.Log("Start Tutorial");
        }
    }
}
