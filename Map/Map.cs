﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HNS
{
    class Map
    {
        GroundTypes[,] gt;
        public GroundTypes this[Vector2 pos]
        {
            get
            {
                float x = pos.X / StaticClass.mapScale;
                float y = pos.Y / StaticClass.mapScale;
                if (x >= StaticClass.WIDTH || x <= 0)
                {
                    return GroundTypes.Obstacle;
                }
                if (y >= StaticClass.HEIGHT || y <= 0)
                {
                    return GroundTypes.Obstacle;
                }
                return gt[(int)x, (int)y];
            }


        }

        public Map(Texture2D map1)
        {
            Color[] c = new Color[map1.Width * map1.Height];
            gt = new GroundTypes[map1.Width, map1.Height];
            map1.GetData<Color>(c);
            for (int y = 0; y < map1.Height; y++)
            {
                for (int x = 0; x < map1.Width; x++)
                {
                    if (c[y * map1.Width + x] == c[0])
                    {
                        gt[x, y] = GroundTypes.Obstacle;
                    }
                    else if (c[y * map1.Width + x] != c[0])
                    {
                        gt[x, y] = GroundTypes.Way;
                    }
                }
            }
        }

    }
}
