using System.Collections.Generic;
using ToDoApi.Data.Dtos;
using ToDoApi.Data.Entities;

namespace ToDoApi.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }
}