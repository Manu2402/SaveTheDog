using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastProject
{
    class TextObject
    {
        protected List<TextChar> sprites;
        protected string text;
        protected Font font;
        protected bool isActive;
        protected float hSpace;

        protected char newLineChar;

        protected Vector2 position;

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; UpdateCharStatus(); }
        }

        public string Text
        {
            get { return text; }
            set { SetText(value); }
        }

        public TextObject(Vector2 position, string textString = "", Font font = null, float horizontalSpacing = 0)
        {
            this.position = position;
            hSpace = horizontalSpacing;

            newLineChar = '\n';

            if (font == null)
            {
                //get default font
                font = FontMgr.GetFont();
            }

            this.font = font;

            sprites = new List<TextChar>();

            if (textString != "")
            {
                SetText(textString);
            }
        }

        protected void SetText(string newText)
        {
            if (newText != text)
            {
                text = newText;
                int numChars = text.Length;
                float charX = position.X;
                float charY = position.Y;

                for (int i = 0; i < numChars; i++)
                {
                    char c = text[i];//string as char array

                    if (i >= sprites.Count)//i is greater than last char index
                    {
                        TextChar tc = new TextChar(new Vector2(charX, charY), c, font);
                        tc.IsActive = isActive;
                        sprites.Add(tc);
                    }
                    else if (c != sprites[i].Character)
                    {
                        sprites[i].Character = c;
                    }

                    //newLine for Text
                    if (c == newLineChar)
                    {
                        charY += sprites[i].HalfHeight * 2;
                        charX = position.X;
                    }
                    else
                    {
                        charX += sprites[i].HalfWidth * 2 + hSpace;
                    }
                }

                if (sprites.Count > numChars)
                {
                    int count = sprites.Count - numChars;
                    int startCut = numChars;

                    for (int i = startCut; i < sprites.Count; i++)
                    {
                        sprites[i].Destroy();
                    }

                    sprites.RemoveRange(startCut, count);
                }
            }
        }

        protected virtual void UpdateCharStatus()
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                sprites[i].IsActive = isActive;
            }
        }

        public void Destroy()
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                sprites[i].Destroy();
            }

            sprites.Clear();

            font.Destroy();
        }

    }
}
