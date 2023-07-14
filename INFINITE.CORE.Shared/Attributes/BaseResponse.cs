namespace INFINITE.CORE.Shared.Attributes
{
    public class StatusResponse
    {
        public StatusResponse()
        {
            BadRequest();
        }

        public int Code { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; } = null!;
        public string Description { get; set; }

        public void SetMessage(string message)
        {
            this.Message = message;
        }

        public void OK()
        {
            Code = 200;
            Succeeded = true;
            Message = "All OK";
            Description = "";
        }

        public void OK(string message)
        {
            OK();
            this.Message = message;
        }

        public void BadRequest()
        {
            Code = 400;
            Succeeded = false;
            Message = "Bad Request";
            Description = "";
        }
        public void BadRequest(string message)
        {
            BadRequest();
            this.Message = message;
            Description = "";
        }
        public void UnAuthorized()
        {
            Code = 401;
            Succeeded = false;
            Message = "Unauthorized";
            Description = "";
        }
        public void NotAcceptable()
        {
            Code = 406;
            Succeeded = false;
            Message = "Not Acceptable";
            Description = "";
        }
        public void NotAcceptable(string message)
        {
            NotAcceptable();
            this.Message = message;
            Description = "";
        }
        public void UnAuthorized(string message)
        {
            UnAuthorized();
            this.Message = message;
            Description = "";
        }
        public void NotFound()
        {
            Code = 404;
            Succeeded = false;
            Message = "Not Found";
        }
        public void NotFound(string message)
        {
            NotFound();
            this.Message = message;
            Description = "";
        }
        public void Error(string message, string description)
        {
            Code = 500;
            Succeeded = false;
            this.Message = message;
            this.Description = description;
        }

        public void Forbidden(string message)
        {
            Code = 403;
            Succeeded = false;
            this.Message = message;
            Description = "";
        }

        public string GetErrorMessage()
        {
            return Message + (string.IsNullOrEmpty(Description) ? "" : " = " + Description);
        }

    }
    public class ObjectResponse<T> : StatusResponse
    {
        public ObjectResponse()
        {
            BadRequest();
        }
        public T Data { get; set; }
    }
    public class ListResponse<T> : StatusResponse
    {
        public ListResponse()
        {
            BadRequest();
        }
        public int? Count { get; set; }
        public int Filtered { get; set; }
        public List<T> List { get; set; }
    }
    public class ReferensiStringObject
    {
        public string Id { get; set; }
        public string Nama { get; set; }
    }
    public class ReferensiStringKodeObject : ReferensiStringObject
    {
        public string Kode { get; set; }
    }


    public class ReferensiObject
    {
        public int Id { get; set; }
        public string Nama { get; set; }
    }

    public class ReferensiKodeObject : ReferensiObject
    {
        public string Kode { get; set; }
    }
}
