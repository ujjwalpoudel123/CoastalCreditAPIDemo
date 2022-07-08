/*********************************************************
 * 
 * Author : Ujjwal R. Poudel
 * Date:    July 7th 2022
 * Project: Coastal Credit Job Interview Mini Project
 * 
 * Description of the project : 
 * Create a single-page mobile-first web application that allows the user to control their credit card.  
 * The application will allow the user to lock/freeze their card to disable transactions or unlock/unfreeze it to enable transactions again.  
 * The current state of the card should be maintained within the “session”.  The user should get a confirmation that their submission was successful.  
 * The user should be able to submit messages or report an issue with their card (i.e. lost/damaged/stolen).  
 * If a card is reported lost/damaged/stolen then it should also be frozen.
 * 
 * 
 ********************************************************/
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace APIDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
