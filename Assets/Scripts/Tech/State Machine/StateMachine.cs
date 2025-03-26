using System;
using System.Collections.Generic;

namespace Tech.State_Machine
{
    public class StateMachine<StateID, BState> where StateID : Enum where BState : BaseState
    {
        public readonly Dictionary<StateID, BState> _states = new ();
        
        public StateID CurrentStateID { get; private set; }
        public BState CurrentState { get; private set; }

        public void AddNewState(StateID stateID, BState newState)
        {
            _states.Add(stateID, newState);
        }
        
        public virtual void Initialize(StateID startedState)
        {
            CurrentState = _states[startedState];
            CurrentStateID = startedState;
            CurrentState.Enter();
        }

        public void ChangeState(StateID newStateID)
        {
            var newState = _states[newStateID];
            
            if (CurrentState == newState) return;
            
            CurrentState.Exit();
            CurrentStateID = newStateID;
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}