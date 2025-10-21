using System.Collections.Generic;
using System.Linq;
using System;

public static class ListExtensions
{
    //PARA HACER EL SHUFFLE DE JEFEMAZMORRA EN LOS DESAFIOS MATEMATICOS.
    public static void Shuffle<T>(this IList<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
