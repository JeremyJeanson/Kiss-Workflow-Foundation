using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.UseExtensions.Extensions
{
public sealed class MyExtension : IMyExtension
{
    public void DoAction1()
    {
        Console.WriteLine("Do action 1");
    }

    public void DoAction2()
    {
        Console.WriteLine("Do action 2");
    }
}
}
