using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastProject
{
    class PlayState : State
    {
        public PlayState(Player owner) : base(owner) { }

        public override void Update()
        {
            Owner.Input();
        }
    }
}
