using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3DTX_TextureConverter.Telltale
{
    public struct T3SamplerStateBlock
    {
        public uint mData;

        public override string ToString()
        {
            return string.Format("[T3SamplerStateBlock] mData: {0}", mData);
        }
    }

    public struct T3SamplerStateBlock_SamplerStateEntry
    {
        public int mShift;
        public int mMask;

        public override string ToString()
        {
            return string.Format("[T3SamplerStateBlock::SamplerStateEntry] mShift: {0} mMask: {1}", mShift, mMask);
        }
    }

    public static class T3SamplerStateBlock_Functions
    {
        /*
void __fastcall T3SamplerStateBlock::SetStateMask(T3SamplerStateBlock *this, unsigned int state)
{
  this->mData |= dword_14102C98C[2 * state];
}

T3SamplerStateBlock *__fastcall T3SamplerStateBlock::Merge(T3SamplerStateBlock *this, T3SamplerStateBlock *result, T3SamplerStateBlock *rhs, T3SamplerStateBlock *mask)
{
  T3SamplerStateBlock *v4; // rax@1

  v4 = result;
  result->mData = mask->mData & rhs->mData | this->mData & ~mask->mData;
  return v4;
}

__int64 __fastcall T3SamplerStateBlock::InternalGetSamplerState(T3SamplerStateBlock *this, unsigned int state)
{
  return (this->mData & HIDWORD((&T3SamplerStateBlock::smEntries)[state])) >> LODWORD((&T3SamplerStateBlock::smEntries)[state]);
}

void __fastcall T3SamplerStateBlock::InternalSetSamplerState(T3SamplerStateBlock *this, unsigned int state, unsigned int value)
{
  T3SamplerStateBlock *v3; // r9@1
  struct T3SamplerStateBlock::SamplerStateEntry near **v4; // rcx@1

  v3 = this;
  v4 = &(&T3SamplerStateBlock::smEntries)[state];
  v3->mData &= ~*(v4 + 1);
  v3->mData |= value << *v4;
}

__int64 __fastcall T3SamplerStateBlock::DecrementMipBias(T3SamplerStateBlock *this, unsigned int steps)
{
  float v2; // xmm0_4@1
  unsigned int v3; // eax@1
  __int64 result; // rax@3
  unsigned int v5; // er8@3

  v2 = FLOAT_8_0;
  v3 = steps + ((dword_14102C9B4 & this->mData) >> dword_14102C9B0);
  if ( v3 < 8.0 )
    v2 = v3;
  result = ffloor(v2);
  v5 = this->mData & ~dword_14102C9B4;
  this->mData = v5;
  this->mData = v5 | (result << dword_14102C9B0);
  return result;
}

void T3SamplerStateBlock::Initialize(void)
{
  LODWORD(T3SamplerStateBlock::smEntries) = 0;
  dword_14102C98C[0] = 15;
  dword_14102C990 = 4;
  dword_14102C994 = 240;
  dword_14102C998 = 8;
  dword_14102C99C = 256;
  dword_14102C9A0 = 9;
  T3SamplerStateBlock::kDefault.mData = (((T3SamplerStateBlock::kDefault.mData & 0xFFFFFFF0 | 1) & 0xFFFFFF0F | 0x10) & 0xFFFFFEFF | 0x100) & 0xFFC001FF;
  dword_14102C9A4 = 7680;
  dword_14102C9A8 = 13;
  dword_14102C9AC = 0x2000;
  dword_14102C9B0 = 14;
  dword_14102C9B4 = 4177920;
}

        */
    }
}
