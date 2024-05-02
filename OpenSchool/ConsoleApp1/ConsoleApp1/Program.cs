using MassTransit.Internals;
using System;

public class HelloWorld
{
    public interface A { }
    public interface B : A { }
    public class V_B : B { }
    public static void Main(string[] args)
    {
        Console.WriteLine (typeof(V_B).HasInterface<A>());
    }
}