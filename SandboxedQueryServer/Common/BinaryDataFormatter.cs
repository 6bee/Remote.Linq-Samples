﻿// Copyright (c) Christof Senn. All rights reserved. See license.txt in the project root for license information.

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;

namespace Common
{
    public static class BinaryDataFomratter
    {
        [SecuritySafeCritical]
        public static void Write(this Stream stream, object obj)
        {
            byte[] data;
            using (var dataStream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(dataStream, obj);
                dataStream.Position = 0;
                data = dataStream.ToArray();
            }

            var size = data.LongLength;
            var sizeData = BitConverter.GetBytes(size);

            stream.Write(sizeData, 0, sizeData.Length);
            stream.WriteByte(obj is Exception ? (byte)1 : (byte)0);
            stream.Write(data, 0, data.Length);
        }

        [SecuritySafeCritical]
        public static T Read<T>(this Stream stream)
        {
            var bytes = new byte[256];

            stream.Read(bytes, 0, 8);
            var size = BitConverter.ToInt64(bytes, 0);

            var isException = stream.ReadByte() != 0;

            object obj;
            using (var dataStream = new MemoryStream())
            {
                int count = 0;
                do
                {
                    var length = size - count < bytes.Length
                        ? (int)(size - count)
                        : bytes.Length;

                    int i = stream.Read(bytes, 0, length);
                    count += i;

                    dataStream.Write(bytes, 0, i);
                } while (count < size);

                dataStream.Position = 0;

                var formatter = new BinaryFormatter();
                obj = formatter.Deserialize(dataStream);
            }

            if (isException)
            {
                var exception = (Exception)obj;
                throw exception;
            }

            return (T)obj;
        }
    }
}
