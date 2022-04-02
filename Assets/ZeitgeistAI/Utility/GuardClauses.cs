using System.Collections;
using System.Collections.Generic;

public static class GuardClauses
{
    public static bool IsPositive(int value) { return value >= 0; }
    public static bool IsPositive(float value) { return value >= 0; }
    public static bool IsPositive(double value) { return value >= 0; }

    public static bool IsNotEmpty<T>(ICollection<T> collection) { return collection.Count > 0; }
}
