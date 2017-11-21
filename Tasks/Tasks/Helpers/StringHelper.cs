using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tasks.Helpers
{
    public static class StringHelper
    {
        public static string Letters = "abcdefghijklmnopqrstuvwxyz";
        public static string Get(int length)
        {
            var rd = new Random();
            return  string.Join("", Enumerable.Range(0, length).Select(x => Letters[rd.Next(0, Letters.Length)]).ToList());
        }
       
      
    }
}