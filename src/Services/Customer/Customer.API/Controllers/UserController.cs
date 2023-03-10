using AutoMapper;
using Customer.API.Entities;
using Customer.API.Entities.Dtos;
using Customer.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Customer.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    //[Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository repository;
        private readonly IMapper _mapper;
        public UserController(IUserRepository repository, IMapper mapper)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Returns All User
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ApplicationUser>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> Users()
        {
            var result = await repository.GetUsers();
            return Ok(result);
        }
        /// <summary>
        /// Returns a User if Id Matched
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "User")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<ApplicationUser>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> User(string id)
        {
            var products = await repository.GetUserById(id);
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }
        /// <summary>
        /// Returns a User if email Matched
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Route("[action]/")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApplicationUser>> Email(string email)
        {
            var users = await repository.GetUserByEmail(email);
            if (users == null)
            {
                return NotFound();
            }
            return Ok(users);
        }
        /// <summary>
        /// Create a User
        /// </summary>
        /// <param name="userdto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApplicationUser), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(IdentityResult), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ApplicationUser>> User([FromBody] UserDto userdto)
        {
            var user = _mapper.Map<ApplicationUser>(userdto);
            user.PasswordHash = new PasswordHasher<ApplicationUser>()
                            .HashPassword(user, userdto.Password);
            var result = await repository.CreateUser(user);
            if (!result.Succeeded)
            {
                return Conflict(result);
            }
            return CreatedAtRoute("User", new { id = user.Id }, user);
        }
        /// <summary>
        /// Update a User
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userdto"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserDto userdto)
        {
            if (userdto == null)
                return BadRequest();
            var userkey = await repository.GetUserById(id);
            if (userkey == null)
                return NotFound();

            var userupdated = _mapper.Map<ApplicationUser>(userdto);
            userupdated.Id = userkey.Id;
            userupdated.AddressId = userkey.AddressId;
            userupdated.AddedDate = userkey.AddedDate;

            userkey.Address = userupdated.Address;
            userkey.Email = userupdated.Email;
            userkey.PasswordHash = userupdated.PasswordHash;
            userkey.UserStatus = userupdated.UserStatus;
            userkey.UserName = userupdated.UserName;
            userkey.OrderType = userupdated.OrderType;

            return Ok(await repository.UpdateUser(userkey));
        }
        /// <summary>
        /// Delete a User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}", Name = "DeleteUser")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await repository.DeleteUser(id);
            if (result > 0)
                return NoContent();
            else
                return BadRequest();
        }
    }
}
