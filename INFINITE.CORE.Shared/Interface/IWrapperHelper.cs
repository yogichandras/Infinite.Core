using INFINITE.CORE.Shared.Attributes;

namespace INFINITE.CORE.Shared.Interface
{
    public interface IWrapperHelper
    {
        void Response<T>(ref ObjectResponse<T> a, StatusResponse b);
        void Response<T>(ref ListResponse<T> a, StatusResponse b);
        void Response<T>(ref StatusResponse a, ListResponse<T> b);
        void Response<T>(ref StatusResponse a, ObjectResponse<T> b);
        ObjectResponse<T> Response<T>((bool Status, string Message, T Result, Exception ex) a);
        ListResponse<T> Response<T>((bool Status, string Message, List<T> Result, Exception ex) a);
        StatusResponse Response((bool Status, string Message, Exception ex) a);
        ObjectResponse<T> Response<T>((bool Status, string Message, ObjectResponse<T> Result, Exception ex) a);
        ListResponse<T> Response<T>((bool Status, string Message, ListResponse<T> Result, Exception ex) a);
        StatusResponse Response((bool Status, string Message, StatusResponse Result, Exception ex) a);
        StatusResponse Response((bool Success, string Message, Exception ex, List<ChangeLog> log) value);
    }
}
