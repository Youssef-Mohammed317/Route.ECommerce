using E_Commerce.Shared.Common;
using E_Commerce.Shared.DTOs.AuthDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Service.Abstraction.Interfaces
{
    public interface IAuthService
    {
        Task<Result<bool>> CheckEmailExist(string email);
        Task<Result<UserDTO>> GetUserByEmail(string email);
        Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO);
        Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO);
    }
}
