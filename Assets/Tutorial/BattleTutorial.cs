using PrimeTween;
using Tutorial.Post_it;
using UnityEngine;

namespace Tutorial
{
    public class BattleTutorial : MonoBehaviour
    {
        [SerializeField] private PostIt boardPostIt;
        [SerializeField] private PostIt cardSpellPostIt;
        [SerializeField] private PostIt enemyPostIt;

        public static BattleTutorial instance;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            boardPostIt.gameObject.SetActive(false);
            cardSpellPostIt.gameObject.SetActive(false);
            enemyPostIt.gameObject.SetActive(false);
        }

        public void ActivateTutorialForThisBattle()
        {
            CombatLoop.CombatLoop.OnPlayerDrawHand.AddListener(() => ActivatePostIt(boardPostIt));
            CombatLoop.CombatLoop.OnPlayerPlayAtLeastOneCard.AddListener(() => ActivatePostIt(enemyPostIt));
            CombatLoop.CombatLoop.OnPlayerPlayStartFirstTurn.AddListener(() => ActivatePostIt(cardSpellPostIt));
        }

        private void ActivatePostIt(PostIt postIt)
        {
            postIt.gameObject.SetActive(true);
            Tween.PunchScale(postIt.transform, Vector3.one, 0.1f);
        }
    }
}
