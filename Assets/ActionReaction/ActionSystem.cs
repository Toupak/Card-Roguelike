using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ReSharper disable CanSimplifyDictionaryLookupWithTryGetValue

namespace ActionReaction
{
    public enum ReactionTiming
    {
        PRE,
        POST
    }
    
    //For information on wtf is going on here : https://www.youtube.com/watch?v=ls5zeiDCfvI 
    public class ActionSystem : MonoBehaviour
    {
        public static ActionSystem instance;

        private List<GameAction> reactions = null;
        public bool IsPerforming { get; private set; } = false;
        private static Dictionary<Type, List<Action<GameAction>>> preSubs = new();
        private static Dictionary<Type, List<Action<GameAction>>> postSubs = new();
        private static Dictionary<Type, Func<GameAction, IEnumerator>> performers = new();

        private void Awake()
        {
            instance = this;
        }
        
        public void Perform(GameAction action, Action OnPerformFinished = null)
        {
            if (IsPerforming)
                return;

            IsPerforming = true;

            StartCoroutine(Flow(action, () =>
            {
                IsPerforming = false;
                OnPerformFinished?.Invoke();
            }));
        }

        public void AddReaction(GameAction gameAction)
        {
            reactions?.Add(gameAction);
        }

        private IEnumerator Flow(GameAction action, Action OnFlowFinished = null)
        {
            reactions = action.PreReactions;
            PerformSubscribers(action, preSubs);
            yield return PerformReactions();
            
            reactions = action.PerformReactions;
            yield return PerformPerformer(action);
            yield return PerformReactions();

            reactions = action.PostReactions;
            PerformSubscribers(action, postSubs);
            yield return PerformReactions();
            
            OnFlowFinished?.Invoke();
        }

        private void PerformSubscribers(GameAction action, Dictionary<Type,List<Action<GameAction>>> subs)
        {
            Type type = action.GetType();
            if (subs.ContainsKey(type))
            {
                foreach (Action<GameAction> sub in subs[type])
                {
                    sub(action);
                }
            }
        }

        private IEnumerator PerformReactions()
        {
            foreach (GameAction reaction in reactions)
            {
                yield return Flow(reaction);
            }
        }

        private IEnumerator PerformPerformer(GameAction action)
        {
            Type type = action.GetType();
            if (performers.ContainsKey(type))
            {
                yield return performers[type](action);
            }
        }

        public static void AttachPerformer<T>(Func<T, IEnumerator> performer) where T : GameAction
        {
            Type type = typeof(T);
            IEnumerator wrappedPerformer(GameAction action) => performer((T)action);

            if (performers.ContainsKey(type))
                performers[type] = wrappedPerformer;
            else
                performers.Add(type, wrappedPerformer);
        }
        
        public static void DetachPerformer<T>() where T : GameAction
        {
            Type type = typeof(T);
            if (performers.ContainsKey(type))
                performers.Remove(type);
        }

        public static void SubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
        {
            Type type = typeof(T);
            Dictionary<Type, List<Action<GameAction>>> subs = timing == ReactionTiming.PRE ? preSubs : postSubs;
            void wrappedReaction(GameAction action) => reaction((T)action);
            
            if (subs.ContainsKey(type))
                subs[type].Add(wrappedReaction);
            else
            {
                subs.Add(type, new List<Action<GameAction>>());
                subs[type].Add(wrappedReaction);
            }
        }
        
        public static void UnsubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
        {
            Type type = typeof(T);
            Dictionary<Type, List<Action<GameAction>>> subs = timing == ReactionTiming.PRE ? preSubs : postSubs;

            if (subs.ContainsKey(type))
            {
                void wrappedReaction(GameAction action) => reaction((T)action);
                subs[type].Remove(wrappedReaction);
            }
        }
    }
}
