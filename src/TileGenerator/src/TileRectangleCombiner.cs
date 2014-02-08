using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TileGenerator
{
    /// <summary>
    /// Combines Tile Rectangles into 2x2, 2x1, 1x2, and 1x1 rectangles
    /// </summary>
    public class TileRectangleCombiner
    {
        private static Random rand;

        private static float ChanceOf2x2 = 0.2f; //20% chance of 2x2
        private static float ChanceOfOther = 0.5f; //50% chance of trying to make a 2x1 or 1x2

        /// <summary>
        /// A tile represented as a rectangle
        /// </summary>
        public struct TileRectangle
        {
            public int X, Y; //In tile coords
            public int W, H; //In tile coords (can only be 1x1, 2x1, 1x2, or 2x2 currently)

            public TileRectangle(int aX, int aY, int aW = 1, int aH = 1)
            {
                X = aX;
                Y = aY;
                W = aW;
                H = aH;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tileManager"></param>
        /// <returns></returns>
        public static List<TileRectangle> Combine(TileManager tileManager)
        {
            /* Process:
            * 1) Create all 1x1 tiles based on grid
            * 2) Find 2x2 tiles, merge them
            * 3) Find 1x2 and 2x1 tiles, merge them
            */

            rand = new Random();

            //Initialise lists
            List<TileRectangle> tileRectList = new List<TileRectangle>();
            List<Tuple<int, int>> checkedList = new List<Tuple<int, int>>();

            //Attempt to create the chunks in order
              Create2x2Chunks(ref tileRectList, ref checkedList, tileManager);
            CreateOtherChunks(ref tileRectList, ref checkedList, tileManager);
            Generate1x1Chunks(ref tileRectList, ref checkedList, tileManager);

            return tileRectList;
        }

        /// <summary>
        /// Find any 2x2 areas and attempts to merge them into chunks
        /// </summary>
        /// <param name="tileRectList">Reference list of tile rectangles to add to</param>
        /// <param name="checkedList">Reference list of tiles already checked</param>
        /// <param name="tileManager">Tile Manager</param>
        private static void Create2x2Chunks(ref List<TileRectangle> tileRectList, ref List<Tuple<int, int>> checkedList, TileManager tileManager)
        {
            //Find 2x2 tiles, create them

            /* Process:
             * 1) Loop through all the tiles
             * 2) Check if area around them fits into a 2x2 square
             * 3) If so, create the 2x2 rectangle, add them to checked list
             */

            //Loop through each tile
            for (int y = 0; y < tileManager.Height; ++y)
            {
                for (int x = 0; x < tileManager.Width; ++x)
                {
                    //50% chance to not make anything
                    if (rand.NextDouble() > ChanceOf2x2)
                        continue;

                    if (CheckFor2x2(x, y, ref checkedList, tileManager))
                    {
                        Create2x2(x, y, ref tileRectList, ref checkedList);
                    }
                }
            }
        }

        /// <summary>
        /// Find any 2x1 or 1x2 areas and attempts to merge them into chunks
        /// </summary>
        /// <param name="tileRectList">Reference list of tile rectangles to add to</param>
        /// <param name="checkedList">Reference list of tiles already checked</param>
        /// <param name="tileManager">Tile Manager</param>
        private static void CreateOtherChunks(ref List<TileRectangle> tileRectList, ref List<Tuple<int, int>> checkedList, TileManager tileManager)
        {
            //2) Find 1x2 and 2x1 tiles, create them

            //Loop through each tile
            for (int y = 0; y < tileManager.Height; ++y)
            {
                for (int x = 0; x < tileManager.Width; ++x)
                {
                    //50% chance to not make anything
                    if (rand.NextDouble() > ChanceOfOther)
                        continue;

                    //Random chance to make 1x2 or 2x1 first
                    if (rand.NextDouble() >= 0.5)
                    {
                        //Try to create a 1x2
                        if (CheckFor1x2(x, y, ref checkedList, tileManager))
                        {
                            Create1x2(x, y, ref tileRectList, ref checkedList);
                        }

                        //Try to create a 2x1
                        if (CheckFor2x1(x, y, ref checkedList, tileManager))
                        {
                            Create2x1(x, y, ref tileRectList, ref checkedList);
                        }
                    }
                    else //Make it in the opposite order
                    {
                        //Try to create a 2x1
                        if (CheckFor2x1(x, y, ref checkedList, tileManager))
                        {
                            Create2x1(x, y, ref tileRectList, ref checkedList);
                        }

                        //Try to create a 1x2
                        if (CheckFor1x2(x, y, ref checkedList, tileManager))
                        {
                            Create1x2(x, y, ref tileRectList, ref checkedList);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Generate all remaining 1x1 areas
        /// </summary>
        /// <param name="tileRectList">Reference list of tile rectangles to add to</param>
        /// <param name="checkedList">Reference list of tiles already checked</param>
        /// <param name="tileManager">Tile Manager</param>
        private static void Generate1x1Chunks(ref List<TileRectangle> tileRectList, ref List<Tuple<int, int>> checkedList, TileManager tileManager)
        {
            //3) Create the remaining 1x1 Chunks

            //Loop through each tile
            for (int y = 0; y < tileManager.Height; ++y)
            {
                for (int x = 0; x < tileManager.Width; ++x)
                {
                    //Check if tile solid and 2x2 area isn't in the checked list (may be buggy)
                    if (tileManager.GetTile(x, y) && !checkedList.Any(t => t.Item1 == x && t.Item2 == y))
                    {
                        //Create the 1x1 rectangle
                        tileRectList.Add(new TileRectangle(x, y, 1, 1));

                        //Add them to the checked list
                        checkedList.Add(new Tuple<int, int>(x, y));
                    }
                }
            }
        }

        /// <summary>
        /// Check if a 1x2 chunk would fit
        /// </summary>
        /// <param name="x">X position to check (tiles)</param>
        /// <param name="y">Y position to check (tiles)</param>
        /// <param name="checkedList">Reference list of tiles already checked</param>
        /// <param name="tileManager">Tile Manager</param>
        /// <returns>True if chunk fits</returns>
        private static bool CheckFor1x2(int x, int y, ref List<Tuple<int, int>> checkedList, TileManager tileManager)
        {
            //Check if tiles solid and 1x2 area isn't in the checked list

            //Check if current tile okay
            if (!tileManager.GetTile(x, y))
                return false;

            //Check if next tile okay
            if (!tileManager.GetTile(x, y + 1))
                return false;

            //Check if tiles are valid
            if (checkedList.Any(t => t.Item1 == x && (t.Item2 >= y && t.Item2 < y + 2)))
                return false;

            return true;
        }

        /// <summary>
        /// Check if a 2x1 chunk would fit
        /// </summary>
        /// <param name="x">X position to check (tiles)</param>
        /// <param name="y">Y position to check (tiles)</param>
        /// <param name="checkedList">Reference list of tiles already checked</param>
        /// <param name="tileManager">Tile Manager</param>
        /// <returns>True if chunk fits</returns>
        private static bool CheckFor2x1(int x, int y, ref List<Tuple<int, int>> checkedList, TileManager tileManager)
        {
            //Check if tiles solid and 1x2 area isn't in the checked list

            //Check if current tile okay
            if (!tileManager.GetTile(x, y))
                return false;

            //Check if next tile okay
            if (!tileManager.GetTile(x + 1, y))
                return false;

            //Check if tiles are valid
            if (checkedList.Any(t => (t.Item1 >= x && t.Item1 < x + 2) && t.Item2 == y))
                return false;

            return true;
        }

        /// <summary>
        /// Check if a 2x2 chunk would fit
        /// </summary>
        /// <param name="x">X position to check (tiles)</param>
        /// <param name="y">Y position to check (tiles)</param>
        /// <param name="checkedList">Reference list of tiles already checked</param>
        /// <param name="tileManager">Tile Manager</param>
        /// <returns>True if chunk fits</returns>
        private static bool CheckFor2x2(int x, int y, ref List<Tuple<int, int>> checkedList, TileManager tileManager)
        {
            //Check if tiles solid and 1x2 area isn't in the checked list

            //Check if current tile okay
            if (!tileManager.GetTile(x, y))
                return false;

            //Check if other tiles okay
            if (!tileManager.GetTile(x + 1, y))
                return false;
            if (!tileManager.GetTile(x, y + 1))
                return false;
            if (!tileManager.GetTile(x + 1, y + 1))
                return false;

            //Check if tiles are valid
            if (checkedList.Any(t => (t.Item1 >= x && t.Item1 < x + 2) && (t.Item2 >= y && t.Item2 < y + 2)))
                return false;

            return true;
        }

        /// <summary>
        /// Create a 1x2 chunk in a location
        /// </summary>
        /// <param name="x">X position to check (tiles)</param>
        /// <param name="y">Y position to check (tiles)</param>
        /// <param name="tileRectList">Reference list of tile rectangles to add to</param>
        /// <param name="checkedList">Reference list of tiles already checked</param>
        private static void Create1x2(int x, int y, ref List<TileRectangle> tileRectList, ref List<Tuple<int, int>> checkedList)
        {
            //Create the 1x2 rectangle
            tileRectList.Add(new TileRectangle(x, y, 1, 2));

            //Add them to the checked list
            checkedList.Add(new Tuple<int, int>(x, y));
            checkedList.Add(new Tuple<int, int>(x, y + 1));
        }

        /// <summary>
        /// Create a 2x1 chunk in a location
        /// </summary>
        /// <param name="x">X position to check (tiles)</param>
        /// <param name="y">Y position to check (tiles)</param>
        /// <param name="tileRectList">Reference list of tile rectangles to add to</param>
        /// <param name="checkedList">Reference list of tiles already checked</param>
        private static void Create2x1(int x, int y, ref List<TileRectangle> tileRectList, ref List<Tuple<int, int>> checkedList)
        {
            //Create the 2x1 rectangle
            tileRectList.Add(new TileRectangle(x, y, 2, 1));

            //Add them to the checked list
            checkedList.Add(new Tuple<int, int>(x, y));
            checkedList.Add(new Tuple<int, int>(x + 1, y));
        }

        /// <summary>
        /// Create a 2x2 chunk in a location
        /// </summary>
        /// <param name="x">X position to check (tiles)</param>
        /// <param name="y">Y position to check (tiles)</param>
        /// <param name="tileRectList">Reference list of tile rectangles to add to</param>
        /// <param name="checkedList">Reference list of tiles already checked</param>
        private static void Create2x2(int x, int y, ref List<TileRectangle> tileRectList, ref List<Tuple<int, int>> checkedList)
        {
            //Create the 2x2 rectangle
            tileRectList.Add(new TileRectangle(x, y, 2, 2));

            //Add them to the checked list
            checkedList.Add(new Tuple<int, int>(x, y));
            checkedList.Add(new Tuple<int, int>(x + 1, y));
            checkedList.Add(new Tuple<int, int>(x, y + 1));
            checkedList.Add(new Tuple<int, int>(x + 1, y + 1));
        }
    }
}
