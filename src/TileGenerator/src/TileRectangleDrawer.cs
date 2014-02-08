using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TileGenerator
{
    /// <summary>
    /// Draws tile rectangles
    /// </summary>
    public class TileRectangleDrawer
    {
        /// <summary>
        /// List of tile rectangles to draw
        /// </summary>
        List<TileRectangleCombiner.TileRectangle> tileRectangles;

        /// <summary>
        /// A list of the source rectangles tied to the tile rectangles list (index-matched)
        /// </summary>
        List<Rectangle> sourceRectList;

        /// <summary>
        /// Tileset texture to draw with
        /// </summary>
        Texture2D tileTexture;

        int tileSize;

        /// <summary>
        /// Initialises a new TileRectangle drawer
        /// </summary>
        public TileRectangleDrawer(int tileSize)
        {
            this.tileSize = tileSize;
        }

        /// <summary>
        /// Sets the texture to draw the decal with
        /// </summary>
        /// <param name="texture">Texture to use</param>
        public void SetTexture(Texture2D texture)
        {
            tileTexture = texture;
        }

        /// <summary>
        /// Pick a random source rectangle for a 2x2 tile
        /// </summary>
        /// <param name="rand">The random generator to use</param>
        /// <returns>A source rectangle for a 2x2 tile</returns>
        private Rectangle Get2x2SourceRect(Random rand)
        {
            Rectangle[] sourceRects = new Rectangle[4];

            sourceRects[0] = new Rectangle(0,            tileSize * 4, tileSize * 2, tileSize * 2);
            sourceRects[1] = new Rectangle(tileSize * 2, tileSize * 4, tileSize * 2, tileSize * 2);
            sourceRects[2] = new Rectangle(0,            tileSize * 6, tileSize * 2, tileSize * 2);
            sourceRects[3] = new Rectangle(tileSize * 2, tileSize * 6, tileSize * 2, tileSize * 2);

            int randomIndex = rand.Next(4);

            return sourceRects[randomIndex];
        }

        /// <summary>
        /// Pick a random source rectangle for a 2x1 tile
        /// </summary>
        /// <param name="rand">The random generator to use</param>
        /// <returns>A source rectangle for a 2x1 tile</returns>
        private Rectangle Get2x1SourceRect(Random rand)
        {
            Rectangle[] sourceRects = new Rectangle[2];

            sourceRects[0] = new Rectangle(0,            tileSize * 3, tileSize * 2, tileSize);
            sourceRects[1] = new Rectangle(tileSize * 2, tileSize * 3, tileSize * 2, tileSize);

            int randomIndex = rand.Next(2);

            return sourceRects[randomIndex];
        }

        /// <summary>
        /// Pick a random source rectangle for a 1x2 tile
        /// </summary>
        /// <param name="rand">The random generator to use</param>
        /// <returns>A source rectangle for a 1x2 tile</returns>
        private Rectangle Get1x2SourceRect(Random rand)
        {
            Rectangle[] sourceRects = new Rectangle[2];

            sourceRects[0] = new Rectangle(tileSize * 2, tileSize, tileSize, tileSize * 2);
            sourceRects[1] = new Rectangle(tileSize * 3, tileSize, tileSize, tileSize * 2);

            int randomIndex = rand.Next(2);

            return sourceRects[randomIndex];
        }

        /// <summary>
        /// Pick a random source rectangle for a 1x1 tile
        /// </summary>
        /// <param name="rand">The random generator to use</param>
        /// <returns>A source rectangle for a 1x1 tile</returns>
        private Rectangle Get1x1SourceRect(Random rand)
        {
            Rectangle[] sourceRects = new Rectangle[4];

            sourceRects[0] = new Rectangle(0,        tileSize,     tileSize, tileSize);
            sourceRects[1] = new Rectangle(tileSize, tileSize,     tileSize, tileSize);
            sourceRects[2] = new Rectangle(0,        tileSize * 2, tileSize, tileSize);
            sourceRects[3] = new Rectangle(tileSize, tileSize * 2, tileSize, tileSize);

            int randomIndex = rand.Next(4);

            return sourceRects[randomIndex];
        }

        /// <summary>
        /// Set the TileRectangles to draw
        /// </summary>
        /// <param name="newTiles">A list of TileRectangles to draw</param>
        public void SetTileRectangles(List<TileRectangleCombiner.TileRectangle> newTiles)
        {
            //Set the internal list and recreate the source rectangle list to match
            tileRectangles = newTiles;
            sourceRectList = new List<Rectangle>();

            Random rand = new Random();

            //Loop over each TileRectangle input and work out which source type to generate
            //Also add the source rectangle generated to the source rect list
            foreach (TileRectangleCombiner.TileRectangle tileRectangle in tileRectangles)
            {
                //2x2
                if (tileRectangle.W == 2 && tileRectangle.H == 2)
                {
                    sourceRectList.Add(Get2x2SourceRect(rand));
                }

                //2x1
                else if (tileRectangle.W == 2 && tileRectangle.H == 1)
                {
                    sourceRectList.Add(Get2x1SourceRect(rand));
                }

                //1x2
                else if (tileRectangle.W == 1 && tileRectangle.H == 2)
                {
                    sourceRectList.Add(Get1x2SourceRect(rand));
                }

                //1x1
                else
                {
                    sourceRectList.Add(Get1x1SourceRect(rand));
                }
            }
        }

        /// <summary>
        /// Draw the tile rectangles
        /// </summary>
        /// <param name="spriteBatch">Spritebatch to draw on</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            //Don't draw if no data
            if (tileRectangles == null) return;

            //Don't draw if no texture
            if (tileTexture == null) return;

            //Loop through each tile rectangle and draw with corresponding source rect
            for (int i = 0; i < tileRectangles.Count; ++i)
            {
                TileRectangleCombiner.TileRectangle tileRectangle = tileRectangles[i];

                Rectangle destRect = new Rectangle();
                destRect.X      = tileRectangle.X * tileSize;
                destRect.Y      = tileRectangle.Y * tileSize;
                destRect.Width  = tileRectangle.W * tileSize;
                destRect.Height = tileRectangle.H * tileSize;

                spriteBatch.Draw(tileTexture, destRect, sourceRectList[i], Color.White);
            }
        }
    }
}
