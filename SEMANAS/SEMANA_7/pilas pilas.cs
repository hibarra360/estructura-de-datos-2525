using System;
using System.Collections.Generic;

class BalanceoParentesis
{
    static void Main()
    {
        Console.WriteLine("Verificador de paréntesis balanceados");
        Console.WriteLine("Ingrese una expresión matemática:");
        string expresion = Console.ReadLine();

        if (EstaBalanceada(expresion))
        {
            Console.WriteLine("Fórmula balanceada.");
        }
        else
        {
            Console.WriteLine("Fórmula NO balanceada.");
        }
    }

    /// <summary>
    /// Verifica si los paréntesis, llaves y corchetes están balanceados en una expresión
    /// </summary>
    /// <param name="expresion">La expresión matemática a evaluar</param>
    /// <returns>True si está balanceada, False si no</returns>
    static bool EstaBalanceada(string expresion)
    {
        // Creamos una pila para guardar los símbolos de apertura
        Stack<char> pila = new Stack<char>();

        // Recorremos cada caracter de la expresión
        foreach (char caracter in expresion)
        {
            // Si es un símbolo de apertura, lo agregamos a la pila
            if (caracter == '(' || caracter == '{' || caracter == '[')
            {
                pila.Push(caracter);
            }
            // Si es un símbolo de cierre, verificamos el balanceo
            else if (caracter == ')' || caracter == '}' || caracter == ']')
            {
                // Si la pila está vacía, no hay balanceo
                if (pila.Count == 0)
                {
                    return false;
                }

                // Sacamos el último símbolo de apertura
                char ultimo = pila.Pop();

                // Verificamos que corresponda con el símbolo de cierre
                if (!Corresponden(ultimo, caracter))
                {
                    return false;
                }
            }
        }

        // Si la pila queda vacía, está balanceado
        return pila.Count == 0;
    }

    /// <summary>
    /// Verifica si dos caracteres corresponden (apertura y cierre)
    /// </summary>
    static bool Corresponden(char apertura, char cierre)
    {
        return (apertura == '(' && cierre == ')') ||
               (apertura == '{' && cierre == '}') ||
               (apertura == '[' && cierre == ']');
    }
}

