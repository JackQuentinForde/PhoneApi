using Microsoft.AspNetCore.Mvc;
using Persistence.Repositories;

namespace PhoneApi.Controllers
{
    [ApiController]
    [Route("Account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(
            IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpPost("{name}/CreateAccount")]
        public async Task<ActionResult> CreateAccount(string name)
        {
            await _accountRepository.CreateAccount(name);
            return Ok();
        }
    }
}
