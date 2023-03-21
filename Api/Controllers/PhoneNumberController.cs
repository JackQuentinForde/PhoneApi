using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repositories;

namespace Api.Controllers
{
    [ApiController]
    [Route("PhoneNumber")]
    [Authorize("DefaultPolicy")]
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

        [HttpDelete("{id}/DeletePhoneNumber")]
        public async Task<ActionResult> DeletePhoneNumber(int id)
        {
            await _phoneNumberRepository.DeletePhoneNumber(id);
            return Ok();
        }

        [HttpGet("{id}/GetPhoneNumber")]
        public async Task<ActionResult> GetPhoneNumber(int id)
        {
            return Ok(await _phoneNumberRepository.GetPhoneNumber(id));
        }

        [HttpGet("{accountId}/GetAll")]
        public async Task<ActionResult> GetAll(int accountId)
        {
            return Ok(await _phoneNumberRepository.GetAll(accountId));
        }
    }
}
