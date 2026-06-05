using System;

namespace BankingApp;
class Program
{
    static int Main(string[] args)
    {
        var app = new App();
        return app.Execute(args);
    }
}