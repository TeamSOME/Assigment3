using System;
using Xunit;
using Assigment3.Controllers;
using Assigment3.Data;
using Assigment3.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using WheatherDatoes = Assigment3.Models.WheatherDato;
using System.Linq;
using System.Collections.Generic;

namespace Opgave3.XUnitTest
{
    public class UnitTest
    {
        private readonly WheatherDatoesController _uut;
        private readonly Assigment3Context _db;

        public UnitTest()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<Assigment3Context>().UseSqlite(connection).Options;
            _db = new Assigment3Context(options);
            _uut = new WheatherDatoesController(_db, null);
        }

        [Fact]
        public async void GetWeatherCorrectPlaceTest()
        {
            _db.Database.EnsureCreated();
            var testWeather = new WheatherDatoes { Date = DateTime.Now, place = new Location { Name = "Singapore"}, TemperatureC = 32 };
            _db.WheatherDato.Add(testWeather);
            _db.SaveChanges();

            var test = await _uut.GetByDateWeatherData(DateTime.Now);
            Assert.Equal("Singapore", test.Value.ElementAt(0).place.Name);
            _db.Database.EnsureDeleted();
        }

        [Fact]
        public async void PostWeatherCorrectPlaceTest()
        {
            _db.Database.EnsureCreated();
            var testWeather = new WheatherDatoes { Date = DateTime.Now, place = new Location { Name = "Lalandia" }, TemperatureC = 32 };
            await _uut.PostWheatherDato(testWeather);

            var test = await _uut.GetWheatherDato();
            List<WheatherDato> weatherList = test.Value.ToList();
            Assert.Equal("Lalandia", weatherList.ElementAt(0).place.Name);
            _db.Database.EnsureDeleted();
        }

        [Fact]
        public async void ByDateWeatherGetsCorrectDataTest()
        {
            _db.Database.EnsureCreated();
            var testWeather = new WheatherDatoes { Date = new DateTime(2021, 05, 24), place = new Location { Name = "Lalandia" }, TemperatureC = 32 };
            var testWeather1 = new WheatherDatoes { Date = new DateTime(2021, 05, 24), place = new Location { Name = "Frederecia" }, TemperatureC = 22 };
            var testWeather2 = new WheatherDatoes { Date = new DateTime(2021, 05, 24), place = new Location { Name = "Aalborg" }, TemperatureC = 34 };
            var testWeather3 = new WheatherDatoes { Date = new DateTime(2021, 05, 24), place = new Location { Name = "Aarhus" }, TemperatureC = 12 };
            var testWeather4 = new WheatherDatoes { Date = new DateTime(2021, 05, 23), place = new Location { Name = "Billund" }, TemperatureC = 25 };
            var testWeather5 = new WheatherDatoes { Date = new DateTime(2021, 05, 22), place = new Location { Name = "Esbjerg" }, TemperatureC = 14 };
            _db.WheatherDato.AddRange(testWeather, testWeather1, testWeather2, testWeather3, testWeather4, testWeather5);
            _db.SaveChanges();

            var test = await _uut.GetByDateWeatherData(new DateTime(2021, 05, 24));
            List<WheatherDato> weatherList = test.Value.ToList();
            Assert.Equal(4, weatherList.Count);

            Assert.Collection(weatherList, item => Assert.Equal("Lalandia", item.place.Name),
                                           item => Assert.Equal("Frederecia", item.place.Name),
                                           item => Assert.Equal("Aalborg", item.place.Name),
                                           item => Assert.Equal("Aarhus", item.place.Name));
            _db.Database.EnsureDeleted();
        }

        [Fact]
        public async void Last3DaysWeatherGetsOnlyLast3DaysTest()
        {
            _db.Database.EnsureCreated();
            var testWeather = new WheatherDatoes { Date = DateTime.Now, place = new Location { Name = "Lalandia" }, TemperatureC = 32 };
            var testWeather1 = new WheatherDatoes { Date = new DateTime(2021, 05, 26), place = new Location { Name = "Frederecia" }, TemperatureC = 22 };
            var testWeather2 = new WheatherDatoes { Date = new DateTime(2021, 05, 25), place = new Location { Name = "Aalborg" }, TemperatureC = 34 };
            var testWeather3 = new WheatherDatoes { Date = new DateTime(2021, 05, 24), place = new Location { Name = "Aarhus" }, TemperatureC = 12 };
            var testWeather4 = new WheatherDatoes { Date = new DateTime(2021, 05, 23), place = new Location { Name = "Billund" }, TemperatureC = 25 };
            var testWeather5 = new WheatherDatoes { Date = new DateTime(2021, 05, 22), place = new Location { Name = "Esbjerg" }, TemperatureC = 14 };
            _db.WheatherDato.AddRange(testWeather, testWeather1, testWeather2, testWeather3, testWeather4, testWeather5);
            _db.SaveChanges();

            var test = await _uut.GetLatestWeatherData();
            List<WheatherDato> weatherList = test.Value.ToList();
            Assert.Equal(3, weatherList.Count);

            Assert.Collection(weatherList, item => Assert.Equal("Lalandia", item.place.Name),
                                           item => Assert.Equal("Frederecia", item.place.Name),
                                           item => Assert.Equal("Aalborg", item.place.Name));
            _db.Database.EnsureDeleted();
        }

        [Fact]
        public async void IntervalWeatherGetsCorrectDataTest()
        {
            _db.Database.EnsureCreated();
            var testWeather = new WheatherDatoes { Date = DateTime.Now, place = new Location { Name = "Lalandia" }, TemperatureC = 32 };
            var testWeather1 = new WheatherDatoes { Date = new DateTime(2021, 05, 26), place = new Location { Name = "Frederecia" }, TemperatureC = 22 };
            var testWeather2 = new WheatherDatoes { Date = new DateTime(2021, 05, 25), place = new Location { Name = "Aalborg" }, TemperatureC = 34 };
            var testWeather3 = new WheatherDatoes { Date = new DateTime(2021, 05, 24), place = new Location { Name = "Aarhus" }, TemperatureC = 12 };
            var testWeather4 = new WheatherDatoes { Date = new DateTime(2021, 05, 23), place = new Location { Name = "Billund" }, TemperatureC = 25 };
            var testWeather5 = new WheatherDatoes { Date = new DateTime(2021, 05, 22), place = new Location { Name = "Esbjerg" }, TemperatureC = 14 };
            _db.WheatherDato.AddRange(testWeather, testWeather1, testWeather2, testWeather3, testWeather4, testWeather5);
            _db.SaveChanges();

            var test = await _uut.GetIntervalWeatherData(new DateTime(2021, 05, 23), new DateTime(2021, 05, 26));
            List<WheatherDato> weatherList = test.Value.ToList();
            Assert.Equal(4, weatherList.Count);

            Assert.Collection(weatherList, item => Assert.Equal("Frederecia", item.place.Name),
                                           item => Assert.Equal("Aalborg", item.place.Name),
                                           item => Assert.Equal("Aarhus", item.place.Name),
                                           item => Assert.Equal("Billund", item.place.Name));
            _db.Database.EnsureDeleted();
        }

        
    }
}
