using System;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace LoginReg.Models
{
    public static class LoggedIn
    {
        public static void SetLogin(HttpContext context, int id)
        {
            context.Session.SetString("UserID", id.ToString());
        }
        public static int GetUserID(HttpContext context)
        {
            string Id = context.Session.GetString("UserID");
            int UserId = Convert.ToInt32(Id);
            System.Console.WriteLine("User ID " + UserId);
            return UserId;
        }
    }
}
