using GalaSoft.MvvmLight;
using Personadex.Model;

namespace Personadex.ViewModel
{
    internal sealed class PersonaViewModel : ViewModelBase
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

        public PersonaViewModel(Persona persona)
        {
            _persona = persona;
        }

        private static string GetAffinityFriendlyText(string affinity)
        {
            return string.IsNullOrEmpty(affinity) ? "No value" : affinity;
        }
    }
}