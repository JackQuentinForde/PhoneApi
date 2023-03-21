using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repositories;

namespace Api.Controllers
{
    [ApiController]
    [Route("Account")]
    [Authorize("DefaultPolicy")]
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

        [HttpPost("{id}/{active}/SetActive")]
        public async Task<ActionResult> SetActive(int id, bool active)
        {
            await _accountRepository.SetActive(id, active);
            return Ok();
        }
    }
}
