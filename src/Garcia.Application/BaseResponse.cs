using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Garcia.Domain;

namespace Garcia.Application
{
    public class BaseResponse : IBaseResponse
    {
        private readonly ApiError _error;
        protected int _statusCode => (int)Status;

        public BaseResponse(ApiError error = null)
        {
            _error = error;
        }

        public BaseResponse(HttpStatusCode status, ApiError error = null)
        {
            Status = status;
            _error = error;
        }

        public HttpStatusCode Status { get; set; }
        public bool Success { get => _error is null; }
        public ApiError Error => _error;
        /// <summary>
        /// Http status code that will returned
        /// </summary>
        public int StatusCode => _statusCode == 0 ? (int)HttpStatusCode.OK : _statusCode;
    }

    public class BaseResponse<TModel> : BaseResponse, IBaseResponse<TModel>
    {
        public BaseResponse(TModel model, ApiError error = null) : base(error)
        {
            Result = model;
        }

        public BaseResponse(TModel model, HttpStatusCode status, ApiError error = null) : base(error)
        {
            Result = model;
            Status = status;

        }

        public TModel Result { get; }
    }

    public class BaseResponse<TModel, TError> : BaseResponse<TModel>, IBaseResponse<TModel>
        where TError : ApiError
    {
        public BaseResponse(TModel model, TError error = null) : base(model, error)
        {
        }

        public BaseResponse(TModel model, HttpStatusCode status, TError error = null) : base(model, status, error)
        {
        }
    }
}
