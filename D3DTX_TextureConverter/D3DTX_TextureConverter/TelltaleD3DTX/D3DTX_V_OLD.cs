using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using D3DTX_TextureConverter.TelltaleFunctions;
using D3DTX_TextureConverter.TelltaleEnums;
using D3DTX_TextureConverter.TelltaleTypes;
using D3DTX_TextureConverter.Utilities;
using D3DTX_TextureConverter.DirectX;
using D3DTX_TextureConverter.Main;

/*
 * NOTE:
 * 
 * This version of D3DTX is INCOMPLETE. 
 * 
 * INCOMPLETE meaning that all of the data isn't known and getting identified correctly. We still parse, identify, and modify if needed but it's just named as "Unknown".
 * The reason being we don't have "full knowledge" of it, given that games that shipped with this version haven't shipped with a PDB. 
 * So the only source is looking through the strings in the game exe through a hex editor to identify what variables might be in the file.
 * This D3DTX version derives from version 9 and has been 'stripped' or adjusted to suit this version of D3DTX.
 * Also, Telltale uses Hungarian Notation for variable naming.
*/

/* - D3DTX Old Version games
 * Telltale Texas Hold'em  (UNTESTED)
 * Bone: Out from Boneville  (UNTESTED)
 * CSI: 3 Dimensions of Murder  (UNTESTED)
 * Bone: The Great Cow Race  (UNTESTED)
 * Sam & Max Save the World  (UNTESTED)
 * CSI: Hard Evidence  (UNTESTED)
 * Sam & Max Beyond Time and Space  (UNTESTED)
 * Strong Bad's Cool Game for Attractive People  (UNTESTED)
 * Wallace & Gromit's Grand Adventures  (UNTESTED)
 * Tales of Monkey Island  (UNTESTED)
 * CSI: Deadly Intent  (UNTESTED)
 * Sam & Max: The Devil's Playhouse  (UNTESTED)
 * Nelson Tethers: Puzzle Agent  (UNTESTED)
 * CSI: Fatal Conspiracy  (UNTESTED)
 * Poker Night at the Inventory  (UNTESTED)
 * Back to the Future: The Game  (UNTESTED)
 * Puzzle Agent 2 (UNTESTED)
 * Jurassic Park: The Game  (UNTESTED)
 * Law & Order: Legacies  (UNTESTED)
 * The Walking Dead Season One (Original) (TESTED)
 * Poker Night 2 (Original) (UNTESTED)
*/

namespace D3DTX_TextureConverter.TelltaleD3DTX
{
    /// <summary>
    /// This is a custom class that matches what is serialized in a D3DTX (versions older than 4). (INCOMPLETE)
    /// </summary>
    public class D3DTX_V_OLD
    {
        /*
         * NOT IMPLEMENTED YET
        */
    }
}
