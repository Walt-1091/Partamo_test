using Sat.Recruitment.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection.PortableExecutable;

namespace Sat.Recruitment.Api.Utils
{
    public static class Utils
    {
        public static void ValidateErrors(User user, ref string errors)
        {
            if (user.Name == null)
                //Validate if Name is null
                errors = "The name is required";
            if (user.Email == null)
                //Validate if Email is null
                errors = errors + " The email is required";
            if (user.Address == null)
                //Validate if Address is null
                errors = errors + " The address is required";
            if (user.Phone == null)
                //Validate if Phone is null
                errors = errors + " The phone is required";
        }

        private static StreamReader ReadUsersFromFile()
        {
            var path = Directory.GetCurrentDirectory() + "/Files/Users.txt";
            FileStream fileStream = new FileStream(path, FileMode.Open);
            StreamReader reader = new StreamReader(fileStream);
            return reader;
        }

        public static void NormalizeEmail(string email, ref string newEmail) {
            var aux = email.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
            var atIndex = aux[0].IndexOf("+", StringComparison.Ordinal);
            aux[0] = atIndex < 0 ? aux[0].Replace(".", "") : aux[0].Replace(".", "").Remove(atIndex);
            newEmail = string.Join("@", new string[] { aux[0], aux[1] });
        }

        public static decimal ApplyPercentageToType(string userType, decimal money) {
            if (userType == "Normal")
            {
                if (money > 100)
                {
                    var percentage = Convert.ToDecimal(0.12);
                    return ReturnNewValueMoney(percentage, money);
                }
                else if (money > 10)
                {
                    var percentage = Convert.ToDecimal(0.8);
                    return ReturnNewValueMoney(percentage, money);
                }
            }
            else if (userType == "SuperUser" && money > 100)
            {
                var percentage = Convert.ToDecimal(0.20);
                return ReturnNewValueMoney(percentage, money);
            }
            else if (userType == "Premium" && money > 100)
                return ReturnNewValueMoney(Convert.ToDecimal(2), money);
             
            return money;
        }

        private static decimal ReturnNewValueMoney(decimal percentage, decimal money)
        {
            var gif = money * percentage;
            return money + gif;
        }

        public static List<User> GetUsersFile() {
            List<User> users = new List<User>();
            var reader = ReadUsersFromFile();
            while (reader.Peek() >= 0)
            {
                var line = reader.ReadLineAsync().Result;
                var user = new User
                {
                    Name = line.Split(',')[0].ToString(),
                    Email = line.Split(',')[1].ToString(),
                    Phone = line.Split(',')[2].ToString(),
                    Address = line.Split(',')[3].ToString(),
                    UserType = line.Split(',')[4].ToString(),
                    Money = decimal.Parse(line.Split(',')[5].ToString()),
                };
                users.Add(user);
            }
            reader.Close();
            return users;
        }
    }
}
