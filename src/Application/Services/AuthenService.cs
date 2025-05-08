using EXAM_SYSTEM_API.Application.Interfaces;
using EXAM_SYSTEM_API.Application.Shared.Responses;
using EXAM_SYSTEM_API.Domain.CustomRequest;
using EXAM_SYSTEM_API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EXAM_SYSTEM_API.Application.Services
{
    public class AuthenService : IAuthenService
    {
        private readonly IServiceFactory _serviceFactory;
        private readonly IGenericCrudService<User> _userService;

        public AuthenService(IServiceFactory serviceFactory, IGenericCrudService<User> userService)
        {
            _serviceFactory = serviceFactory;
            _userService = userService;
        }

        public async Task<ResponseData> Login(LoginRequest loginRequestDTO)
        {
            // ดึงข้อมูลผู้ใช้จาก Database
            var user = await _serviceFactory.GetService<User>()
                .GetAll()
                .FirstOrDefaultAsync(u => u.Email == loginRequestDTO.Email);

            if (user == null)
            {
                return new ResponseData(401, false, "Invalid email or password.");
            }

            // ตรวจสอบ Password Hash
            if (!BCrypt.Net.BCrypt.Verify(loginRequestDTO.Password, user.PasswordHash))
            {
                return new ResponseData(401, false, "Invalid email or password.");
            }

            // สร้าง Token (ในที่นี้เป็น mock token)
            var token = $"fake-jwt-token-for-{user.Email}";

            return new ResponseData(200, true, "Login successful", new
            {
                Token = token,
                UserId = user.UserId,
                Email = user.Email,
                FullName = user.FullName
            });
        }

        public async Task<ResponseData> Register(RegisterationRequest registerationRequestDTO)
        {
            // ตรวจสอบว่า Email ซ้ำหรือไม่
            var existingUser = await _serviceFactory.GetService<User>()
                .GetAll()
                .AnyAsync(u => u.Email == registerationRequestDTO.Email);

            if (existingUser)
            {
                return new ResponseData(409, false, "Email is already registered.");
            }

            // สร้าง Hash ของ Password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerationRequestDTO.Password);

            // สร้าง User ใหม่
            var newUser = new User
            {
                Username = registerationRequestDTO.Username,
                FullName = registerationRequestDTO.FullName,
                Email = registerationRequestDTO.Email,
                PasswordHash = hashedPassword,
                CreatedAt = DateTime.UtcNow
            };

            // บันทึกลง Database
            var response = await _userService.AddAsync(newUser);

            return new ResponseData(201, true, "User registered successfully.");
        }
    }
}
