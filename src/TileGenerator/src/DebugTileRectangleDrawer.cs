using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileGenerator
{
    /// <summary>
    /// Draws debug rectangles when provided with a list of TileRectangles
    /// </summary>
    public class DebugTileRectangleDrawer
    {
        static Color Rect2x2Color = Color.Red;
        static Color Rect2x1Color = Color.Green;
        static Color Rect1x2Color = Color.YellowGreen;
        static Color Rect1x1Color = Color.White;

        /// <summary>
        /// Hold a cache of the TileRectangles for drawing
        /// </summary>
        List<TileRectangleCombiner.TileRectangle> tileRectangles = null;

        int tileSize;

        /// <summary>
        /// Initialises a new TileRectangle drawer
        /// </summary>
        public DebugTileRectangleDrawer(int tileSize)
        {
            this.tileSize = tileSize;
        }

        /// <summary>
        /// Set the TileRectangles to be drawn as debug
        /// </summary>
        /// <param name="newTiles">The TileRectangles to be drawn</param>
        public void SetTileRectangles(List<TileRectangleCombiner.TileRectangle> newTiles)
        {
            tileRectangles = newTiles;
        }

        /// <summary>
        /// Draw the TileRectangles as a debug overlay
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="opacity">Opacity to draw overlay at</param>
        public void Draw(SpriteBatch spriteBatch, float opacity = 0.3f)
        {
            //Don't draw if no data
            if (tileRectangles == null) return;

            //Loop over each TileRectangle to draw
            foreach (TileRectangleCombiner.TileRectangle tileRectangle in tileRectangles)
            {
                Color c; //Holds the color to draw the rectangle
                
                //2x2 is Red
                if (tileRectangle.W == 2 && tileRectangle.H == 2)
                {
                    c = Rect2x2Color;
                }

                //2x1 is Green
                else if (tileRectangle.W == 2 && tileRectangle.H == 1)
                {
                    c = Rect2x1Color;
                }

                //1x2 is YellowGreen
                else if (tileRectangle.W == 1 && tileRectangle.H == 2)
                {
                    c = Rect1x2Color;
                }

                //1x1 is White
                else
                {
                    c = Rect1x1Color;
                }

                //Make the color transparent via opacity (0.5 opacity is 50% transparency)
                c *= opacity;

                //Build rectangle from tile size
                Rectangle rectToDraw = new Rectangle();
                rectToDraw.X      = tileRectangle.X * tileSize;
                rectToDraw.Y      = tileRectangle.Y * tileSize;
                rectToDraw.Width  = tileRectangle.W * tileSize;
                rectToDraw.Height = tileRectangle.H * tileSize;

                //Draw it
                Utility.PrimitiveRenderer.DrawRectangle(spriteBatch, rectToDraw, c);
            }
        }
    }
}
