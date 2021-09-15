using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace D3DTX_TextureConverter.Utilities
{
    public static class ByteFunctions
    {
        /// <summary>
        /// Gets a single byte and parsed the value into an unsigned integer. Increments the byte pointer position by 1.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="bytePointerLocation"></param>
        /// <returns></returns>
        public static bool ReadBool(byte[] bytes, ref uint bytePointerLocation)
        {
            //get the byte at the position in the array
            byte[] raw_bytes = AllocateBytes(1, bytes, bytePointerLocation);

            //convert the byte into a value
            bool parsedValue = BitConverter.ToBoolean(raw_bytes, 0);

            //increment the pointer position
            bytePointerLocation += 1;

            //return the parsed value
            return parsedValue;
        }

        /// <summary>
        /// Gets two bytes and parses the value into an unsigned short. Increments the byte pointer position by 2.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="bytePointerLocation"></param>
        /// <returns></returns>
        public static ushort ReadUnsignedShort(byte[] bytes, ref uint bytePointerLocation)
        {
            //allocate the bytes from the main byte array
            byte[] raw_bytes = AllocateBytes(2, bytes, bytePointerLocation);

            //convert the byte into a value
            ushort parsedValue = BitConverter.ToUInt16(raw_bytes, 0);

            //increment the pointer position
            bytePointerLocation += 2;

            //return the parsed value
            return parsedValue;
        }

        /// <summary>
        /// Gets 8 bytes and parses the value into a long. Increments the byte pointer position by 8.
        /// </summary>
        /// <param name="fileBytes"></param>
        /// <param name="bytePointerLocation"></param>
        /// <returns></returns>
        public static long ReadLong(byte[] bytes, ref uint bytePointerLocation)
        {
            //allocate the bytes from the main byte array
            byte[] raw_bytes = AllocateBytes(8, bytes, bytePointerLocation);

            //convert the bytes into a value
            long parsedValue = BitConverter.ToInt64(raw_bytes, 0);

            //increment the pointer position
            bytePointerLocation += 8;

            //return the parsed value
            return parsedValue;
        }

        /// <summary>
        /// Gets a single byte and parsed the value into an unsigned integer. Increments the byte pointer position by 1.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="bytePointerLocation"></param>
        /// <returns></returns>
        public static uint ReadByte(byte[] bytes, ref uint bytePointerLocation)
        {
            //get the byte at the position in the array
            byte raw_bytes = AllocateByte(bytes, bytePointerLocation);

            //convert the byte into a value
            uint parsedValue = raw_bytes;

            //increment the pointer position
            bytePointerLocation += 1;

            //return the parsed value
            return parsedValue;
        }

        /// <summary>
        /// Gets two bytes and parses the value into a short. Increments the byte pointer position by 2.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="bytePointerLocation"></param>
        /// <returns></returns>
        public static short ReadShort(byte[] bytes, ref uint bytePointerLocation)
        {
            //allocate the bytes from the main byte array
            byte[] raw_bytes = AllocateBytes(2, bytes, bytePointerLocation);

            //convert the byte into a value
            short parsedValue = BitConverter.ToInt16(raw_bytes, 0);

            //increment the pointer position
            bytePointerLocation += 2;

            //return the parsed value
            return parsedValue;
        }

        /// <summary>
        /// Gets two bytes and parses the value into a half float. Increments the byte pointer position by 2.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="bytePointerLocation"></param>
        /// <returns></returns>
        public static float ReadHalfFloat(byte[] bytes, ref uint bytePointerLocation)
        {
            //allocate the bytes from the main byte array
            byte[] raw_bytes = AllocateBytes(2, bytes, bytePointerLocation);

            //convert the byte into a value
            float parsedValue = BitConverter.ToInt16(raw_bytes, 0);

            //increment the pointer position
            bytePointerLocation += 2;

            //return the parsed value
            return parsedValue;
        }

        /// <summary>
        /// Gets 4 bytes and parses the value into an unsigned integer. Increments the byte pointer position by 4.
        /// </summary>
        /// <param name="fileBytes"></param>
        /// <param name="bytePointerLocation"></param>
        /// <returns></returns>
        public static uint ReadUnsignedInt(byte[] bytes, ref uint bytePointerLocation)
        {
            //allocate the bytes from the main byte array
            byte[] raw_bytes = AllocateBytes(4, bytes, bytePointerLocation);

            //convert the bytes into a value
            uint parsedValue = BitConverter.ToUInt32(raw_bytes, 0);

            //increment the pointer position
            bytePointerLocation += 4;

            //return the parsed value
            return parsedValue;
        }

        /// <summary>
        /// Gets 4 bytes and parses the value into an integer. Increments the byte pointer position by 4.
        /// </summary>
        /// <param name="fileBytes"></param>
        /// <param name="bytePointerLocation"></param>
        /// <returns></returns>
        public static int ReadInt(byte[] bytes, ref uint bytePointerLocation)
        {
            //allocate the bytes from the main byte array
            byte[] raw_bytes = AllocateBytes(4, bytes, bytePointerLocation);

            //convert the bytes into a value
            int parsedValue = BitConverter.ToInt32(raw_bytes, 0);

            //increment the pointer position
            bytePointerLocation += 4;

            //return the parsed value
            return parsedValue;
        }

        /// <summary>
        /// Allocates bytes of the length of the string and parses the value into a string. Increments the byte pointer position by the string length.
        /// </summary>
        /// <param name="fileBytes"></param>
        /// <param name="stringLength"></param>
        /// <param name="bytePointerLocation"></param>
        /// <returns></returns>
        public static string ReadFixedString(byte[] bytes, int stringLength, ref uint bytePointerLocation)
        {
            //allocate the bytes from the main byte array
            byte[] raw_bytes = AllocateBytes(stringLength, bytes, bytePointerLocation);

            //convert the bytes into a value
            string parsedString = Encoding.ASCII.GetString(raw_bytes);

            //increment the pointer position
            bytePointerLocation += (uint)stringLength;

            //return the parsed value
            return parsedString;
        }

        /// <summary>
        /// Allocates bytes of the length of the string and parses the value into a string. Increments the byte pointer position by the string length.
        /// </summary>
        /// <param name="fileBytes"></param>
        /// <param name="stringLength"></param>
        /// <param name="bytePointerLocation"></param>
        /// <returns></returns>
        public static string ReadFixedString(byte[] bytes, uint stringLength, ref uint bytePointerLocation)
        {
            return ReadFixedString(bytes, (int)stringLength, ref bytePointerLocation);
        }

        /// <summary>
        /// Gets 4 bytes and parses the value into a float. Increments the byte pointer position by 4.
        /// </summary>
        /// <param name="fileBytes"></param>
        /// <param name="bytePointerLocation"></param>
        /// <returns></returns>
        public static float ReadFloat(byte[] bytes, ref uint bytePointerLocation)
        {
            //allocate the bytes from the main byte array
            byte[] raw_bytes = AllocateBytes(4, bytes, bytePointerLocation);

            //convert the bytes into a value
            float parsedValue = BitConverter.ToSingle(raw_bytes, 0);

            //increment the pointer position
            bytePointerLocation += 4;

            //return the parsed value
            return parsedValue;
        }

        public static byte[] ModifyBytes(byte[] source, byte[] newBytes, uint indexOffset)
        {
            if (source.Length >= indexOffset)
                return source;

            //run a loop and begin going through for the lenght of the bytes
            for (int i = 0; i < newBytes.Length; i++)
            {
                //assign the value from the source byte array with the offset
                source[indexOffset + i] = newBytes[i];
            }

            //return the final byte array
            return source;
        }

        public static byte[] ModifyBytes(byte[] source, int newValue, uint indexOffset)
        {
            byte[] newBytes = BitConverter.GetBytes(newValue);

            return ModifyBytes(source, newBytes, indexOffset);
        }

        /// <summary>
        /// Combines two byte arrays into one.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static byte[] Combine(byte[] first, byte[] second)
        {
            //allocate a byte array with both total lengths combined to accomodate both
            byte[] bytes = new byte[first.Length + second.Length];

            //copy the data from the first array into the new array
            Buffer.BlockCopy(first, 0, bytes, 0, first.Length);

            //copy the data from the second array into the new array (offset by the total length of the first array)
            Buffer.BlockCopy(second, 0, bytes, first.Length, second.Length);

            //return the final byte array
            return bytes;
        }

        /// <summary>
        /// Allocates a byte array of fixed length. 
        /// <para>Depending on 'size' it allocates 'size' amount of bytes from 'sourceByteArray' offset by 'offsetLocation'</para>
        /// </summary>
        /// <param name="size"></param>
        /// <param name="sourceByteArray"></param>
        /// <param name="offsetLocation"></param>
        /// <returns></returns>
        public static byte AllocateByte(byte[] sourceByteArray, uint offsetLocation)
        {
            return sourceByteArray[offsetLocation];
        }

        /// <summary>
        /// Allocates a byte array of fixed length. 
        /// <para>Depending on 'size' it allocates 'size' amount of bytes from 'sourceByteArray' offset by 'offsetLocation'</para>
        /// </summary>
        /// <param name="size"></param>
        /// <param name="sourceByteArray"></param>
        /// <param name="offsetLocation"></param>
        /// <returns></returns>
        public static byte[] AllocateBytes(int size, byte[] sourceByteArray, uint offsetLocation)
        {
            //allocate byte array of fixed length
            byte[] byteArray = new byte[size];

            if (offsetLocation + size > sourceByteArray.Length)
                return byteArray;

            //run a loop and begin gathering values
            for (int i = 0; i < size; i++)
            {
                //assign the value from the source byte array with the offset
                byteArray[i] = sourceByteArray[offsetLocation + i];
            }

            //return the final byte array
            return byteArray;
        }

        public static byte[] AllocateBytes(byte[] bytes, byte[] destinationBytes, uint offset)
        {
            //for the length of the byte array, assign each byte value to the destination byte array values with the given offset
            for (int i = 0; i < bytes.Length; i++)
            {
                destinationBytes[offset + i] = bytes[i];
            }

            //return the result
            return destinationBytes;
        }

        public static byte[] AllocateBytes(string stringValue, byte[] destinationBytes, uint offset)
        {
            //for the length of the byte array, assign each byte value to the destination byte array values with the given offset
            for (int i = 0; i < stringValue.Length; i++)
            {
                destinationBytes[offset + i] = Convert.ToByte(stringValue[i]);
            }

            //return the result
            return destinationBytes;
        }
    }
}
