﻿using AutoMapper;
using Garcia.Application.Contracts.Identity;
using Garcia.Application.Contracts.Infrastructure;
using Garcia.Application.Contracts.Persistence;
using Garcia.Application.Identity.Models.Request;
using Garcia.Application.Identity.Models.Response;
using Garcia.Domain;
using Garcia.Domain.Identity;

namespace Garcia.Application.Identity.Services
{
    public class AuthenticationService<TRepository, TUser, TUserDto, TKey> : IAuthenticationService<TUser, TUserDto, TKey>
        where TRepository : IAsyncRepository<TUser, TKey>
        where TKey : IEquatable<TKey>
        where TUser : class, IUserEntity<TKey>
        where TUserDto : class, IUser
    {
        protected TRepository Repository { get; }
        protected IEncryption Encryption { get; }
        protected IMapper Mapper { get; }
        protected IJwtService Jwt { get; }


        public AuthenticationService(TRepository repository, IEncryption encryption, IJwtService jwt)
        {
            Jwt = jwt;
            Repository = repository;
            Encryption = encryption;
            Mapper = InitializeMapper()
                .CreateMapper();
        }

        public virtual async Task<BaseResponse<TUserDto>> ValidateUser(Credentials request)
        {
            request.Password = Encryption.CreateOneWayHash(request.Password);
            var user = (await Repository.GetAsync(x => x.Username == request.Username && x.Password == request.Password))
                .FirstOrDefault();

            if (user == null)
            {
                return new BaseResponse<TUserDto>(default, System.Net.HttpStatusCode.NotFound,
                    new ApiError("User not found", "Requested user credentials doesn't match any record"));
            }

            var result = Mapper.Map<TUserDto>(user);
            return new BaseResponse<TUserDto>(result);
        }

        public virtual async Task<BaseResponse<LoginResponse<TUserDto>>> Login(Credentials request, string ip)
        {
            request.Password = Encryption.CreateOneWayHash(request.Password);
            var user = (await Repository.GetAsync(x => x.Username == request.Username && x.Password == request.Password))
                .FirstOrDefault();

            if (user == null)
            {
                return new BaseResponse<LoginResponse<TUserDto>>(default, System.Net.HttpStatusCode.Unauthorized,
                    new ApiError("Username or Password is incorrect", ""));
            }

            var token = await Jwt.GenerateJwt(user.Username, user.Id.ToString()!, user.Roles);

            var result = new LoginResponse<TUserDto>
            {
                User = Mapper.Map<TUserDto>(user),
                TokenInfo = token
            };

            return new BaseResponse<LoginResponse<TUserDto>>(result);
        }

        public virtual async Task<BaseResponse<LoginResponse<TUserDto>>> Signup<TRequest>(TRequest request, string ip)
            where TRequest : Credentials
        {
            var user = (await Repository.GetAsync(x => x.Username == request.Username)).FirstOrDefault();

            if (user != null)
            {
                return new BaseResponse<LoginResponse<TUserDto>>(default, System.Net.HttpStatusCode.Conflict,
                    new ApiError("The username has already been taken.", ""));
            }

            request.Password = Encryption.CreateOneWayHash(request.Password);
            user = Helpers.BasicMap<TUser, TRequest>(request);
            await Repository.AddAsync(user);

            var token = await Jwt.GenerateJwt(user.Username, user.Id.ToString()!, user.Roles);

            var result = new LoginResponse<TUserDto>
            {
                User = Mapper.Map<TUserDto>(user),
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

    public class AuthenticationService<TRepository, TUser, TUserDto> : AuthenticationService<TRepository, TUser, TUserDto, long>, IAuthenticationService<TUser, TUserDto>
        where TRepository : IAsyncRepository<TUser, long>
        where TUser : class, IUserEntity<long>
        where TUserDto : class, IUser
    {
        public AuthenticationService(TRepository repository, IEncryption encryption, IJwtService jwt) : base(repository, encryption, jwt)
        {
        }
    }

    public class AuthenticationService<TUser, TUserDto> : AuthenticationService<IAsyncRepository<TUser>, TUser, TUserDto>, IAuthenticationService<TUser, TUserDto>
    where TUser : Entity<long>, IUserEntity<long>
    where TUserDto : class, IUser
    {
        public AuthenticationService(IAsyncRepository<TUser> repository, IEncryption encryption, IJwtService jwt) : base(repository, encryption, jwt)
        {
        }
    }
}
