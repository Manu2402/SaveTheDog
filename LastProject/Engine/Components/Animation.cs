using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace LastProject
{
    class Animation : Component, IUpdatable //Animation: ripresa dal progetto CoffinDance
    {
        protected int numFrames;
        protected float frameDuration;
        protected int currentFrame;
        protected float elapsedTime;
        protected bool isPlaying;
        public bool Loop;
        

        public int FrameWidth { get; protected set; }
        public int FrameHeight { get; protected set; }
        public Vector2 Offset { get; protected set; }
        public float FramesPerSecond { get => 1 / frameDuration; set => frameDuration = 1 / value; }


        public Animation(GameObject owner, int numFrames, int frameW, int frameH, float framesPerSecond, bool loop = true) : base(owner)
        {
            this.numFrames = numFrames;
            FrameWidth = frameW;
            FrameHeight = frameH;
            Loop = loop;
            frameDuration = 1 / framesPerSecond;

            Offset = Vector2.Zero;

            UpdateMgr.AddItem(this);
        }

        public virtual void Play()
        {
            isPlaying = true;
        }

        public virtual void Restart()
        {
            currentFrame = 0;
            elapsedTime = 0f;
            Play();
        }

        public virtual void Stop()
        {
            currentFrame = 0;
            elapsedTime = 0f;
            isPlaying = false;
        }

        public virtual void StopAtStart()
        {
            Stop();
            Offset = Vector2.Zero;
        }

        public virtual void Pause()
        {
            isPlaying = false;
        }

        protected virtual void OnAnimationEnd()
        {
            isPlaying = false;
            GameObject.IsActive = false;
        }

        public void Update()
        {
            if (IsEnabled && isPlaying) //Agisco solo se il component è attivo e l'animazione è attiva
            {
                elapsedTime += Game.DeltaTime;

                if(elapsedTime >= frameDuration)
                {
                    currentFrame++;
                    elapsedTime = 0f;

                    if(currentFrame >= numFrames)
                    {
                        if (Loop)
                        {
                            currentFrame = 0;
                        }
                        else
                        {
                            OnAnimationEnd();
                            return;
                        }
                    }

                    Offset = new Vector2(currentFrame * FrameWidth, Offset.Y);
                }
            }
        }

    }
}
