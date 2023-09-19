using Domain.DTOs.Requests;
using Domain.DTOs.Responses;
using Domain.Exceptions;
using Domain.Mappers;
using Domain.Models;
using Domain.Ports.Driving;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Adapter.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserMapper _userMapper;

        private readonly IValidator<AddUserDTO> _addUserValidator;
        private readonly IValidator<DeleteUserByIdDTO> _deleteUserByIdValidator;
        private readonly IValidator<DeleteUserByLoginDTO> _deleteUserByLoginValidator;
        private readonly IValidator<GetUserByIdDTO> _getUserByIdValidator;
        private readonly IValidator<GetUserByLoginDTO> _getUserByLoginValidator;

        public UserController(IUserService userService, IUserMapper userMapper, IValidator<AddUserDTO> addUserValidator,
            IValidator<DeleteUserByIdDTO> deleteUserByIdValidator, IValidator<DeleteUserByLoginDTO> deleteUserByLoginValidator,
            IValidator<GetUserByIdDTO> getUserByIdValidator, IValidator<GetUserByLoginDTO> getUserByLoginValidator)
        {
            _userService = userService;
            _userMapper = userMapper;
            _addUserValidator = addUserValidator;
            _deleteUserByIdValidator = deleteUserByIdValidator;
            _deleteUserByLoginValidator = deleteUserByLoginValidator;
            _getUserByIdValidator = getUserByIdValidator;
            _getUserByLoginValidator = getUserByLoginValidator;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> Get()
        {
            var allUsers = await _userService.GetAllUsers();

            var usersDTOs = allUsers.Select(c => _userMapper.MapToUserDTO(c));

            return Ok(usersDTOs);
        }

        [HttpGet("get-by-id")]
        public async Task<ActionResult<UserDTO>> Get(GetUserByIdDTO dto)
        {
            var validationResult = _getUserByIdValidator.Validate(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            try
            {
                var userById = await _userService.GetUserById(dto.Id);

                var userDTO = _userMapper.MapToUserDTO(userById);

                return Ok(userDTO);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex);
            }
        }
        [HttpGet("get-by-login")]
        public async Task<ActionResult<UserDTO>> Get(GetUserByLoginDTO dto)
        {
            var validationResult = _getUserByLoginValidator.Validate(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            try
            {
                var userByLogin = await _userService.GetUserByLogin(dto.Login);

                var userDTO = _userMapper.MapToUserDTO(userByLogin);

                return Ok(userDTO);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex);
            }
        }

        [HttpDelete("delete-by-id")]
        public async Task<ActionResult<UserDTO>> Delete(DeleteUserByIdDTO dto)
        {
            var validationResult = _deleteUserByIdValidator.Validate(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            try
            {
                var deletedUserById = await _userService.DeleteUserById(dto.Id);

                var userDTO = _userMapper.MapToUserDTO(deletedUserById);

                return Ok(userDTO);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex);
            }
        }
        [HttpDelete("delete-by-login")]
        public async Task<ActionResult<UserDTO>> Delete(DeleteUserByLoginDTO dto)
        {
            var validationResult = _deleteUserByLoginValidator.Validate(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            try
            {
                var deletedUserByLogin = await _userService.DeleteUserByLogin(dto.Login);

                var userDTO = _userMapper.MapToUserDTO(deletedUserByLogin);

                return Ok(userDTO);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex);
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult<UserDTO>> Add([FromBody] AddUserDTO dto)
        {
            var validationResult = _addUserValidator.Validate(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            try
            {
                var addedUser = await _userService.AddUser(dto.Login, dto.Password, dto.Group);

                var userDTO = _userMapper.MapToUserDTO(addedUser);

                return Ok(userDTO);
            }
            catch (UserLoginAlreadyExistException ex)
            {
                return Conflict(ex);
            }
            catch (UserAdminAlreadyExistException ex)
            {
                return Conflict(ex);
            }
            catch (UserGroupNotFoundException ex)
            {
                return Conflict(ex);
            }
        }
    }
}
