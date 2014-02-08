Spelunky Tile Generation
==============

The purpose of this program is the creation of graphical tiles based on segmenting a 2D boolean array into chunks.

I created this to try and emulate the style of the terrain generation in a game called Spelunky. After studying how their terrain worked I ended up with this. I may add more to this in the future.

An example of the game Spelunky, and the tiles I'm attempting to recreate, is below:

![Gameplay](/wiki-images/spelunky_example.jpg)

#How it works

Their tilesets look like this (an single area excerpt from a much larger atlas):

![Tileset](/wiki-images/spelunky_tiles.png)

I've highlighted the chunks of 2x2, 2x1, 1x2, and 1x1.

I broke their generation down into two processes:
* Segmenting into chunks listed above
* A border detail pass

The chunks are immutable. When the chunks are generated they do not change (breaking a block simply removes that block, the surrounding tiles are not changed graphically.

The borders are mutable. The borders know which tile they are connected to, and if that tile is destroyed, the border is also destroyed. New borders are also created around exposed tiles. More detail provided under the borders generation.

## Segmenting into chunks

Process:
 * Segment a boolean array of tiles into rectangles
 * Merge those rectangles where applicable, with a random bias for variety
 * Map the rectangles to the texture atlas, picking a random index for the rectangle size

![ShufflingTiles](/wiki-images/tiles_shuffling.gif)

This is what it looks like with a debugging overlay:

![DebugOverlay](/wiki-images/tiles_overlay.png)

## Border detail pass

Borders are simply decals rendered on a foreground layer that don't affect anything. I implemented them as a LinkedList so when you need to unlink them for destroying, there's not a performance hit from reshuffling a list.

Process:
 * Loops over each tile
 * Checks if the tile exists
 * Check if any tile is empty surrounding the tile
 * If it is, add corresponding border decal there

With this method, when a block is destroyed (currently unimplemented) this requires flushing of borders and reprocessing of all border tiles, which also changes existing border graphics from re-randomisation. This could be prevented by keeping reference to all tiles with borders on them.

This process could be optimised in the future by a propigation method:
 * When a block is destroyed:
    * Dispose of the block itself from the tile grid to avoid conflicts when generating borders
    * Propigate a message to all connected borders to notify them to be disposed
    * Notify the border generation to generate borders on all currently connected blocks

After this pass the output looks similar to this:

![BorderPassDirt](/wiki-images/tiles_with_borders_1.png)

This is what the debug overlay looks like:

![BorderPassDirtDebug](/wiki-images/tiles_with_borders_overlay.png)

This method is flexible and can easily be applied to multiple tilesets. This is an example output with an ice tileset:

![BorderPassIce](/wiki-images/tiles_with_borders_2.png)

## The testbed

The main runnable portion of the solution is a testbed for use in testing. It currently runs realtime with the user clicking the mouse buttons to add (left mouse) or remove (right mouse) blocks, and the entire tile grid is recalculated on any change.

Middle mouse also toggles debug overlay.

This runs smoothly realtime, but in a real situation this would only be run once on level load, with the borders updating periodically as a block gets destroyed (the propigation method outlined above could be used to optimise further).

Possible changes are converting the testbed into a scenario where you place down blocks and hit a button to "build the map" which involves generating tiles and placing borders. This could be used to demonstrate destroying blocks and regenerating borders (with the current setup this is unable to be tested).
