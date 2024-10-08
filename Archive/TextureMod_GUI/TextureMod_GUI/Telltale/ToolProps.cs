using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3DTX_TextureConverter.Telltale
{
    public class ToolProps
    {
        public bool mbHasProps;

        public override string ToString()
        {
            return string.Format("[ToolProps] mbHasProps: {0}", mbHasProps);
        }
    }
}
