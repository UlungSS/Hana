using AutoMapper;
using Hana.Core;
using Hana.Core.Api.Middleware;

using Hana.Users.Contract;
using Hana.Users.Entities;
using Hana.Users.Models;

namespace Hana.Users.Services;

public class UserServices(IUserRepository userRepository, IMapper mapper) : IUserServices
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _userRepository.GetAll();
    }

    public async Task<User> GetById(int id)
    {
        var user = await _userRepository.GetById(id);

        return user! == null ? throw new KeyNotFoundException("User not found") : user;
    }

    public async Task<User> GetByPhoneNumber(string phoneNum)
    {
        var user = await _userRepository.GetByPhoneNumber(phoneNum);

        return user! == null ? throw new KeyNotFoundException("Phone Number not found") : user;
    }

    public async Task<User> GetByEmail(string email)
    {
        var user = await _userRepository.GetByEmail(email);

        return user! == null ? throw new KeyNotFoundException("Email not found") : user;
    }

    public async Task Create(CreateUserRequest model)
    {
        // validate
        if (await _userRepository.GetByPhoneNumber(model.PhoneNumber!) != null)
            throw new AppException("User with the phone number '" + model.PhoneNumber + "' already exists");
        
        if (await _userRepository.GetByEmail(model.Email!) != null)
            throw new AppException("User with the email '" + model.Email + "' already exists");

        // map model to new user object
        var user = _mapper.Map<User>(model);

        // add hash & guid
        user.Guid = new RandomIdentityGenerator(4).GenerateId();
        user.Pass_Hash = BCrypt.Net.BCrypt.HashPassword(model.Password);

        // save user
        await _userRepository.Create(user);
    }

    public async Task Update(int id, UpdateUserRequest model)
    {
        var user = await _userRepository.GetById(id) ?? throw new KeyNotFoundException("User not found");

        // validate
        var phoneNum = !string.IsNullOrEmpty(model.PhoneNumber) && user.PhoneNumber != model.PhoneNumber;
        if (phoneNum && await _userRepository.GetByPhoneNumber(model.PhoneNumber!) != null)
            throw new AppException("User with the phone number '" + model.PhoneNumber + "' already exists");

        var emailChanged = !string.IsNullOrEmpty(model.Email) && user.Email != model.Email;
        if (emailChanged && await _userRepository.GetByEmail(model.Email!) != null)
            throw new AppException("User with the email '" + model.Email + "' already exists");

        // hash password if it was entered
        if (!string.IsNullOrEmpty(model.Password))
            user.Pass_Hash = BCrypt.Net.BCrypt.HashPassword(model.Password);

        // copy model props to user
        _mapper.Map(model, user);

        // save user
        await _userRepository.Update(user);
    }

    public async Task Delete(int id)
    {
        await _userRepository.Delete(id);
    }
}
