using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace LastProject
{
    class Player : Actor
    {
        enum Animations
        {
            WalkUp, WalkDown, WalkLeft, WalkRight, LAST
        }

        //References
        public InventoryGUI InventoryGUI;

        protected Agent agent;

        protected Animation walkAnimation;
        protected Animation deathAnimation;
        protected Texture spriteSheetDeath;

        protected Animation currentAnimation;
        protected Texture[] textures;

        protected StateMachine fsm;

        public Agent Agent { get => agent; set => agent = value; }
        public StateMachine Fsm { get => fsm; }

        public Player(string textureStart) : base(textureStart, spriteWidth: Game.PixelsToUnits(Game.UnitSize))
        {
            IsActive = true;
            maxSpeed = 6f;

            Energy = Game.LastEnergyPlayer;

            sprite.pivot = Vector2.Zero;

            //Rigidbody
            RigidBody = new RigidBody(this);
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            RigidBody.Type = RigidBodyType.Player;
            RigidBody.AddCollisionType(RigidBodyType.NPC | RigidBodyType.Objects);

            //Animation
            walkAnimation = new Animation(this, 4, 16, 16, 8);
            deathAnimation = new Animation(this, 2, 16, 16, 2, false);

            textures = new Texture[(int)Animations.LAST];

            for (int i = 0; i < textures.Length; i++)
            {
                textures[i] = GfxMgr.GetTexture($"PlayerWalk_{i}");
            }

            spriteSheetDeath = GfxMgr.GetTexture("Death");

            walkAnimation.IsEnabled = true;
            deathAnimation.IsEnabled = true;

            //Flag for increase movement speed also in other scenes
            if (Game.HasBoots) SpeedUp();

            //adaptation of the single frame of the spritesheet
            frameW = (int)Game.UnitSize;

            Reset();

            //GUI
            InventoryGUI = new InventoryGUI(new Vector2(1f, 1f));
            InventoryGUI.IsActive = true;

            //Pathfinding
            Agent = new Agent(this);

            //FSM
            fsm = new StateMachine();

            DialogueState dialogueState = new DialogueState(this);
            PlayState playState = new PlayState(this);

            fsm.AddState(StateEnum.DIALOGUE, dialogueState);
            fsm.AddState(StateEnum.PLAY, playState);

            //Check for enter state (start of game and the rest)
            if (Game.CurrentScene.GetType() == typeof(MainScene) && ((MainScene)Game.CurrentScene).FirstLoad)
            {
                fsm.GoTo(StateEnum.DIALOGUE);
                dialogueState.SetDialogue(DialogueName.Start);
            }
            else
            {
                fsm.GoTo(StateEnum.PLAY);
            }

            UpdateMgr.AddItem(this);
            DrawMgr.AddItem(this);
        }

        public void Input()
        {
            //Pathfinding
            if (Game.Window.MouseLeft)
            {
                //Absolute position
                Vector2 mousePos = CameraMgr.MainCamera.position - CameraMgr.MainCamera.pivot + Game.Window.MousePosition;
                List<Node> path = ((MapScene)Game.CurrentScene).Map.GetPath((int)Position.X, (int)Position.Y, (int)mousePos.X, (int)mousePos.Y);
                agent.SetPath(path);
            }
        }

        public void SpeedUp()
        {
            maxSpeed = 9f;
            walkAnimation.FramesPerSecond = 12f;
        }

        public override void Update()
        {
            if (IsActive)
            {
                agent.Update();
                CheckSignsAnimation();
                currentAnimation = walkAnimation;
            }
        }

        public void UpdateStateMachine()
        {
            fsm.Update();
        }

        //Select right spritesheet for animation
        protected void CheckSignsAnimation()
        {
            if (Agent.Target == null) //No path
            {
                walkAnimation.StopAtStart();
                return;
            }
            else if (Velocity.X == 1f)
            {
                //Right
                texture = textures[(int)Animations.WalkRight];
            }
            else if (Velocity.X == -1f)
            {
                //Left
                texture = textures[(int)Animations.WalkLeft];
            }
            else if (Velocity.Y == 1f)
            {
                //Down
                texture = textures[(int)Animations.WalkDown];
            }
            else if (Velocity.Y == -1f)
            {
                //Up
                texture = textures[(int)Animations.WalkUp];
            }

            walkAnimation.Play();
        }

        public override void OnDie()
        {
            texture = spriteSheetDeath;
            currentAnimation = deathAnimation;
            currentAnimation.Play();
            ((MapScene)Game.CurrentScene).NextScene = Game.Scenes[(int)ScenesType.GameOver];
            ((MapScene)Game.CurrentScene).IsPlaying = false;
        }

        public void ResetSpawnUnderground()
        {
            Position = new Vector2(24, 16);
            Agent.Target = null;
            texture = textures[(int)Animations.WalkDown];
        }

        public override void Draw()
        {
            if (IsActive)
            {
                sprite.DrawTexture(texture, (int)currentAnimation.Offset.X, (int)currentAnimation.Offset.Y, frameW, frameH);
            }
        }

        public override void Destroy()
        {
            base.Destroy();

            InventoryGUI.Destroy();
            agent.Destroy();

            walkAnimation = null;
            deathAnimation = null;
            spriteSheetDeath = null;
            currentAnimation = null;

            Fsm.Clear();
            for (int i = 0; i < textures.Length; i++)
            {
                textures[i] = null;
            }

            textures = null;
        }

    }
}
