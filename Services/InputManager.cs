class InputManager
{
    public static bool IsWithinScope<T>(string input, List<T> scope, out int index)
    {
        bool success = int.TryParse(input, out index);
        if (success && index - 1 >= 0 && index - 1 < scope.Count)
        {
            return true;
        }
        return false;
    }

    public static bool IsWithinScope(string input, (int A, int B) limit, out int num)
    {
        bool success = int.TryParse(input, out num);
        if (success && num >= limit.A && num <= limit.B)
        {
            return true;
        }
        return false;
    }

}
