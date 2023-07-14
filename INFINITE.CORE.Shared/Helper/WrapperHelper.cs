using INFINITE.CORE.Shared.Interface;
using INFINITE.CORE.Shared.Attributes;
using System.Collections.Generic;
using System;

namespace INFINITE.CORE.Shared.Helper
{
    public class WrapperHelper : IWrapperHelper
    {
        public void Response<T>(ref ObjectResponse<T> a, StatusResponse b)
        {
            a.Succeeded = b.Succeeded;
            a.Code = b.Code;
            a.Description = b.Description;
            a.Message = b.Message;
        }
        public void Response<T>(ref ListResponse<T> a, StatusResponse b)
        {
            a.Succeeded = b.Succeeded;
            a.Code = b.Code;
            a.Description = b.Description;
            a.Message = b.Message;
        }
        public void Response<T>(ref StatusResponse a, ListResponse<T> b)
        {
            a.Succeeded = b.Succeeded;
            a.Code = b.Code;
            a.Description = b.Description;
            a.Message = b.Message;
        }
        public void Response<T>(ref StatusResponse a, ObjectResponse<T> b)
        {
            a.Succeeded = b.Succeeded;
            a.Code = b.Code;
            a.Description = b.Description;
            a.Message = b.Message;
        }

        #region Code To Response
        public ObjectResponse<T> Response<T>((bool Status, string Message, T Result, Exception ex) a)
        {
            var result = new ObjectResponse<T>();
            if (a.Status)
                result.OK();
            else
            {
                result.BadRequest(a.Message);
                result.Description = a.ex != null ? a.ex.InnerException?.ToString() ?? "" : "";
            }
            result.Data = a.Result;
            return result;
        }
        public ListResponse<T> Response<T>((bool Status, string Message, List<T> Result, Exception ex) a)
        {
            var result = new ListResponse<T>();
            if (a.Status)
                result.OK();
            else
            {
                result.BadRequest(a.Message);
                result.Description = a.ex != null ? a.ex.InnerException?.ToString() ?? "" : "";
            }
            result.List = a.Result;
            return result;
        }
        public StatusResponse Response((bool Status, string Message, Exception ex) a)
        {
            var result = new StatusResponse();
            if (a.Status)
                result.OK();
            else
            {
                result.BadRequest(a.Message);
                result.Description = a.ex != null ? a.ex.InnerException?.ToString() ?? "" : "";
            }
            return result;
        }
        #endregion

        public ObjectResponse<T> Response<T>((bool Status, string Message, ObjectResponse<T> Result, Exception ex) a)
        {
            var result = new ObjectResponse<T>();
            if (!a.Status)
            {
                if (a.Result != null)
                    result.Data = a.Result.Data;

                result.BadRequest(a.Result?.Message ?? a.Message);
                result.Description = a.ex != null ? a.ex.InnerException?.ToString() ?? "" : "";
                return result;
            }

            return a.Result;
        }

        public ListResponse<T> Response<T>((bool Status, string Message, ListResponse<T> Result, Exception ex) a)
        {
            var result = new ListResponse<T>();
            if (!a.Status)
            {
                if (a.Result != null)
                    result.List = a.Result.List;

                result.BadRequest(a.Result?.Message ?? a.Message);
                result.Description = a.ex != null ? a.ex.InnerException?.ToString() ?? "" : "";
                return result;
            }

            return a.Result;
        }

        public StatusResponse Response((bool Status, string Message, StatusResponse Result, Exception ex) a)
        {
            var result = new StatusResponse();
            if (!a.Status)
            {
                result.BadRequest(a.Result?.Message ?? a.Message);
                result.Description = a.ex != null ? a.ex.InnerException?.ToString() ?? "" : "";
                return result;
            }

            return a.Result;
        }

        public StatusResponse Response((bool Success, string Message, Exception ex, List<ChangeLog> log) a)
        {
            var result = new StatusResponse();
            if (!a.Success)
            {
                result.BadRequest(a.Message);
                result.Description = a.ex != null ? a.ex.InnerException?.ToString() ?? "" : "";
                return result;
            }
            result.OK();
            return result;
        }
    }
}
