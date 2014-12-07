using System.Collections.Generic;
using System.IO;
using Personadex.Utils;
using SQLitePCL;

namespace Personadex.Model
{
    internal sealed class PersonaService : IPersonaService
    {
        private const string Comma        = ",";
        private const string Newline      = "\n";
        private const string DatabaseName = "Personas.db";

        private const string SelectAllPersonasQuery =
            "SELECT" + Newline
                + "Persona._id as " + "Id" + Comma + Newline
                + "Persona.Name as " + "Name" + Comma + Newline
                + "Level" + Comma + Newline
                + "PhysLevel.Name as " + "Physical" + Comma + Newline
                + "FireLevel.Name as " + "Fire" + Comma + Newline
                + "IceLevel.Name as " + "Ice" + Comma + Newline
                + "ElectricityLevel.Name as " + "Electricity" + Comma + Newline
                + "WindLevel.Name as " + "Wind" + Comma + Newline
                + "LightLevel.Name as " + "Light" + Comma + Newline
                + "DarkLevel.Name as " + "Dark" + Comma + Newline
                + "Arcana.Name as " + "Arcana" + Comma + Newline
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

        private const string SelectPersonaRangeQuery =
            SelectAllPersonasQuery + Newline
                + "WHERE Persona._id >= ? AND Persona._id <= ?";

        private const string PersonaCountQuery =
            "SELECT COUNT(*) FROM Persona";

        public long GetPersonaCount()
        {
            using (var connection = OpenDatabaseConnection())
            using (var statement = connection.Prepare(PersonaCountQuery))
            {
                if (statement.Step() != SQLiteResult.ROW)
                {
                    throw new SQLiteException("Couldn't get count of personas");
                }

                return (long)statement[0];
            }
        }

        public IReadOnlyList<Persona> GetPersonas(Range range)
        {
            var personas = new List<Persona>();

            using (var connection = OpenDatabaseConnection())
            using (var statement  = connection.Prepare(SelectPersonaRangeQuery))
            {
                statement.Bind(1, range.Start + 1);
                statement.Bind(2, range.End + 1);

                while (statement.Step() == SQLiteResult.ROW)
                {
                    personas.Add(ToPersona(statement));
                }
            }

            return personas;
        }

        private static SQLiteConnection OpenDatabaseConnection()
        {
            return
                new SQLiteConnection(
                    Path.Combine(
                        Windows.ApplicationModel.Package.Current.InstalledLocation.Path,
                        DatabaseName
                        )
                );
        }

        private static Persona ToPersona(ISQLiteStatement statement)
        {
            return new Persona
            {
                Id          = (uint)(long)statement["Id"],
                Name        = (string)statement["Name"],
                Level       = (uint)(long)statement["Level"],
                Physical    = (string)statement["Physical"],
                Fire        = (string)statement["Fire"],
                Ice         = (string)statement["Ice"],
                Electricity = (string)statement["Electricity"],
                Wind        = (string)statement["Wind"],
                Light       = (string)statement["Light"],
                Dark        = (string)statement["Dark"],
                Arcana      = (string)statement["Arcana"],
                Skills      = (string)statement["Skills"]
            };
        }
    }
}