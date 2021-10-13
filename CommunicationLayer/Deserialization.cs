using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    class Deserialization
    {
        public static T JsonDeserialize<T>(byte[] jsonString)
        {

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream((jsonString));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }
    }
}
