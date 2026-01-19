using ActionReaction;
using ActionReaction.Game_Actions;
using UnityEngine;
using static Combat.CombatLoop;

namespace RunTracker
{
    public class FightTracker : MonoBehaviour
    {
        private int totalDMG;
        private int avgDMGPerTurn;
        private int thisTurnDMG;
        private int lastTurnDMG;

        private int turnCounter;
        private int playerTurns;

        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<DealDamageGA>(DamageTrack, ReactionTiming.POST);
            ActionSystem.SubscribeReaction<StartTurnGa>(UpdateTurn, ReactionTiming.PRE);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<DealDamageGA>(DamageTrack, ReactionTiming.POST);
            ActionSystem.UnsubscribeReaction<StartTurnGa>(UpdateTurn, ReactionTiming.PRE);
        }

        void DamageTrack(DealDamageGA gA)
        {
            /*if (IsPlayerTurn() == false)
            return;

        if (ShouldNotBeCounted(gA))
            return;
        */
            avgDMGPerTurn = (avgDMGPerTurn + gA.amount) / playerTurns;
            thisTurnDMG += gA.amount;
            totalDMG += gA.amount;

            Debug.Log("TOUP Average DMG per turn : " + avgDMGPerTurn);
            Debug.Log("TOUP This Turn DMG : " + thisTurnDMG);
            Debug.Log("TOUP Last Turn DMG : " + lastTurnDMG);
            Debug.Log("TOUP Total Damage : " + totalDMG);

            //UpdateDisplay
        }

        void UpdateTurn(StartTurnGa gA)
        {
            turnCounter += 1;

            if (gA.starting == TurnType.Player)
            {
                playerTurns++;
                lastTurnDMG = thisTurnDMG;
                thisTurnDMG = 0;
                //UpdateDisplay;
            }
            else
                return;
        }

        /*private bool ShouldNotBeCounted(DealDamageGA gA)
    {
        if (gA.target.enemyCardController == null && IsPlayerTurn() == true)
            return true;
        else
            return true;
    }
    */
    }
}
