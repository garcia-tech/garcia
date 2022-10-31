using System.Net;

namespace Garcia.Domain
{
    public interface IBaseResponse
    {
        /// <summary>
        /// Status code that the API will return
        /// </summary>
        HttpStatusCode Status { get; set; }
        /// <summary>
        /// Indicates whether the response was successful
        /// </summary>
        bool Success { get; }
    }

    public interface IBaseResponse<T> : IBaseResponse
    {
        /// <summary>
        /// The data that the API will return
        /// </summary>
        T Result { get; }
    }
}
