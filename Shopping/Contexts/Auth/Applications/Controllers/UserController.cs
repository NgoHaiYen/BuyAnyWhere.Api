using System;
using System.Linq;
using System.Web.Http;
using Shopping.Contexts.Auth.Applications.DTOs;
using Shopping.Models;
using System.Data.Entity;
using System.Threading.Tasks;
using Shopping.Contexts.Auth.Applications.Interfaces;
using System.Web;
using Shopping.Contexts.Procurement.Applications.Interfaces;

namespace Shopping.Contexts.Auth.Applications.Controllers
{
    [RoutePrefix("api/Auth/Users")]
    public class UserController : ApiController
    {
        private readonly ShoppingEntities shoppingEntities;

        private readonly IUltilityService ultilityService;

        private readonly INotificationService notificationService;

        public UserController(ShoppingEntities shoppingEntities, IUltilityService ultilityService
            , INotificationService notificationService)
        {
            this.shoppingEntities = shoppingEntities;
            this.notificationService = notificationService;
            this.ultilityService = ultilityService;
        }
        
        [HttpPut]
        [Route("current/CloundToken")]
        public IHttpActionResult PostCloudToken([FromBody] string cloudToken)
        {
            var token = ultilityService.GetHeaderToken(HttpContext.Current);

            var userToken = shoppingEntities.UserTokens.FirstOrDefault(t => t.Name == token);

            if (userToken == null)
            {
                throw new BadRequestException("Access token khong hop le");
            }

            var user = userToken.User;
            user.CloudToken = cloudToken;
            shoppingEntities.SaveChanges();

            notificationService.Notify("Hello", "World", cloudToken);
            return Ok(new UserDto(user));
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAsync([FromUri] UserFilterDto userFilterDto)
        {
            if (userFilterDto == null)
            {
                userFilterDto = new UserFilterDto();
            }

            var users = await userFilterDto.SkipAndTake(userFilterDto.ApplyTo(shoppingEntities.Users.AsNoTracking().Include(t => t.Role))).ToListAsync();
            var userDtos = users.ConvertAll(t => new UserDto(t));

            return Ok(userDtos);
        }

        [HttpPut]
        [Route("{userId}/Notifications")]
        IHttpActionResult PutNotification([FromUri]Guid userId, [FromUri] bool state)
        {
            var user = shoppingEntities.Users.FirstOrDefault(t => t.Id == userId);

            if (user == null)
            {
                throw new BadRequestException("Không tồn tại User");
            }

            user.Notification = state;
            
            shoppingEntities.SaveChanges();
            return Ok(new UserDto(user));
        }

        [HttpPut]
        [Route("current")]
        public IHttpActionResult PutCurrentUser([FromBody] UserDto userDto)
        {
            var token = ultilityService.GetHeaderToken(HttpContext.Current);

            var userToken = shoppingEntities.UserTokens.FirstOrDefault(t => t.Name == token);

            if (userToken == null)
            {
                throw new BadRequestException("Access token khong hop le");
            }

            var user = userToken.User;
            user.Name = userDto.Name;
            user.Gender = userDto.Gender;
            user.AvatarUrl = userDto.AvatarUrl;

            shoppingEntities.SaveChanges();

            return Ok(new UserDto(user));
        }



        [HttpGet]
        [Route("{userId}")]
        public IHttpActionResult Get([FromUri] Guid userId)
        {
            var user = shoppingEntities.Users.Include(t => t.Role).FirstOrDefault(t => t.Id == userId);

            if (user == null)
                throw new BadRequestException("Không tìm thấy User này");

            var userDto = new UserDto(user);

            return Ok(userDto);
        }


        [HttpGet]
        [Route("Counter")]
        public IHttpActionResult Count([FromUri] UserFilterDto userFilterDto)
        {
            if (userFilterDto == null)
            {
                userFilterDto = new UserFilterDto();
            }

            var count = userFilterDto.ApplyTo(shoppingEntities.Users).Count();
            return Ok(count);
        }

       
        [HttpPut]
        [Route("{userId}/Role/{roleId}")]
        public IHttpActionResult PutRole([FromUri] Guid userId, [FromUri] Guid roleId)
        {
            var user = shoppingEntities.Users.FirstOrDefault(t => t.Id == userId);
            var role = shoppingEntities.Roles.FirstOrDefault(t => t.Id == roleId);

            if (user == null)
                throw new BadRequestException("ID người dùng không hợp lệ");

            if (role == null)
                throw new BadRequestException("ID role không hợp lệ");

            user.RoleId = roleId;

            shoppingEntities.SaveChanges();
            return Ok(new UserDto(user));
        }
    }
}