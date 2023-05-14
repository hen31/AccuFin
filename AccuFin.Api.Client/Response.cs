using System.Net;

namespace AccuFin.Api.Client
{
    public class Response
    {
        public Response()
        {

        }
        public HttpStatusCode StatusCode { get; set; }
        public bool Success { get; set; } = true;
    }
    public class Response<ResultType> : Response<ResultType, string>
    {
        public Response()
        {

        }
    }

    public class Response<ResultType, ErrorDataType> : Response
    {
        public Response()
        {

        }
        public ResultType Data { get; set; }
        public ErrorDataType ErrorData { get; set; }
    }
}