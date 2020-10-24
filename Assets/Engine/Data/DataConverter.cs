using System;
using System.Collections.Generic;

namespace enjoythevibes.Serialization
{
    public static class DataConverter
    {
        public static void SetBytes(byte[] bytes, int value, ref int offset)
        {
            var data = BitConverter.GetBytes(value);
            Buffer.BlockCopy(data, 0, bytes, offset, 4);
            offset += 4;
        }

        public static void SetBytes(byte[] bytes, List<int> value, ref int offset)
        {
            SetBytes(bytes, value.Count, ref offset);
            foreach (var item in value)
            {
                SetBytes(bytes, item, ref offset);
            }
        }

        public static void SetBytes(byte[] bytes, float value, ref int offset)
        {
            var data = BitConverter.GetBytes(value);
            Buffer.BlockCopy(data, 0, bytes, offset, 4);
            offset += 4;
        }

        public static void SetBytes(byte[] bytes, bool value, ref int offset)
        {
            var data = BitConverter.GetBytes(value);
            Buffer.BlockCopy(data, 0, bytes, offset, 1);
            offset += 1;
        }

        public static int? ReadInt32Bytes(byte[] bytes, ref int offset)
        {
            if (offset + 4 > bytes.Length) return null;
            var data = BitConverter.ToInt32(bytes, offset);
            offset += 4;
            return data;
        }

        public static float? ReadFloatBytes(byte[] bytes, ref int offset)
        {
            if (offset + 4 > bytes.Length) return null;
            var data = BitConverter.ToSingle(bytes, offset);
            offset += 4;
            return data;
        }    

        public static bool? ReadBoolBytes(byte[] bytes, ref int offset)
        {
            if (offset + 1 > bytes.Length) return null;
            var data = BitConverter.ToBoolean(bytes, offset);
            offset += 1;
            return data;
        }

        public static void ReadListInt(byte[] bytes, ref int offset, int count, List<int> list)
        {
            if (offset + (sizeof(int) * count) > bytes.Length) return;
            list.Clear();
            for (int i = 0; i < count; i++)
            {
                var item = ReadInt32Bytes(bytes, ref offset);
                if (item.HasValue)
                {
                    list.Add(item.Value);
                }
                #if UNITY_EDITOR
                else
                {
                    UnityEngine.Debug.LogError("Error: ReadListInt");
                }
                #endif
            }
        }
    }
}