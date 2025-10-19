using NuGet.Packaging;
using Planets.Data;
using Planets.Models;

namespace Planets
{
    public class TestDataGenerator
    {
        /// <summary>
        /// Generates the test data and stores them in the DB.
        /// </summary>
        /// <remarks>
        /// NOTE: All existing data in the DB are removed.
        /// </remarks>
        /// <param name="dbContext">The database context.</param>
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
            var atmYes = new PlanetPropertyValue { Value = "Yes" };
            var atmNo = new PlanetPropertyValue { Value = "No" };

            var waterYes = new PlanetPropertyValue { Value = "Yes" };
            var waterNo = new PlanetPropertyValue { Value = "No" };

            var terrainRocky = new PlanetPropertyValue { Value = "Rocky" };
            var terrainGas = new PlanetPropertyValue { Value = "Gas" };
            var terrainIce = new PlanetPropertyValue { Value = "Ice" };
            var terrainVolcanic = new PlanetPropertyValue { Value = "Volcanic" };

            var lifeNone = new PlanetPropertyValue { Value = "None" };
            var lifeMicrobial = new PlanetPropertyValue { Value = "Microbial" };
            var lifeHumans = new PlanetPropertyValue { Value = "Humans" };
            var lifeAliens = new PlanetPropertyValue { Value = "Aliens" };

            // Assign values to properties
            atmosphereProperty.PossibleValues.AddRange([atmYes, atmNo]);
            waterProperty.PossibleValues.AddRange([waterYes, waterNo]);
            terrainProperty.PossibleValues.AddRange([terrainRocky, terrainGas, terrainIce, terrainVolcanic]);
            lifeProperty.PossibleValues.AddRange([lifeNone, lifeMicrobial, lifeHumans, lifeAliens]);

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
