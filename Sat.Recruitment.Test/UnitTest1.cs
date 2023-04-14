using System;
using System.Dynamic;

using Microsoft.AspNetCore.Mvc;

using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Api.Models;
using Xunit;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var userController = new UsersController();
            var user = new User(){
                Name= "Mike", 
                Email = "mike@gmail.com", 
                Address = "Av. Juan G", 
                Phone = "+349 1122354215",
                UserType = "Normal", 
                Money = 124
            };

            var result = userController.CreateUser(user).Result;


            Assert.True(result.IsSuccess);
            Assert.Equal("User Created", result.Errors);
        }

        [Fact]
        public void Test2()
        {
            var userController = new UsersController();
            var user = new User()
            {
                Name = "Agustina",
                Email = "Agustina@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                UserType = "Normal",
                Money = 124
            };
            var result = userController.CreateUser(user).Result;

            Assert.False(result.IsSuccess);
            Assert.Equal("The user is duplicated", result.Errors);
        }
    }
}
