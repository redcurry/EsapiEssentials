namespace EsapiEssentials.PluginRunner
{
    internal interface IDataRepository
    {
        Data Load();
        void Save(Data data);
    }
}