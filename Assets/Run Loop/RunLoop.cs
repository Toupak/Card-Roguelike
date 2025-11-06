using System;
using System.Collections;
using Run_Loop.Run_Parameters;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Run_Loop
{
    public class RunLoop : MonoBehaviour
    {
        [SerializeField] private SceneField parameterScene;
        [SerializeField] private SceneField rewardScene;
        [SerializeField] private SceneField combatScene;

        public static RunLoop instance;
        
        public RunParameterData currentRunParameterData { get; private set; }

        private void Awake()
        {
            instance = this;
        }

        private IEnumerator Start()
        {
            if (SceneManager.GetActiveScene().name == parameterScene.SceneName)
                yield return StartNewRun(true);
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
                StoreRewards();
                
                yield return LoadScene(combatScene);
                yield return new WaitUntil(IsCombatOver);
                isPlayerAlive = CheckCombatResult();
                
                if (IsRunOver())
                    break;
            }

            if (!isPlayerAlive)
                yield return StartNewRun(false);
        }

        private RunParameterData RetrieveRunParameters()
        {
            throw new System.NotImplementedException();
        }

        private bool AreParametersSet()
        {
            return false;
        }

        private bool IsRewardSelected()
        {
            return false;
        }

        private void StoreRewards()
        {
            
        }

        private bool IsCombatOver()
        {
            return false;
        }

        private bool CheckCombatResult()
        {
            return true;
        }
        
        private bool IsRunOver()
        {
            return false;
        }

        private IEnumerator LoadScene(SceneField sceneField)
        {
            yield break;
        }
    }
}
