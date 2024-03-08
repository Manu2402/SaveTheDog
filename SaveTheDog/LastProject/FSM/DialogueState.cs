using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;

namespace LastProject
{
    enum DialogueName //List Name for any Dialogue
    {
        Start,
        Rock,
        RockDestroyedFirst,
        RockDestroyed,
        DoorLocked,
        DoorUnlocked,
        Princess,
        PricessAfterPickedUpKey,
        BookPlayerHouse,
        BookPrincessHouse,
        Pot,
        BootsPickup,
        KeyPickup,
        PickaxePickup,
        Dog,
    }

    class DialogueState : State
    {
        private DialogueBar dialogueBar; //Bar for dialogues
        private bool isEnterPressed = true;
        private string wholeText; //Text
        private string nameText; //Name

        private int index; //Index for select a dialogue

        private Dictionary<DialogueName, Tuple<string, string[]>> dialogues;
        //Key: code name of the dialogue
        //Value: name text, series of texts for dialogue

        private DialogueName currentDialogue; //Enum to access in dialogues

        public DialogueState(Player owner) : base(owner)
        {
            dialogues = new Dictionary<DialogueName, Tuple<string, string[]>>();

            //All dialogues

            string[] startDialogue = new string[] { "Hi, excuse me I need your help.\n" +
                                                    "In the house next to mine I heard the\n" +
                                                    "bark of a dog, it seemed to be crying.\n" +
                                                    "I tried to open the door but\n" +
                                                    "it was locked. That dog is suffering.\n" +
                                                    "Please help me to find the key!",

                                                    "You may find clues inside the houses.\n" +
                                                    "Mine is the central one,\n" +
                                                    "just drop by :)"};

            string[] rockDialogue = new string[] { "You must have a pickaxe to destroy\n" +
                                                   "this rock!" };

            string[] doorLockedDialogue = new string[] { "The door is locked!" };

            string[] doorUnlockedDialogue = new string[] { "You unlocked the door!" };

            string[] princessDialogue = new string[] { "Have not found the key yet?\n" +
                                                       "Try searching better inside the houses." };

            string[] pricessAfterPickedUpKeyDialogue = new string[] { "You found the key, great!\n" +
                                                                      "Now go to the house with the red\n" +
                                                                      "roof to free it!!" };

            string[] bookPlayerHouseDialogue = new string[] { "To find the key you will\n" +
                                                              "face a challenge. Inside a basement\n" +
                                                              "you will find a series of platforms\n" +
                                                              "and, to unlock the key, you will\n" +
                                                              "need to press them all in the correct\n" +
                                                              "order. Every time you fail you will get\n" +
                                                              "hurt, be careful not to get killed!\n" +

                                                              "But perhaps this is not the right\n" +
                                                              "house to proceed..." };

            string[] bookPrincessHouseDialogue = new string[] { "There might be an object you\n" +
                                                                "could interact with in this room..." };

            string[] potDialogue = new string[] { "This pot has something strange...",

                                                  "You have activated a secret passage!"};

            string[] bootsPickupDialogue = new string[] { "You have collected your boots!\n" +
                                                          "Now you will move faster." };

            string[] keyPickupDialogue = new string[] { "You have collected a key!" };

            string[] pickaxePickupDialogue = new string[] { "You have collected a pickaxe!\n" +
                                                            "Who knows what it will do :?" };

            string[] dogDialogue = new string[] {"You saved me! Thank you!\n" +
                                                 "I thought I was stuck here\n" +
                                                 "for a long time... ",

                                                 "Congratulations, you completed\n" +
                                                 "the demo!"};

            string[] rockDestroyedDialogue = new string[] {"Hi Matteo Chironi, I am Manuel, the\n" +
                                                           "developer of this game. I have a\n" +
                                                           "proposal for you. If you send me a\n" +
                                                           "heart (<3) in direct on my Instagram\n" +
                                                           "profile: \"manuel02._\" I will send you\n" +
                                                           "a video where I buy \"More Ducks \n" +
                                                           "Everywhere\" on Steam. Are you there? :)",

                                                           "I also take this opportunity to thank\n" +
                                                           "you and thank all the profs. for the\n" +
                                                           "work you have done. I will wait for\n" +
                                                           "your message if it does not bother you."};

            string[] rockDestroyedFirstDialogue = new string[] {"There is something under this rock.",

                                                                "You have unlocked an Easter Egg...",

                                                                rockDestroyedDialogue[0],
                                                                rockDestroyedDialogue[1] };

            dialogues.Add(DialogueName.Start, new Tuple<string, string[]>("Princess", startDialogue));
            dialogues.Add(DialogueName.Rock, new Tuple<string, string[]>("Someone", rockDialogue));
            dialogues.Add(DialogueName.DoorLocked, new Tuple<string, string[]>("Someone", doorLockedDialogue));
            dialogues.Add(DialogueName.Princess, new Tuple<string, string[]>("Princess", princessDialogue));
            dialogues.Add(DialogueName.PricessAfterPickedUpKey, new Tuple<string, string[]>("Princess", pricessAfterPickedUpKeyDialogue));
            dialogues.Add(DialogueName.BookPlayerHouse, new Tuple<string, string[]>("Book", bookPlayerHouseDialogue));
            dialogues.Add(DialogueName.BookPrincessHouse, new Tuple<string, string[]>("Book", bookPrincessHouseDialogue));
            dialogues.Add(DialogueName.Pot, new Tuple<string, string[]>("Someone", potDialogue));
            dialogues.Add(DialogueName.BootsPickup, new Tuple<string, string[]>("Someone", bootsPickupDialogue));
            dialogues.Add(DialogueName.KeyPickup, new Tuple<string, string[]>("Someone", keyPickupDialogue));
            dialogues.Add(DialogueName.DoorUnlocked, new Tuple<string, string[]>("Someone", doorUnlockedDialogue));
            dialogues.Add(DialogueName.PickaxePickup, new Tuple<string, string[]>("Someone", pickaxePickupDialogue));
            dialogues.Add(DialogueName.Dog, new Tuple<string, string[]>("Dog", dogDialogue));
            dialogues.Add(DialogueName.RockDestroyedFirst, new Tuple<string, string[]>("Manuel", rockDestroyedFirstDialogue));
            dialogues.Add(DialogueName.RockDestroyed, new Tuple<string, string[]>("Manuel", rockDestroyedDialogue));

            index = 0;
        }

