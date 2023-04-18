using AutoMapper;
using FluentResults;
using FoodPool.user.dtos;
using FoodPool.user.entities;
using FoodPool.user.interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FoodPool.user;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<Result> Create(CreateUserDto createUserDto)
    {
        try
        {
            if (_userRepository.Exist(createUserDto.Username!)) return Result.Fail(new Error("409"));
            var user = _mapper.Map<User>(createUserDto);
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _userRepository.Insert(user);
            _userRepository.Save();
            return Result.Ok();
        }
        catch (Exception)
        {
            return Result.Fail(new Error("400"));
        }
    }

    public async Task<Result<List<GetUserDto>>> GetAll()
    {
        var users = await _userRepository.GetAll();
        return Result.Ok(users.Select(user => _mapper.Map<GetUserDto>(user)).ToList());
    }

    public async Task<Result<GetUserDto>> GetById(int id)
    {
        var user = await _userRepository.GetById(id);
        return Result.Ok(_mapper.Map<GetUserDto>(user));
    }

    public async Task<Result<GetUserDto>> GetCurrent(int id)
    {
        if (!_userRepository.ExistById(id)) return Result.Fail(new Error("404"));
        var user = await _userRepository.GetById(id);
        return Result.Ok(_mapper.Map<GetUserDto>(user));
    }

    public async Task<Result<GetUserDto>> Update(UpdateUserDto updateUserDto, int id)
    {
        try
        {
            if (!_userRepository.ExistById(id)) return Result.Fail(new Error("404"));
            updateUserDto.Password = BCrypt.Net.BCrypt.HashPassword(updateUserDto.Password);
            _userRepository.Update(updateUserDto, id);
            _userRepository.Save();
            var user = await GetById(id);
            return Result.Ok(user.Value);
        }
        catch (Exception)
        {
            return Result.Fail(new Error("400"));
        }
    }

    public Task<Result> Delete(int id)
    {
        if (!_userRepository.ExistById(id)) return Task.FromResult(Result.Fail(new Error("404")));
        _userRepository.Delete(id);
        _userRepository.Save();
        return Task.FromResult(Result.Ok());
    }
}