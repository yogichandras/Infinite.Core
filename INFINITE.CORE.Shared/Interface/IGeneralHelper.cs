namespace INFINITE.CORE.Shared.Interface
{
    public interface IGeneralHelper
    {
        bool ValidatePassword(string password);
        string PasswordEncrypt(string text);
        T Clone<T>(T obj);
    }
}

