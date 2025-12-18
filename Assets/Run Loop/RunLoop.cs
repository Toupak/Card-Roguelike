using System.Collections;
using Battles.Data;
using BoomLib.Tools;
using CombatLoop.Battles;
using Data;
using Run_Loop.Rewards;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Run_Loop
{
    public class RunLoop : MonoBehaviour
    {
        [SerializeField] private Image blackScreen;
        
        [Space]
        [SerializeField] private SceneField hubScene;
        [SerializeField] private SceneField overWorldScene;
        [SerializeField] private SceneField rewardScene;
        [SerializeField] private SceneField combatScene;

        [Space] 
        [SerializeField] private CardDatabase cardDatabase;

        [Space] 
        [SerializeField] private BattlesDataHolder battlesDataHolder;

        public static RunLoop instance;
        
        public CardDatabase dataBase => cardDatabase;
        public BattleData currentBattleData => battlesDataHolder.battles[currentBattleIndex];

        private int currentBattleIndex;
        private bool isInRun;
        
        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            
            if (currentSceneName != hubScene)
            {
                Debug.Log($"Game was not started in scene : {hubScene.SceneName} => RunLoop will self destroy.");
                Destroy(gameObject);
                return;
            }
        }

        public void StartRun()
        {
            if (isInRun)
                return;

            isInRun = true;
            StartCoroutine(StartNewRun());
        }

        public void StartBattle()
        {
            if (!isInRun)
                return;

            StartCoroutine(StartNewBattle());
        }

        private IEnumerator StartNewBattle()
        {
            yield return LoadScene(combatScene);
            yield return new WaitUntil(IsCombatOver);
            bool isPlayerAlive = CheckCombatResult();
            StoreCardsHealth();

            if (!IsRunOver() && isPlayerAlive)
            {
                yield return LoadScene(rewardScene);
                yield return new WaitUntil(IsRewardSelected);
                
                currentBattleIndex += 1;
                if (currentBattleIndex >= battlesDataHolder.battles.Count)
                    currentBattleIndex = 0;
                
                yield return LoadScene(overWorldScene);
            }
            
            if (!isPlayerAlive)
                yield return LoadScene(hubScene);
        }

        private IEnumerator StartNewRun()
        {
            currentBattleIndex = 0;
            PlayerDeck.instance.ClearDeck();
            
            yield return LoadScene(overWorldScene);
        }

        private bool IsRewardSelected()
        {
            return RewardLoop.instance != null && RewardLoop.instance.isRewardScreenOver;
        }

        private bool IsCombatOver()
        {
            return CombatLoop.CombatLoop.instance.IsMatchOver();
        }

        private bool CheckCombatResult()
        {
            return CombatLoop.CombatLoop.instance.HasPlayerWon();
        }
        
        private void StoreCardsHealth()
        {
            CombatLoop.CombatLoop.instance.StoreCardsHealth();
        }
        
        private bool IsRunOver()
        {
            return false;
        }

        private IEnumerator LoadScene(SceneField sceneField)
        {
            Debug.Log($"Load Scene : {sceneField.SceneName}");
            yield return Fader.Fade(blackScreen, 0.3f, true);

            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneField.SceneName);
            yield return new WaitUntil(() => operation.isDone);
            
            yield return Fader.Fade(blackScreen, 0.3f, false);
        }
    }
}
