using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Aiv.Fast2D;
using OpenTK;

namespace LastProject
{
    class TmxLayer
    {
        private Texture tilesetImage;
        private Sprite layerSprite;
        private Texture layerTexture;
        private TmxTileset tileset;
        private int rows, cols;
        private int tileW, tileH;

        public string[] IDs { get; }

        public TmxLayer(XmlNode layerNode, TmxTileset tileset, int cols, int rows, int tileW, int tileH)
        {
            XmlNode dataNode = layerNode.SelectSingleNode("data");
            string csvData = dataNode.InnerText;
            csvData = csvData.Replace("\r\n", "").Replace("\n", "").Replace(" ", "").Replace("\t", "");

            string[] Ids = csvData.Split(',');
            IDs = Ids;

            this.cols = cols;
            this.rows = rows;
            this.tileW = tileW;
            this.tileH = tileH;
            this.tileset = tileset;

            SetMap();

            layerSprite = new Sprite(Game.PixelsToUnits(801), Game.PixelsToUnits(800));
        }

        public void SetMap()
        {
            tilesetImage = GfxMgr.GetTexture(tileset.TextureName);

            layerTexture = new Texture((int)Game.UnitsToPixels(rows), (int)Game.UnitsToPixels(cols));
            byte[] mapBitmap = new byte[layerTexture.Width * layerTexture.Height * 4];

            Texture tilesetTexture = GfxMgr.GetTexture("tileset");
            byte[] tilesetBitmap = tilesetTexture.Bitmap;

            int bytesPerPixel = 4;
            int tilesetBitmapRowLength = 256 * bytesPerPixel;
            int mapBitmapRowLength = layerTexture.Width * bytesPerPixel;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    int tileId = int.Parse(IDs[r * cols + c]);

                    if (tileId > 0)
                    {
                        int tilesetXOff = tileset.GetAtIndex(tileId).X * bytesPerPixel;
                        int tilesetYOff = tileset.GetAtIndex(tileId).Y * tilesetBitmapRowLength;

                        int tilesetBitmapIndexInitial = tilesetYOff + tilesetXOff;

                        int mapXOff = c * tileW * bytesPerPixel;
                        int mapYOff = r * tileH * mapBitmapRowLength;

                        int mapBitmapIndexInitial = mapXOff + mapYOff;

                        for (int i = 0; i < tileH; i++)
                        {
                            int tilesetBitmapIndexUpdate = i * tilesetBitmapRowLength;
                            int mapBitmapIndexUpdate = i * mapBitmapRowLength;

                            Array.Copy(tilesetBitmap,
                                       tilesetBitmapIndexInitial + tilesetBitmapIndexUpdate,
                                       mapBitmap,
                                       mapBitmapIndexInitial + mapBitmapIndexUpdate,
                                       tileW * bytesPerPixel);
                        }
                    }
                }
            }

            layerTexture.Update(mapBitmap);
        }

        public void Draw()
        {
            layerSprite.DrawTexture(layerTexture);
        }

        public void Destroy()
        {
            tilesetImage = null;
            layerSprite = null;
            layerTexture = null;
            tileset = null;
        }
    }
}