        //Get the specific texts by the name
        public void SetDialogue(DialogueName name)
        {
            if (name != currentDialogue)
            {
                currentDialogue = name;
            }

            nameText = dialogues[currentDialogue].Item1;
            wholeText = dialogues[currentDialogue].Item2[index];
        }

        public override void OnEnter()
        {
            dialogueBar = new DialogueBar("dialogueBar", DrawLayer.Foreground);
            dialogueBar.IsActive = true;
        }

        public override void OnExit()
        {
            dialogueBar.Destroy();
        }

        public void Input()
        {
            if (Game.Window.GetKey(KeyCode.Return))
            {
                if (!isEnterPressed)
                {
                    Game.CurrentScene.sourceEmitter[(int)MainNameClips.DialogueSFX].Play(Game.CurrentScene.clips[(int)MainNameClips.DialogueSFX]);

                    isEnterPressed = true;

                    //Check index to continue the dialogue
                    index++;

                    if (index >= dialogues[currentDialogue].Item2.Length)
                    {
                        index = 0;
                        fsm.GoTo(StateEnum.PLAY);
                        return;
                    }

                    SetDialogue(currentDialogue);
                }
            }
            else
            {
                isEnterPressed = false;
            }

        }

        public override void Update()
        {
            Input();
            dialogueBar.SetText(wholeText, nameText);
        }

    }
}
