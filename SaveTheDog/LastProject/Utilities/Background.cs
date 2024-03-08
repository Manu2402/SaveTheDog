using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace LastProject
{
    class Background
    {
        protected Texture tilesetTexture;
        public TmxMap TiledMap;

        public Background(string tilesetPath, string XMLPath)
        {
            tilesetTexture = GfxMgr.GetTexture(tilesetPath);
            TiledMap = new TmxMap(XMLPath);
        }

        public void Draw()
        {
            TiledMap.Draw();
        }

        public void Destroy()
        {
            tilesetTexture = null;
            TiledMap.Destroy();
        }
        
    }
}
