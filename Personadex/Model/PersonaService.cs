using System.Collections.Generic;
using System.IO;
using SQLite;

namespace Personadex.Model
{
    internal sealed class PersonaService : IPersonaService
    {
        private const string Comma          = ",";
        private const string Newline        = "\n";
        private const string DatabaseName   = "Personas.db";

        private const string SelectPersonasQuery = 
            "SELECT" + Newline
                + "Persona.Name as "            + "Name" + Comma + Newline
                + "Level" + Comma + Newline
                + "PhysLevel.Name as "          + "Physical" + Comma + Newline
                + "FireLevel.Name as "          + "Fire" + Comma + Newline
                + "IceLevel.Name as "           + "Ice" + Comma + Newline
                + "ElectricityLevel.Name as "   + "Electricity" + Comma + Newline
                + "WindLevel.Name as "          + "Wind" + Comma + Newline
                + "LightLevel.Name as "         + "Light" + Comma + Newline
                + "DarkLevel.Name as "          + "Dark" + Comma + Newline
                + "Arcana.Name as "             + "Arcana" + Comma + Newline
                + "Skills" + Newline
                + "FROM Persona" + Newline
                + "JOIN ElementAffinity as PhysLevel" + Newline
                + "ON Persona.Physical=PhysLevel._id" + Newline
                + "JOIN ElementAffinity as FireLevel" + Newline
                + "ON Persona.Fire=FireLevel._id" + Newline
                + "JOIN ElementAffinity as IceLevel" + Newline
                + "ON Persona.Ice=IceLevel._id" + Newline
                + "JOIN ElementAffinity as ElectricityLevel" + Newline
                + "ON Persona.Electricity=ElectricityLevel._id" + Newline
                + "JOIN ElementAffinity as WindLevel" + Newline
                + "ON Persona.Wind=WindLevel._id" + Newline
                + "JOIN ElementAffinity as LightLevel" + Newline
                + "ON Persona.Light=LightLevel._id" + Newline
                + "JOIN ElementAffinity as DarkLevel" + Newline
                + "ON Persona.Dark=DarkLevel._id" + Newline
                + "JOIN Arcana" + Newline
                + "ON Persona.Arcana=Arcana._id";

        public IReadOnlyList<Persona> GetPersonas()
        {
            var dbConnection = new SQLiteConnection(
                Path.Combine(
                    Windows.ApplicationModel.Package.Current.InstalledLocation.Path,
                    DatabaseName
                    )
                );

            var personas = dbConnection.Query<Persona>(SelectPersonasQuery);

            return personas;
        }
    }
}