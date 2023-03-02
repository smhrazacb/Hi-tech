using AutoMapper;
using Customer.API.Entities;
using Customer.API.Entities.Dtos;
using Customer.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Customer.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
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
        [ProducesResponseType(typeof(IEnumerable<User>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<User>>> Users()
        {
            var result = await repository.GetUsers();
            return Ok(result);
        }
        /// <summary>
        /// Returns a User if Id Matched
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet("{guid}", Name = "User")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<User>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<User>>> User(Guid guid)
        {
            var products = await repository.GetUser(guid);
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
        public async Task<ActionResult<User>> Email(string email)
        {
            var users = await repository.GetUser(email);
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
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<User>> User([FromBody] UserDto userdto)
        {
            var user = _mapper.Map<User>(userdto);
            await repository.CreateUser(user);
            return CreatedAtRoute("User", new { guid = user.Id }, user);
        }
        /// <summary>
        /// Update a User
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="userdto"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateUser(Guid guid, [FromBody] UserDto userdto)
        {
            if (userdto == null)
                return BadRequest();
            var userkey = await repository.GetUserKeys(guid);
            if (userkey == null)
                return NotFound();

            var userupdated = _mapper.Map<User>(userdto);
            userupdated.Id = userkey.Id;
            userupdated.AddressId = userkey.AddressId;
            userupdated.Address.ContactId = userkey.ContactId;
            userupdated.Address.GeoDataId = userkey.GeoDataId;
            userupdated.AddedDate = userkey.AddedDate;

            return Ok(await repository.UpdateUser(userupdated));
        }
        /// <summary>
        /// Delete a User
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpDelete("{guid}", Name = "DeleteUser")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteUser(Guid guid)
        {
            var result = await repository.DeleteUser(guid);
            if (result > 0)
                return NoContent();
            else
                return BadRequest();
        }
    }
}
