using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Audio;

namespace LastProject
{
    enum ScenesType //List of all the Scenes
    {
        Title, Main, PlayerHouse, PrincessHouse, Underground, DogHouse, GameOver, Win, LAST
    }

    abstract class Scene
    {
        public bool IsPlaying { get; set; }

        //Sounds
        public AudioSource[] sourceEmitter;
        public AudioClip[] clips;

        public Scene NextScene;

        public Scene() { }

        public virtual void Start()
        {
            IsPlaying = true;
        }

        public virtual Scene OnExit()
        {
            IsPlaying = false;
            return NextScene;
        }

        protected virtual void LoadAssets() { }
        public virtual void Update() { }
        public virtual void Reload() { }

        public abstract void Input();
        public abstract void Draw();
    }
}
