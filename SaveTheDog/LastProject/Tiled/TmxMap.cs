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
    class TmxMap : IDrawable
    {
        private string tmxFilePath;
        public Map Map;
        public XmlNode mapNode;

        public DrawLayer Layer { get; }

        private TmxTileset tileset;
        private TmxLayer[] layers;

        public TmxMap(string filePath)
        {
            Layer = DrawLayer.Background;
            DrawMgr.AddItem(this);

            tmxFilePath = filePath;

            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(tmxFilePath);
            }
            catch (XmlException e)
            {
                Console.WriteLine("XML Exception: " + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Generic Exception: " + e.Message);
            }

            XmlNode mapNode = xmlDoc.SelectSingleNode("map");
            this.mapNode = mapNode;

            int mapCols = GetIntAttribute(mapNode, "width");
            int mapRows = GetIntAttribute(mapNode, "height");
            int mapTileW = GetIntAttribute(mapNode, "tilewidth");
            int mapTileH = GetIntAttribute(mapNode, "tileheight");

            XmlNode tilesetNode = mapNode.SelectSingleNode("tileset");
            int tilesetTileW = GetIntAttribute(tilesetNode, "tilewidth");
            int tilesetTileH = GetIntAttribute(tilesetNode, "tileheight");
            int tileCount = GetIntAttribute(tilesetNode, "tilecount");
            int tilesetCols = GetIntAttribute(tilesetNode, "columns");
            int tilesetRows = tileCount / tilesetCols;

            tileset = new TmxTileset("tileset", tilesetCols, tilesetRows, tilesetTileW, tilesetTileH);

            XmlNodeList layersNodes = mapNode.SelectNodes("layer");

            layers = new TmxLayer[layersNodes.Count];

            for (int i = 0; i < layersNodes.Count; i++)
            {
                layers[i] = new TmxLayer(layersNodes[i], tileset, mapCols, mapRows, mapTileW, mapTileH);
            }

            //Initialize array for pathfinding nodes
            #region GetPropertiesForPathfinding

            int[] cells = new int[mapRows * mapCols];

            XmlNodeList tileIDs = tilesetNode.SelectNodes("tile");

            for (int i = 0; i < layers.Length; i++)
            {
                for (int j = 0; j < cells.Length; j++)
                {
                    int tileId = int.Parse(layers[i].IDs[j]);

                    for (int k = 0; k < tileIDs.Count; k++)
                    {
                        //No element in layer when show 0 in XML
                        if (tileId == 0)
                        {
                            continue;
                        }
                        else if (tileId == GetIntAttribute(tileIDs[k], "id") + 1)
                        {
                            XmlNode reachableProp = tileIDs[k].SelectSingleNode("properties/property");
                            bool reachable = GetBoolAttribute(reachableProp, "value");
                            cells[j] = (reachable) ? 1 : 0;
                            break;
                        }
                        else
                        {
                            cells[j] = 0;
                        }
                    }
                }
            }

            #endregion

            Map = new Map(mapRows, mapCols, cells);
        }

        public static int GetIntAttribute(XmlNode node, string attrName)
        {
            return int.Parse(GetStringAttribute(node, attrName));
        }

        public static bool GetBoolAttribute(XmlNode node, string attrName)
        {
            return bool.Parse(GetStringAttribute(node, attrName));
        }

        public static string GetStringAttribute(XmlNode node, string attrName)
        {
            return node.Attributes.GetNamedItem(attrName).Value;
        }

        public void Draw()
        {
            for (int i = 0; i < layers.Length; i++)
            {
                if (layers[i] != null)
                {
                    layers[i].Draw();
                }
            }
        }

        public void Destroy()
        {
            Map.Destroy();
            mapNode = null;

            tileset = null;

            for (int i = 0; i < layers.Length; i++)
            {
                layers[i].Destroy();
            }
        }

    }

}
