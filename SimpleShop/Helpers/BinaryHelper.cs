using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace SimpleShop.Helpers
{
    public class BinaryHelper
    {
        public byte[] ToBinary(object input)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, input);
            byte[] bytes = stream.GetBuffer();
            stream.Close();
            return bytes;
        }

        public T FromBinary<T>(byte[] input)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(input);
            T result = (T)formatter.Deserialize(stream);
            stream.Close();
            return result;
        }
    }
}
