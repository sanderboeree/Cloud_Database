using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OnlineStore.Api.Domain.Football;
using OnlineStore.Api.Domain.Users;

namespace OnlineStore.Api.Tests
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "BirthDate should not be in the future")]
        public void CreatePlayerWhereBirthdayIsInFuture()
        {
            var player = new Player()
            {
                BirthDate = new DateTime(2050, 12, 31)
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "BirthDate should not be empty")]
        public void CreatePlayerWhereBirthdayIsMinValue()
        {
            var player = new Player()
            {
                BirthDate = DateTime.MinValue
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "WeightInKg should not be less than zero")]
        public void CreatePlayerWhereWeightInKgIsNegative()
        {
            var player = new Player()
            {
                WeightInKg = -1
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "LengthInCm should not be less than zero")]
        public void CreatePlayerWhereLengthInCmIsNegative()
        {
            var player = new Player()
            {
                LengthInCm = -1
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Level should not be NotSpecified")]
        public void CreatePlayerWhereLevelIsNotSpecified()
        {
            var player = new Player()
            {
                Level = Level.NotSpecified
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "FavouritePosition should not be NotSpecified")]
        public void CreatePlayerWhereFavouritePositionIsNotSpecified()
        {
            var player = new Player()
            {
                FavouritePosition = Position.NotSpecified
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "TeamRole should not be NotSpecified")]
        public void CreatePlayerWhereTeamRoleIsNotSpecified()
        {
            var player = new Player()
            {
                TeamRole = Position.NotSpecified
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Gender should not be NotSpecified")]
        public void CreatePlayerWhereGenderIsNotSpecified()
        {
            var player = new Player()
            {
                Gender = Gender.NotSpecified
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Name should not be empty")]
        public void CreateUserWhereNameIsEmpty()
        {
            var user = new User()
            {
                Name = string.Empty
            };
        }
    }
}
