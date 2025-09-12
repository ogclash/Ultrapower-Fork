using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UCS.Core;
using UCS.Files.Logic;
using UCS.Helpers.Binary;
using UCS.Helpers.List;

namespace UCS.Logic
{
    internal class DataSlot
    {
        public DataSlot(Data d, int value)
        {
            Data  = d;
            Value = value;
        }

        public Data Data;
        public int Value;

        public void Decode(Reader br)
        {
            Data  = br.ReadDataReference();
            Value = br.ReadInt32();
        }

        public byte[] Encode()
        {
            List<byte> data = new List<byte>();
            data.AddInt(Data.GetGlobalID());
            data.AddInt(Value);
            return data.ToArray();
        }

        public void Load(JObject jsonObject)
        {
            Data  = CSVManager.DataTables.GetDataById(jsonObject["global_id"].ToObject<int>());
            Value = jsonObject["value"].ToObject<int>();
        }

        public JObject Save(JObject jsonObject)
        {
            jsonObject.Add("global_id", Data.GetGlobalID());
            jsonObject.Add("value", Value);
            return jsonObject;
        }
    }
}
