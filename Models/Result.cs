public readonly record struct Result(bool IsSuccess, string? ErrorKey = null)
{
    public static Result Ok() => new(true);
    public static Result Fail(string errorKey) => new(false, errorKey);
}

