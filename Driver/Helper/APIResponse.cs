public class APIResponse<T>
{
    public bool HasError { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }

    public APIResponse(bool hasError, T data, string message = null)
    {
        HasError = hasError;
        Data = data;
        Message = message;
    }

    public static APIResponse<T> Success(T data, string message = null)
    {
        return new APIResponse<T>(false, data, message);
    }

    public static APIResponse<T> Error(string message)
    {
        return new APIResponse<T>(true, default(T), message);
    }

    public static APIResponse<T> Created(T data, string message = "Created successfully")
    {
        return new APIResponse<T>(false, data, message);
    }

    public static APIResponse<T> Updated(T data, string message = "Updated successfully")
    {
        return new APIResponse<T>(false, data, message);
    }

    public static APIResponse<T> Deleted(string message = "Deleted successfully")
    {
        return new APIResponse<T>(false, default, message);
    }
}
