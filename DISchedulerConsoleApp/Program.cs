using StructureMap;
using System;

namespace DISchedulerConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = Container.For<ConsoleRegistry>();

            var app = container.GetInstance<Application>();

            app.Run();

            Console.ReadLine();
        }
    }
}
