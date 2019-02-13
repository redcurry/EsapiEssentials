using System.IO;
using YamlDotNet.Serialization;

namespace EsapiEssentials.PluginRunner
{
    internal class DataRepository : IDataRepository
    {
        private readonly string _path;

        private readonly ISerializer _serializer;
        private readonly IDeserializer _deserializer;

        public DataRepository(string path)
        {
            _path = path;

            _serializer = new SerializerBuilder().Build();
            _deserializer = new DeserializerBuilder().Build();
        }

        public Data Load()
        {
            if (!File.Exists(_path))
                return new Data();

            var dataString = File.ReadAllText(_path);
            return _deserializer.Deserialize<Data>(dataString);
        }

        public void Save(Data data)
        {
            var dataString = _serializer.Serialize(data);
            File.WriteAllText(_path, dataString);
        }
    }
}