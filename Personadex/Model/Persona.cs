using Newtonsoft.Json.Linq;
using Personadex.Suspension;

namespace Personadex.Model
{
    internal sealed class Persona : IJsonSerializable
    {
        public const string JsonName = "Persona";

        public uint Id { get; set; }

        public string Name { get; set; }

        public uint Level { get; set; }

        public string Physical { get; set; }

        public string Fire { get; set; }

        public string Ice { get; set; }

        public string Electricity { get; set; }

        public string Wind { get; set; }

        public string Light { get; set; }

        public string Dark { get; set; }

        public string Arcana { get; set; }

        public string Skills { get; set; }

        string IJsonSerializable.Name
        {
            get
            {
                return JsonName;
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int)Id;

                hashCode = hashCode * 23 + Name.GetHashCode();
                hashCode = hashCode * 23 + Level.GetHashCode();
                hashCode = hashCode * 23 + Physical.GetHashCode();
                hashCode = hashCode * 23 + Fire.GetHashCode();
                hashCode = hashCode * 23 + Ice.GetHashCode();
                hashCode = hashCode * 23 + Electricity.GetHashCode();
                hashCode = hashCode * 23 + Wind.GetHashCode();
                hashCode = hashCode * 23 + Light.GetHashCode();
                hashCode = hashCode * 23 + Dark.GetHashCode();
                hashCode = hashCode * 23 + Skills.GetHashCode();

                return hashCode;
            }
        }

        JToken IJsonSerializable.Write()
        {
            return new JObject(
                new JProperty("Id", Id),
                new JProperty("Name", Name),
                new JProperty("Level", Level),
                new JProperty("Physical", Physical),
                new JProperty("Fire", Fire),
                new JProperty("Ice", Ice),
                new JProperty("Electricity", Electricity),
                new JProperty("Wind", Wind),
                new JProperty("Light", Light),
                new JProperty("Dark", Dark),
                new JProperty("Arcana", Arcana),
                new JProperty("Skills", Skills)
                );
        }

        void IJsonSerializable.Read(JToken value)
        {
            Id          = value["Id"].ToObject<uint>();
            Name        = value["Name"].ToObject<string>();
            Level       = value["Level"].ToObject<uint>();
            Physical    = value["Physical"].ToObject<string>();
            Fire        = value["Fire"].ToObject<string>();
            Ice         = value["Ice"].ToObject<string>();
            Electricity = value["Electricity"].ToObject<string>();
            Wind        = value["Wind"].ToObject<string>();
            Light       = value["Light"].ToObject<string>();
            Dark        = value["Dark"].ToObject<string>();
            Arcana      = value["Arcana"].ToObject<string>();
            Skills      = value["Skills"].ToObject<string>();
        }
    }
}
