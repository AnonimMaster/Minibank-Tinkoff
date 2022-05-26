using Minibank.Core.Domains.Users.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using Minibank.Core.Domains.BankAccounts.Repositories;

namespace Minibank.Core.Domains.Users.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IValidator<User> _userValidator;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userRepository, IBankAccountRepository bankAccountRepository, IValidator<User> userValidator, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _bankAccountRepository = bankAccountRepository;
            _userValidator = userValidator;
            _unitOfWork = unitOfWork;
        }

        public Task<User> GetUser(string id)
        {
            return _userRepository.GetUser(id);
        }

        public async Task CreateUser(User user)
        {
            await _userValidator.ValidateAndThrowAsync(user);
            await _userRepository.CreateUser(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public Task<List<User>> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public async Task UpdateUser(User user)
        {
            await _userValidator.ValidateAndThrowAsync(user);
            await _userRepository.UpdateUser(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteUser(string id)
        {
            var isUserExists = await _userRepository.IsUserExists(id);

            if (!isUserExists)
            {
                throw new ValidationException($"Пользователя с Id = {id} нет.");
            }

            if (await _bankAccountRepository.IsLinkedBankAccount(id))
            {
                throw new ValidationException($"Нельзя удалить пользователя с Id = {id}, так как имеются связанные банковские аккаунты.");
            }

            await _userRepository.DeleteUser(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}