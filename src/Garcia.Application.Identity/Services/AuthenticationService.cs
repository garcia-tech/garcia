using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Garcia.Application.Contracts.Identity;
using Garcia.Application.Contracts.Persistence;
using Garcia.Application.Identity.Models.Request;
using Garcia.Domain.Identity;
using AutoMapper;
using Garcia.Application.Contracts.Infrastructure;
using Garcia.Application.Identity.Models.Response;

namespace Garcia.Application.Identity.Services
{
    public class AuthenticationService<TRepository, TUser, TUserDto, TKey> : IAuthenticationService<TUser, TUserDto, TKey>
        where TRepository : IAsyncRepository<TUser, TKey>
        where TKey : IEquatable<TKey>
        where TUser : User<TKey>
        where TUserDto : IUser
    {
        private readonly TRepository _repository;
        private readonly IEncryption _encryption;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwt;


        public AuthenticationService(TRepository repository, IEncryption encryption, IJwtService jwt)
        {
            _repository = repository;
            _encryption = encryption;
            _mapper = InitializeMapper()
                .CreateMapper();
            _jwt = jwt;
        }

        public virtual async Task<BaseResponse<TUserDto>> ValidateUser(Credentials request)
        {
            request.Password = _encryption.CreateOneWayHash(request.Password);
            var user = (await _repository.GetAsync(x => x.Username == request.Username && x.Password == request.Password))
                .FirstOrDefault();

            if(user == null)
            {
                return new BaseResponse<TUserDto>(default, System.Net.HttpStatusCode.NotFound,
                    new ApiError("User not found", "Requested user credentials doesn't match any record"));
            }

            var result = _mapper.Map<TUserDto>(user);
            return new BaseResponse<TUserDto>(result);
        }

        public virtual async Task<BaseResponse<LoginResponse<TUserDto>>> Login(Credentials request, string ip)
        {
            request.Password = _encryption.CreateOneWayHash(request.Password);
            var user = (await _repository.GetAsync(x => x.Username == request.Username && x.Password == request.Password))
                .FirstOrDefault();

            if (user == null)
            {
                return new BaseResponse<LoginResponse<TUserDto>>(default, System.Net.HttpStatusCode.Unauthorized,
                    new ApiError("Username or Password is incorrect", ""));
            }

            var token = await _jwt.GenerateJwt(user.Username, user.Id.ToString()!, user.Roles);

            var result = new LoginResponse<TUserDto>
            {
                User = _mapper.Map<TUserDto>(user),
                TokenInfo = token
            };

            return new BaseResponse<LoginResponse<TUserDto>>(result);
        }

        protected virtual MapperConfiguration InitializeMapper() =>
            new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TUser, TUserDto>()
                   .ReverseMap();
            });
    }
}
