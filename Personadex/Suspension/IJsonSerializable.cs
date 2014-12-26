using Newtonsoft.Json.Linq;

namespace Personadex.Suspension
{
    internal interface IJsonSerializable
    {
        string Name
        {
            get;
        }

        JToken Write();

        void Read(JToken value);
    }
}
