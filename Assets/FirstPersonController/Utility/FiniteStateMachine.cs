using System;
using System.Collections.Generic;
using Util.StatePattern;

namespace FirstPersonController.Utility {
    
    public class FiniteStateMachine<TState> {
        private TState m_State;
        private IState m_DefaultState;
        private Dictionary<Type, List<Transition>> m_Transitions = new Dictionary<Type,List<Transition>>();
        private List<Transition> m_CurrentTransition = new List<Transition>();
        private List<Transition> m_AnyTransitions = new List<Transition>();
        private static List<Transition> EmptyTransitions = new List<Transition>(0);

        private class Transition 
        {
            public Func<bool> Condition { get; }
            public TState To { get; }

            public Transition(TState to, Func<bool> condition) 
            {
                To = to;
                Condition = condition;
            }
        }
        
        public void Update() 
        {
            //check if a transition is needed
            var transition = GetTransition();
            if (transition != null)
            {
                ChangeState(transition.To);
            }

            m_DefaultState?.Update();
        }

        public void FixedUpdate()
        {
            m_DefaultState?.FixedUpdate();
        }

        public void ChangeState(TState state) 
        {
            //check if state is equal, if the same no need to change
            if (m_State.Equals(state))
            {
                return;
            }

            //Set the state to the new state, set default state if possible
            m_DefaultState = m_State as IState;
            m_DefaultState?.OnExit();
            m_State = state;

            //Get the transitions of the new state
            m_Transitions.TryGetValue(m_State.GetType(), out m_CurrentTransition);
            if (m_CurrentTransition == null)
            {
                m_CurrentTransition = EmptyTransitions;
            }

            m_DefaultState?.OnEnter();
        }

        //add transition for any state
        public void AddTransition(TState state, Func<bool> predicate)
        {
            m_AnyTransitions.Add(new Transition(state, predicate));
        }

        //add transition fot specific state
        public void AddTransition(TState from, TState to, Func<bool> predicate) 
        {
            //add new entry if key doesnt exist
            if (!m_Transitions.TryGetValue(from.GetType(), out var transitions)) 
            {
                transitions = new List<Transition>();
                m_Transitions[from.GetType()] = transitions;
            }
            
            transitions.Add(new Transition(to, predicate));
        }
        
        private Transition GetTransition() 
        {
            //check if condition is met for any state
            foreach (var transition in m_AnyTransitions) 
            {
                if (transition.Condition())
                {
                    return transition;
                }
            }

            //Check if condition is met for current state
            foreach (var transition in m_CurrentTransition) 
            {
                if (transition.Condition())
                {
                    return transition;
                }
            }

            return null;
        }
    }
}