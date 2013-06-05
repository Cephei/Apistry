
namespace Apistry.Samples.Service.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Runtime.Caching;
    using System.Web.Http;

    using Apistry.Samples.Application.Dto;
    using Apistry.Samples.Service.Api.Patching;
    using NContext.Common;
    using NContext.Extensions;
    using NContext.Extensions.AspNetWebApi.Extensions;

    /// <summary>
    /// Defines a crude representation of a well documented ApiController.
    /// </summary>
    public class UsersController : ApiController
    {
        public HttpResponseMessage GetUsers()
        {
            return GetUsersFromCache()
                .ToHttpResponseMessage(Request, HttpStatusCode.OK);
        }

        public HttpResponseMessage GetUser(Int32 userId)
        {
            return GetUsersFromCache()
                .Bind(users => GetUserById(userId))
                .ToHttpResponseMessage(Request, HttpStatusCode.OK);
        }

        public HttpResponseMessage PatchUser(Int32 userId, PatchRequest<UserDto> patchRequest)
        {
            return GetUsersFromCache()
                .Bind(users => GetUserById(userId)
                    .Bind(user => patchRequest.Patch(user))
                    .Let(patchResult => UpdateUserCollection(users)))
                .ToHttpResponseMessage(Request, HttpStatusCode.NoContent);
        }

        public HttpResponseMessage PostUser(UserDto userDto)
        {
            return GetUsersFromCache()
                .Fmap(users =>
                    {
                        var userList = users.ToList();
                        userDto.Id = userList.Count + 1;
                        userList.Add(userDto);

                        return userList;
                    })
                .Let(users => UpdateUserCollection(users))
                .Fmap(_ => userDto)
                .ToHttpResponseMessage(Request, HttpStatusCode.Created);
        }

        public HttpResponseMessage DeleteUser(Int32 userId)
        {
            return GetUsersFromCache()
                .Bind(users => GetUserById(userId)
                    .Fmap(user =>
                        {
                            var userList = users.ToList();
                            userList.Remove(user);

                            return userList;
                        })
                    .Let(_ => UpdateUserCollection(users)))
                .ToHttpResponseMessage(Request, (users, response) =>
                    {
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.Content = null;
                    });
        }

        private IResponseTransferObject<IEnumerable<UserDto>> GetUsersFromCache()
        {
            return new ServiceResponse<IEnumerable<UserDto>>(
                MemoryCache.Default.Get<IEnumerable<UserDto>>("users") ?? Enumerable.Empty<UserDto>());
        }

        private IResponseTransferObject<UserDto> GetUserById(Int32 userId)
        {
            return GetUsersFromCache()
                .Bind(users =>
                    {
                        var user = users.SingleOrDefault(u => u.Id.Equals(userId));
                        if (user == null)
                        {
                            return new ServiceResponse<UserDto>(new Error("UserNotFound", new[] {"User not found."}, "404"));
                        }

                        return new ServiceResponse<UserDto>(user);
                    });
        }

        private void UpdateUserCollection(IEnumerable<UserDto> users)
        {
            MemoryCache.Default.Set("users", users, ObjectCache.InfiniteAbsoluteExpiration);
        }
    }
}