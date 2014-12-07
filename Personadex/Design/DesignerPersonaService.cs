using System.Collections.Generic;
using Personadex.Model;
using Personadex.Utils;

namespace Personadex.Design
{
    internal sealed class DesignerPersonaService : IPersonaService
    {
        private readonly IReadOnlyList<Persona> _samplePersonas =
            new List<Persona>
            {
                new Persona
                {
                    Name        = "Izanagi",
                    Level       = 1,
                    Physical    = string.Empty,
                    Fire        = string.Empty,
                    Ice         = string.Empty,
                    Electricity = "Str",
                    Wind        = "Wk",
                    Light       = string.Empty,
                    Dark        = "Nul",
                    Arcana      = "Fool",
                    Skills      = "Zio,Cleave, Rakukaja Rakunda(3) Tarukaja(5)"
                },

                new Persona
                {
                    Name        = "Ose",
                    Level       = 3,
                    Physical    = string.Empty,
                    Fire        = string.Empty,
                    Ice         = string.Empty,
                    Electricity = "Str",
                    Wind        = "Wk",
                    Light       = string.Empty,
                    Dark        = "Nul",
                    Arcana      = "Fool",
                    Skills      = "Zio,Cleave, Rakukaja Rakunda(3) Tarukaja(5)"
                },

                new Persona
                {
                    Name        = "Yomotsu-shikome",
                    Level       = 7,
                    Physical    = string.Empty,
                    Fire        = "Wk",
                    Ice         = "Str",
                    Electricity = string.Empty,
                    Wind        = string.Empty,
                    Light       = string.Empty,
                    Dark        = string.Empty,
                    Arcana      = "Fool",
                    Skills      = "Poisma,Skewer, Evil Touch, Sukunda(9), Mudo(10), Ghastly Wail(11)"
                },

                new Persona
                {
                    Name        = "Kahaku (AKA Hua Po)",
                    Level       = 25,
                    Physical    = string.Empty,
                    Fire        = "Nul",
                    Ice         = "Wk",
                    Electricity = string.Empty,
                    Wind        = string.Empty,
                    Light       = string.Empty,
                    Dark        = string.Empty,
                    Arcana      = "Magician",
                    Skills      = "Agilao,Re Patra,Rakukaja, Fire Break(26)Makajama(27)Dodge Ice (29), Fire Boost(30)"
                },

                new Persona
                {
                    Name        = "Trumpeter",
                    Level       = 67,
                    Physical    = string.Empty,
                    Fire        = string.Empty,
                    Ice         = "Dr",
                    Electricity = "Rpl",
                    Wind        = string.Empty,
                    Light       = "Rpl",
                    Dark        = "Nul",
                    Arcana      = "Judgement",
                    Skills      = "Megidola, Ziodyne, Elec Amp, Masukukaja(68), Cool Breeze(69), Megidolaon(70), Debilitate(73), Heat Riser(74)"
                },

                new Persona
                {
                    Name        = "Izanagi No Okami",
                    Level       = 91,
                    Physical    = "Str",
                    Fire        = "Str",
                    Ice         = "Str",
                    Electricity = "Str",
                    Wind        = "Str",
                    Light       = string.Empty,
                    Dark        = string.Empty,
                    Arcana      = "The World",
                    Skills      = "Megidolaon,Victory Cry, Angelic Grace, Mind Charge, Agidyne(92), Bufudyne(93), Garudyne(94), Ziodyne(95), Fire Amp(96), Ice Amp(97) Elec Amp(98), Wind Amp(99)"
                }
            };

        public long GetPersonaCount()
        {
            return _samplePersonas.Count;
        }

        public IReadOnlyList<Persona> GetPersonas(Range range)
        {
            return _samplePersonas;
        }
    }
}