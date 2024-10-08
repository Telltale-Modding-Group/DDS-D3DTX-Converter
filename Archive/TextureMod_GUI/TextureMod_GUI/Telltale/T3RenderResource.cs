using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3DTX_TextureConverter.Telltale
{
    public class T3RenderResource
    {
        /*
mResourceListLock CriticalLock 4 dup(?)
mResourceList   LinkedListBase<T3RenderResource,0> 4 dup(?)
mDeviceReset    dd ?
                 db ? ; undefined
                 db ? ; undefined
                 db ? ; undefined
                 db ? ; undefined
        */

        /*
void __fastcall T3RenderResource::T3RenderResource(T3RenderResource *this)
{
  T3RenderResource::Manager *v1; // rdx@1

  this->mPrev = 0i64;
  this->mNext = 0i64;
  this->vfptr = &T3RenderResource::`vftable';
  this->mCurrentList = -1;
  *&this->mFlags.mFlags = 0i64;
  v1 = T3RenderResource::smManager;
  this->mFrameUsed.mValue = 0;
  T3RenderResource::_AddToList(this, v1, (v1->mDeviceReset > 0));
}
        */
    }
}
