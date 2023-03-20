using Microsoft.AspNetCore.Mvc;
using Persistence.Repositories;

namespace Api.Controllers
{
    [ApiController]
    [Route("PhoneNumber")]
    public class PhoneNumberController : ControllerBase
    {
        private readonly IPhoneNumberRepository _phoneNumberRepository;
        private readonly IAccountRepository _accountRepository;

        public PhoneNumberController(
            IPhoneNumberRepository phoneNumberRepository,
            IAccountRepository accountRepository)
        {
            _phoneNumberRepository = phoneNumberRepository;
            _accountRepository = accountRepository;
        }

        [HttpPost("{phoneNumber}/{accountId}/CreatePhoneNumber")]
        public async Task<ActionResult> CreatePhoneNumber(string phoneNumber, int accountId)
        {
            if (await _accountRepository.IsActive(accountId))
            {
                await _phoneNumberRepository.CreatePhoneNumber(phoneNumber, accountId);
            }
            return Ok();
        }
    }
}
