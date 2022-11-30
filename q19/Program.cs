using System;

Vetor v = (1, 2);
Vetor u = (4, 3);
Vetor r = (5, 5);

Console.WriteLine($"{v} + {u} = {r}?");

if (v + u == r)
    Console.WriteLine("Sim!");
else Console.WriteLine("Não! (mas devia)");


public class Vetor
{
    public Vetor(int[] dados)
    {
        throw new NotImplementedException();
    }

    public bool EIgual(Vetor vetor)
    {
        throw new NotImplementedException();
    }

    public Vetor Soma(Vetor vetor)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        throw new NotImplementedException();
    }

    public static implicit operator Vetor((int, int) tupla)
        => new Vetor(new int[] {
            tupla.Item1, 
            tupla.Item2
        });

    public static implicit operator Vetor((int, int, int) tupla)
        => new Vetor(new int[] {
            tupla.Item1, 
            tupla.Item2,
            tupla.Item3
        });

    public static Vetor operator +(Vetor v, Vetor u)
        => v.Soma(u);

    public static bool operator ==(Vetor v, Vetor u)
        => v.EIgual(u);

    public static bool operator !=(Vetor v, Vetor u)
        => !v.EIgual(u);
}
