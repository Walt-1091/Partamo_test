using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sat.Recruitment.Api.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public partial class UsersController : ControllerBase
    {

        private readonly List<User> _users = new List<User>();

        [HttpPost]
        [Route("/create-user")]
        public Task<Result> CreateUser(User newUser)
        {
            var errors = "";

            Utils.Utils.ValidateErrors(newUser, ref errors);

            if (errors != null && errors != "")
                return Task.FromResult(new Result()
                {
                    IsSuccess = false,
                    Errors = errors
                });

            _users.AddRange(Utils.Utils.GetUsersFile());
            newUser.Money = Utils.Utils.ApplyPercentageToType(newUser.UserType, newUser.Money);
            string newEmail = "";
            Utils.Utils.NormalizeEmail(newUser.Email, ref newEmail);
            newUser.Email = newEmail;
            
            try
            {
                var isDuplicated = false;
                foreach (var user in _users)
                {
                    if (user.Email == newUser.Email || user.Phone == newUser.Phone)
                        isDuplicated = true;
                    else if (user.Name == newUser.Name && user.Address == newUser.Address)
                        isDuplicated = true;
                }

                if (!isDuplicated)
                {
                    Debug.WriteLine("User Created");
                    return Task.FromResult(new Result()
                    {
                        IsSuccess = true,
                        Errors = "User Created",
                        user = newUser
                    });
                }
                else
                {
                    Debug.WriteLine("The user is duplicated");
                    return Task.FromResult(new Result()
                    {
                        IsSuccess = false,
                        Errors = "The user is duplicated"
                    });
                }
            }
            catch
            {
                Debug.WriteLine("The user is duplicated");
                return Task.FromResult(new Result()
                {
                    IsSuccess = false,
                    Errors = "The user is duplicated"
                });
            }
        }
    }

}
