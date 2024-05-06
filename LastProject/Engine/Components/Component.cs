using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastProject
{
    enum ComponentType
    {
        SoundEmitter,
        RandomizedSoundEmitter,
        Animation
    }

    abstract class Component //Componente (funzionalità) applicato ai GameObject
    {
        public GameObject GameObject { get; protected set; }
        protected bool isEnabled;

        public bool IsEnabled
        {
            get { return isEnabled && GameObject.IsActive; }
            set { isEnabled = value; }
        }

        public Component(GameObject owner)
        {
            GameObject = owner;
        }

    }
}
