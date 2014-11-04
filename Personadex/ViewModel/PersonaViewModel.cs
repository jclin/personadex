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
                return _persona.Physical;
            }
        }

        public string Fire
        {
            get
            {
                return _persona.Fire;
            }
        }

        public string Ice
        {
            get
            {
                return _persona.Ice;
            }
        }

        public string Electricity
        {
            get
            {
                return _persona.Electricity;
            }
        }

        public string Wind
        {
            get
            {
                return _persona.Wind;
            }
        }

        public string Light
        {
            get
            {
                return _persona.Light;
            }
        }

        public string Dark
        {
            get
            {
                return _persona.Dark;
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
                return _persona.Skills;
            }
        }

        public PersonaViewModel(Persona persona)
        {
            _persona = persona;
        }
    }
}