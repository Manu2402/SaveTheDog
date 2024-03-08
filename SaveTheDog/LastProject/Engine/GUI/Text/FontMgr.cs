using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastProject
{
    static class FontMgr
    {
        private static Dictionary<string, Font> fonts;
        private static Font defaultFont;

        static FontMgr()
        {
            fonts = new Dictionary<string, Font>();
        }

        public static Font AddFont(string fontName, string texturePath, int numColumns, int firstCharacterASCIIvalue, int charWidth, int charHeight)
        {
            Font f;
            if (!fonts.ContainsKey(fontName))
            {
                f = new Font(fontName, texturePath, numColumns, firstCharacterASCIIvalue, charWidth, charHeight);
                fonts.Add(fontName, f);

                if (defaultFont == null)
                {
                    defaultFont = f;
                }
            }
            else
            {
                f = GetFont(fontName);
            }

            return f;
        }

        public static Font GetFont(string fontName = "")
        {
            if (fontName != "" && fonts.ContainsKey(fontName))
            {
                return fonts[fontName];
            }

            return defaultFont;
        }

        public static void ClearAll()
        {
            fonts.Clear();
            defaultFont = null;
        }
    }
}
