using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TileGenerator
{
    /// <summary>
    /// Manages and draws edge decals
    /// </summary>
    public class EdgeDecalManager
    {
        enum BlockSide
        {
            LEFT,
            RIGHT,
            TOP,
            BOTTOM
        }

        /// <summary>
        /// Holds data for an edge decal
        /// </summary>
        struct EdgeDecal
        {
            //Source on texture
            public Rectangle sourceRect { get; private set; }

            //Local to draw from
            public Vector2 origin { get; private set; }

            //Position to draw at
            public Vector2 position { get; private set; }
            
            //Index of attatched block
            public int attatchedBlockIndex { get; private set; }

            public EdgeDecal(Rectangle sr, Vector2 ori, Vector2 pos, int abi) : this()
            {
                sourceRect = sr;
                origin = ori;
                position = pos;
                attatchedBlockIndex = abi;
            }
        }

        //Used to hold and draw the decals
        LinkedList<EdgeDecal> decalLList;

        //Texture to draw the decals with
        Texture2D decalTexture;

        //A 2D array to hold source rectangles (seems kinda hacky but it's the best I've got for now)
        Rectangle[][] sourceRects;
        Vector2[] origins;

        /// <summary>
        /// Initialises a new decal manager
        /// </summary>
        /// <param name="tileSize">Size of each tile on the texture (pixels)</param>
        public EdgeDecalManager(int tileSize)
        {
            decalLList = new LinkedList<EdgeDecal>();

            /*
             * Generate a 2D array of source rectangles:
             *   The first dimension being direction
             *   The second a list of source rectangles to pick from
             */
            sourceRects = new Rectangle[4][];

            //Left (1 choice)
            sourceRects[(ushort)BlockSide.LEFT] = new Rectangle[1];
            //At (8, 3) on texture grid
            sourceRects[(ushort)BlockSide.LEFT][0] = new Rectangle(7 * tileSize, 2 * tileSize, tileSize, tileSize);

            //Right (1 choice)
            sourceRects[(ushort)BlockSide.RIGHT] = new Rectangle[1];
            //At (8, 1) on texture grid
            sourceRects[(ushort)BlockSide.RIGHT][0] = new Rectangle(7 * tileSize, tileSize, tileSize, tileSize);

            //Top (3 choices)
            sourceRects[(ushort)BlockSide.TOP] = new Rectangle[3];
            //At (5-7, 0) on texture grid
            for (int i = 0; i < 3; ++i)
            {
                sourceRects[(ushort)BlockSide.TOP][i] = new Rectangle((5 + i) * tileSize, 0, tileSize, tileSize);
            }

            //Bottom (2 choices)
            sourceRects[(ushort)BlockSide.BOTTOM] = new Rectangle[2];
            //At (5-6, 1) on texture grid
            for (int i = 0; i < 2; ++i)
            {
                sourceRects[(ushort)BlockSide.BOTTOM][i] = new Rectangle((5 + i) * tileSize, tileSize, tileSize, tileSize);
            }

            //Create origins
            origins = new Vector2[4];
            origins[(ushort)BlockSide.LEFT]   = new Vector2(tileSize / 2, 0);
            origins[(ushort)BlockSide.RIGHT]  = new Vector2(tileSize / 2, 0);
            origins[(ushort)BlockSide.TOP]    = new Vector2(0, tileSize / 2.5f); //~0.4 of tilesize
            origins[(ushort)BlockSide.BOTTOM] = new Vector2(0, tileSize / (1 + (2f / 3f))); //~1.666 of tilesize
        }

        /// <summary>
        /// Sets the texture to draw the decal with
        /// </summary>
        /// <param name="texture">Texture to use</param>
        public void SetTexture(Texture2D texture)
        {
            this.decalTexture = texture;
        }

        /// <summary>
        /// Create edge decals around blocks
        /// </summary>
        /// <param name="blocks">A 2D grid signifying blocks</param>
        /// <param name="width">Width of the grid (tiles)</param>
        /// <param name="height">Height of the grid (tiles)</param>
        /// <param name="tileSize">Size of each tile in the grid (pixels)</param>
        public void CreateEdgeDecals(bool[,] blocks, int width, int height, int tileSize)
        {
            //Clear the old decal list
            decalLList.Clear();

            //Generate a new random for use
            Random r = new Random();

            //Loop over all the tiles
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    /* Process:
                     * 1) Check if tile exists
                     * 2) check if block empty on any side of it
                     * 3) If so, create the appropriate decal there
                     */

                    //If tile doesn't exist, don't process it
                    if (!blocks[x, y]) continue;

                    int blockIndex = x + y * width;

                    //Create decals on the left
                    if (x != 0 && !blocks[x - 1, y])
                    {
                        //Grab the source rectangle from the array
                        Rectangle sourceRect = sourceRects[(ushort)BlockSide.LEFT][0];

                        Vector2 origin = origins[(ushort)BlockSide.LEFT];

                        //Create the left decal at topleft of block
                        Vector2 position = new Vector2(x * tileSize, y * tileSize);

                        decalLList.AddLast(new EdgeDecal(sourceRect, origin, position, blockIndex));
                    }

                    //Create decals on the right
                    if (x != width - 1 && !blocks[x + 1, y])
                    {
                        //Grab the source rectangle from the array
                        Rectangle sourceRect = sourceRects[(ushort)BlockSide.RIGHT][0];

                        Vector2 origin = origins[(ushort)BlockSide.RIGHT];

                        //Create the left decal at topright of block
                        Vector2 position = new Vector2((x + 1) * tileSize, y * tileSize);

                        decalLList.AddLast(new EdgeDecal(sourceRect, origin, position, blockIndex));
                    }

                    //Create decals on the top
                    if (y != 0 && !blocks[x, y - 1])
                    {
                        //Grab a random source rectangle from the array
                        int randIndex = r.Next(sourceRects[(ushort)BlockSide.TOP].Length);
                        Rectangle sourceRect = sourceRects[(ushort)BlockSide.TOP][randIndex];

                        Vector2 origin = origins[(ushort)BlockSide.TOP];

                        //Create the left decal at topleft of block
                        Vector2 position = new Vector2(x * tileSize, y * tileSize);

                        decalLList.AddLast(new EdgeDecal(sourceRect, origin, position, blockIndex));
                    }

                    //Create decals on the bottom
                    if (y != height - 1 && !blocks[x, y + 1])
                    {
                        //Grab a random source rectangle from the array
                        int randIndex = r.Next(sourceRects[(ushort)BlockSide.BOTTOM].Length);
                        Rectangle sourceRect = sourceRects[(ushort)BlockSide.BOTTOM][randIndex];

                        Vector2 origin = origins[(ushort)BlockSide.BOTTOM];

                        //Create the left decal at bottomleft of block
                        Vector2 position = new Vector2(x * tileSize, (y + 1) * tileSize);

                        decalLList.AddLast(new EdgeDecal(sourceRect, origin, position, blockIndex));
                    }
                }
            }
        }

        /// <summary>
        /// Call when a block is destroyed
        /// </summary>
        /// <param name="blockIndex">Index of the destroyed block</param>
        public void BlockDestroyed(int blockIndex)
        {
            //Untested, might not work, might have to iterate over nodes and unlink or something
            foreach (EdgeDecal decal in decalLList)
            {
                //Check if the decal is attatched to the block index and remove it
                if (decal.attatchedBlockIndex == blockIndex)
                {
                    decalLList.Remove(decal);
                }
            }
        }

        /// <summary>
        /// Call when a block is destroyed
        /// </summary>
        /// <param name="blockX">X position of the destroyed block (tiles)</param>
        /// <param name="blockY">Y position of the destroyed block (tiles)</param>
        /// <param name="arrayWidth">Width of tile grid (tiles)</param>
        public void BlockDestroyed(int blockX, int blockY, int arrayWidth)
        {
            //Convert grid position (2D) into a block index (1D)
            BlockDestroyed(blockX + blockY * arrayWidth);
        }

        /// <summary>
        /// Draw edge decals
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (EdgeDecal decal in decalLList)
            {
                spriteBatch.Draw(decalTexture, decal.position - decal.origin, decal.sourceRect, Color.White);
            }
        }
    }
}
