using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastProject
{
    enum StateEnum
    {
        DIALOGUE, PLAY
    }

    class StateMachine
    {
        private Dictionary<StateEnum, State> states;
        private State current;

        public StateMachine()
        {
            states = new Dictionary<StateEnum, State>();
            current = null;
        }

        public State GetState(StateEnum key)
        {
            if (states.ContainsKey(key))
            {
                return states[key];
            }

            return null;
        }

        public State GetCurrentState()
        {
            return current;
        }

        public void AddState(StateEnum key, State value)
        {
            states[key] = value;
            value.SetStateMachine(this);
        }

        public void GoTo(StateEnum key)
        {
            if (current != null)
            {
                current.OnExit();
            }

            current = states[key];
            current.OnEnter();
        }

        public void Update()
        {
            if (current != null) current.Update();
        }

        public void Clear()
        {
            states.Clear();
            states = null;
            current = null;
        }

    }
}