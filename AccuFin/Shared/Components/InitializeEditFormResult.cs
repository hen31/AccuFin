using AccuFin.Api.Client;

namespace AccuFin.Shared.Components
{
    public class InitializeEditFormResult<T>
    {

        public InitializeEditFormResult(T model, Response<T> response)
        {
            Model = model;
            Response = response;
        }
        public T Model { get; set; }
        public Response<T> Response { get; set; }
    }
}
