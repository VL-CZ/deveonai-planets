using Planets.Data;
using Planets.Models;

namespace Planets
{
    public class TestDataGenerator
    {
        public static async Task GenerateTestDataAsync(ApplicationDbContext dbContext)
        {
            // Remove all existing data
            dbContext.Planets.RemoveRange(dbContext.Planets);
            dbContext.PlanetPropertyValues.RemoveRange(dbContext.PlanetPropertyValues);
            dbContext.PlanetProperties.RemoveRange(dbContext.PlanetProperties);

            // Planet properties
            var atmosphereProperty = new PlanetProperty { Name = "Atmosphere" };
            var waterProperty = new PlanetProperty { Name = "Water" };
            var terrainProperty = new PlanetProperty { Name = "Terrain" };
            var lifeProperty = new PlanetProperty { Name = "Life" };

            // Property values
            var atmYes = new PlanetPropertyValue { Value = "Yes", PlanetProperty = atmosphereProperty };
            var atmNo = new PlanetPropertyValue { Value = "No", PlanetProperty = atmosphereProperty };
            atmosphereProperty.PossibleValues.Add(atmYes);
            atmosphereProperty.PossibleValues.Add(atmNo);

            var waterYes = new PlanetPropertyValue { Value = "Yes", PlanetProperty = waterProperty };
            var waterNo = new PlanetPropertyValue { Value = "No", PlanetProperty = waterProperty };
            waterProperty.PossibleValues.Add(waterYes);
            waterProperty.PossibleValues.Add(waterNo);

            var terrainRocky = new PlanetPropertyValue { Value = "Rocky", PlanetProperty = terrainProperty };
            var terrainGas = new PlanetPropertyValue { Value = "Gas", PlanetProperty = terrainProperty };
            var terrainIce = new PlanetPropertyValue { Value = "Ice", PlanetProperty = terrainProperty };
            var terrainVolcanic = new PlanetPropertyValue { Value = "Volcanic", PlanetProperty = terrainProperty };
            terrainProperty.PossibleValues.Add(terrainRocky);
            terrainProperty.PossibleValues.Add(terrainGas);
            terrainProperty.PossibleValues.Add(terrainIce);
            terrainProperty.PossibleValues.Add(terrainVolcanic);

            var lifeNone = new PlanetPropertyValue { Value = "None", PlanetProperty = lifeProperty };
            var lifeMicrobial = new PlanetPropertyValue { Value = "Microbial", PlanetProperty = lifeProperty };
            var lifeHumans = new PlanetPropertyValue { Value = "Humans", PlanetProperty = lifeProperty };
            var lifeAliens = new PlanetPropertyValue { Value = "Aliens", PlanetProperty = lifeProperty };
            lifeProperty.PossibleValues.Add(lifeNone);
            lifeProperty.PossibleValues.Add(lifeMicrobial);
            lifeProperty.PossibleValues.Add(lifeHumans);

            // Planets (subset of properties)
            var planets = new List<Planet>
            {
                new() { Name = "Earth", PropertyValues = [atmYes, waterYes, terrainRocky, lifeHumans] },
                new() { Name = "Mars", PropertyValues = [atmYes, waterNo, lifeMicrobial] },
                new() { Name = "Mercury", PropertyValues = [terrainRocky] },
                new() { Name = "Venus", PropertyValues = [atmYes, terrainRocky] },
                new() { Name = "Jupiter", PropertyValues = [atmYes, terrainGas] },
                new() { Name = "Europa", PropertyValues = [waterYes, atmNo, terrainIce, lifeAliens] }
            };

            // Store & save data
            dbContext.Planets.AddRange(planets);
            dbContext.PlanetProperties.AddRange([atmosphereProperty, waterProperty, terrainProperty, lifeProperty]); // the property values are added automatically

            await dbContext.SaveChangesAsync();
        }
    }
}
