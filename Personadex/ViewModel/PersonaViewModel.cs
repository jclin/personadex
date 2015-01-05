using System;
using GalaSoft.MvvmLight;
using Newtonsoft.Json.Linq;
using Personadex.Model;
using Personadex.Suspension;
using Personadex.Utils;

namespace Personadex.ViewModel
{
    internal sealed class PersonaViewModel : ViewModelBase, IEquatable<PersonaViewModel>, IJsonSerializable
    {
        private readonly Persona _persona;

        public string Name
        {
            get
            {
                return _persona.Name;
            }
        }

        public uint Level
        {
            get
            {
                return _persona.Level;
            }
        }

        public string Physical
        {
            get
            {
                return GetAffinityFriendlyText(_persona.Physical);
            }
        }

        public string Fire
        {
            get
            {
                return GetAffinityFriendlyText(_persona.Fire);
            }
        }

        public string Ice
        {
            get
            {
                return GetAffinityFriendlyText(_persona.Ice);
            }
        }

        public string Electricity
        {
            get
            {
                return GetAffinityFriendlyText(_persona.Electricity);
            }
        }

        public string Wind
        {
            get
            {
                return GetAffinityFriendlyText(_persona.Wind);
            }
        }

        public string Light
        {
            get
            {
                return GetAffinityFriendlyText(_persona.Light);
            }
        }

        public string Dark
        {
            get
            {
                return GetAffinityFriendlyText(_persona.Dark);
            }
        }

        public string Arcana
        {
            get
            {
                return _persona.Arcana;
            }
        }

        public string Skills
        {
            get
            {
                return _persona.Skills.Trim();
            }
        }

        public Uri SmallImageUri
        {
            get
            {
                return ImageUriBuilder.BuildUri(_persona.Name, ImageSize.Small);
            }
        }

        public Uri LargeImageUri
        {
            get
            {
                return ImageUriBuilder.BuildUri(_persona.Name, ImageSize.Large);
            }
        }

        public PersonaViewModel(Persona persona)
        {
            _persona = persona;
        }

        public bool Equals(PersonaViewModel other)
        {
            if (other == null)
            {
                return false;
            }

            return other._persona.Id == _persona.Id;
        }

        public override bool Equals(object other)
        {
            return Equals(other as PersonaViewModel);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = hashCode * 23 + _persona.GetHashCode();

                return hashCode;
            }
        }

        private static string GetAffinityFriendlyText(string affinity)
        {
            return string.IsNullOrEmpty(affinity) ? "No value" : affinity;
        }

        string IJsonSerializable.Name
        {
            get
            {
                return Persona.JsonName;
            }
        }

        JToken IJsonSerializable.Write()
        {
            return ((IJsonSerializable) _persona).Write();
        }

        void IJsonSerializable.Read(JToken value)
        {
            ((IJsonSerializable)_persona).Read(value);
        }
    }
}