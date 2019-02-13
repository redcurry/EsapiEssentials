using System.Collections.Generic;

namespace EsapiEssentials.PluginRunner
{
    internal class Data
    {
        public Data()
        {
            Recents = new List<RecentEntry>();
        }

        public List<RecentEntry> Recents { get; set; }
    }
}