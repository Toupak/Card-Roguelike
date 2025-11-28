using System.Collections;
using BoomLib.Tools;
using Data;
using Run_Loop.Rewards;
using Run_Loop.Run_Parameters;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Run_Loop
{
    public class RunLoop : MonoBehaviour
    {
        [SerializeField] private Image blackScreen;
        
        [Space]
        [SerializeField] private SceneField parameterScene;
        [SerializeField] private SceneField rewardScene;
        [SerializeField] private SceneField combatScene;

        [Space] 
        [SerializeField] private CardDatabase cardDatabase;
        
        public static RunLoop instance;
        
        public RunParameterData currentRunParameterData { get; private set; }
        public CardDatabase dataBase => cardDatabase;
        
        private void Awake()
        {
            instance = this;
        }

        private IEnumerator Start()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            
            if (currentSceneName == parameterScene.SceneName)
                yield return StartNewRun(true);
            else if (currentSceneName == rewardScene)
            {
                Debug.Log($"Game was not started in scene : {parameterScene.SceneName} => RunLoop will stop itself.");
                yield break;
            }
            else
            {
                Debug.Log($"Game was not started in scene : {parameterScene.SceneName} => RunLoop will self destroy.");
                Destroy(gameObject);
            }
        }

        private IEnumerator StartNewRun(bool alreadyInParameterScene)
        {
            if (!alreadyInParameterScene)
                yield return LoadScene(parameterScene);
            yield return new WaitUntil(AreParametersSet);

            currentRunParameterData = RetrieveRunParameters();
            
            bool isPlayerAlive = true;
            while (isPlayerAlive)
            {
                yield return LoadScene(rewardScene);
                yield return new WaitUntil(IsRewardSelected);
                
                yield return LoadScene(combatScene);
                yield return new WaitUntil(IsCombatOver);
                isPlayerAlive = CheckCombatResult();
                StoreCardsHealth();
                
                if (IsRunOver() || !isPlayerAlive)
                    break;
            }

            if (!isPlayerAlive)
            {
                PlayerDeck.instance.ClearDeck();
                yield return StartNewRun(false);
            }
        }

        private RunParameterData RetrieveRunParameters()
        {
            return RunParameterGatherer.instance.selectedRunParameter;
        }

        private bool AreParametersSet()
        {
            return RunParameterGatherer.instance.isParameterSelected;
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

            var operation = SceneManager.LoadSceneAsync(sceneField.SceneName);
            yield return new WaitUntil(() => operation.isDone);
            
            yield return Fader.Fade(blackScreen, 0.3f, false);
        }
    }
}
