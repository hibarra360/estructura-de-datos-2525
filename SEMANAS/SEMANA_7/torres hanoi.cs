using System;
using System.Collections.Generic;

class TorresHanoi
{
    // Representamos las torres como pilas
    static Stack<int> torreA = new Stack<int>();
    static Stack<int> torreB = new Stack<int>();
    static Stack<int> torreC = new Stack<int>();
    static int movimientos = 0;

    static void Main()
    {
        Console.WriteLine("Resolución de Torres de Hanoi");
        Console.Write("Ingrese el número de discos: ");
        int discos = int.Parse(Console.ReadLine());

        // Inicializar la torre A con los discos
        for (int i = discos; i >= 1; i--)
        {
            torreA.Push(i);
        }

        Console.WriteLine("\nConfiguración inicial:");
        MostrarTorres();

        // Resolver el problema
        MoverDiscos(discos, torreA, torreC, torreB);

        Console.WriteLine($"\nResuelto en {movimientos} movimientos.");
    }

    /// <summary>
    /// Mueve discos entre torres siguiendo las reglas de Hanoi
    /// </summary>
    /// <param name="n">Número de discos a mover</param>
    /// <param name="origen">Torre de origen</param>
    /// <param name="destino">Torre de destino</param>
    /// <param name="auxiliar">Torre auxiliar</param>
    static void MoverDiscos(int n, Stack<int> origen, Stack<int> destino, Stack<int> auxiliar)
    {
        if (n > 0)
        {
            // Mover n-1 discos de origen a auxiliar
            MoverDiscos(n - 1, origen, auxiliar, destino);

            // Mover el disco más grande al destino
            if (origen.Count > 0)
            {
                int disco = origen.Pop();
                destino.Push(disco);
                movimientos++;
                Console.WriteLine($"\nMovimiento {movimientos}: Mover disco {disco}");
                MostrarTorres();
            }

            // Mover los n-1 discos de auxiliar a destino
            MoverDiscos(n - 1, auxiliar, destino, origen);
        }
    }

    /// <summary>
    /// Muestra el estado actual de las tres torres
    /// </summary>
    static void MostrarTorres()
    {
        Console.WriteLine("Torre A: " + string.Join(", ", torreA.Reverse()));
        Console.WriteLine("Torre B: " + string.Join(", ", torreB.Reverse()));
        Console.WriteLine("Torre C: " + string.Join(", ", torreC.Reverse()));
    }
}