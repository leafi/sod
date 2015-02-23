using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sod.OpenGL
{
    // Handles management & rendering of a big ol' layer of 2D tiles.
    // This is designed for fast rendering and (at 2x memory cost) fast updating.

    // need shader inputs:
    // - MV matrix, P matrix (note: deffers special ortho P matrix!)
    // - tiledata uniform buffer object (index in atlas, some properties, custom properties)
    // - sprite atlas (either UBO or + info on size of atlas texture)
    // - rect u,v (to calculate coords)
    // - size of 1 tile (so we know when to advance to next tile based on u,v)
    // - tint MUL (including alpha!), tint ADD

    // (likely to have 2 possible copies of sprite atlas, so we can update without stalling.
    //  especially if atlas is large. it doesn't have to be this way, though!
    //  we can just have a new render pass with new mostly  zero UBO to have a go
    //  at the same space but with a different atlas ting.)

    // actually.. https://www.opengl.org/wiki/Buffer_Object#Persistent_mapping & 'Write Synchronization' section
    // Note: 'However, you should not use the pointer you are given like any other pointer you might have.
    //        If this pointer is a pointer to non-standard memory (uncached or video memory), then writing to it 
    //        haphazardly can be problematic. If you are attempting to stream data to the buffer, you should always 
    //        map the buffer only for writing and you should write sequentially. You do not need to write every byte,
    //        but you should avoid going backwards or skipping around in the memory.'

    public class TileLayer
    {
    }
}
