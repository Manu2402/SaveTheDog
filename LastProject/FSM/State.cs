using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastProject
{
    abstract class State
    {
        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public abstract void Update();

        public Player Owner { get; protected set; }

        protected StateMachine fsm;

        public State(Player owner)
        {
            Owner = owner;
        }


        public void SetStateMachine(StateMachine sm)
        {
            fsm = sm;
        }

    }
}
