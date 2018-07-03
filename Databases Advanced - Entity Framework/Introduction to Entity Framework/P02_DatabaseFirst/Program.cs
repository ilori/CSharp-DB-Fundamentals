namespace P02_DatabaseFirst
{
    using System;
    using System.Globalization;
    using System.IO;
    using Data;
    using System.Linq;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class Program
    {
        public static void Main()
        {
            var db = new SoftUniContext();
            using (db)
            {

            }
        }
    }
}