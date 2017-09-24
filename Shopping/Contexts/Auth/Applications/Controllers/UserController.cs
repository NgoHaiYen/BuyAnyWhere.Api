using System;
using System.Linq;
using System.Web.Http;
using Shopping.Contexts.Auth.Applications.DTOs;
using Shopping.Models;
using System.Data.Entity;
using System.Web.Http.Description;
using System.Collections.Generic;

namespace Shopping.Contexts.Auth.Applications.Controllers
{
    [RoutePrefix("api/Auth/Users")]
    public class UserController : ApiController
    {
        private readonly ShoppingEntities shoppingEntities;

        public UserController(ShoppingEntities shoppingEntities)
        {
            this.shoppingEntities = shoppingEntities;
        }


        /// <summary>  
        ///  Xem danh sách thông tin tất cả các User kèm Role của User đó
        ///, có thể Skip, Take, Order, Filter
        /// </summary>

        [HttpGet]
        [Route("")]
        [ResponseType(typeof(List<UserDto>))]
        public IHttpActionResult Get([FromUri] UserFilterDto userFilterDto)
        {
            if (userFilterDto == null)
            {
                userFilterDto = new UserFilterDto();
            }

            var users = userFilterDto.SkipAndTake(userFilterDto.ApplyTo(shoppingEntities.Users.Include(t => t.Role))).ToList();
            var userDtos = users.ConvertAll(t => new UserDto(t, null, t.Role));

            return Ok(userDtos);
        }


        /// <summary>  
        ///  Xem thông tin User cụ thể khi truyền vào userId
        /// </summary>
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

        /// <summary>  
        ///  Tạo 1 User mới
        /// </summary>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] UserDto userDto)
        {
            var user = userDto.ToModel();
            shoppingEntities.Users.Add(userDto.ToModel());
            shoppingEntities.SaveChanges();

            return Get(user.Id);
        }

        /// <summary>  
        ///  Đếm số lượng User, truyền vào Filter
        /// </summary>
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


        /// <summary>  
        ///  Sửa quyền của User, truyền vào userId và roleId
        /// </summary>
        [HttpPut]
        [Route("{userId}/Role/{roleId}")]
        public IHttpActionResult PutRole([FromUri] Guid userId, [FromUri] Guid roleId)
        {
            var user = shoppingEntities.Users.FirstOrDefault(t => t.Id == userId);
            var role = shoppingEntities.Roles.FirstOrDefault(t => t.Id == roleId);

            if (user == null)
                throw new BadRequestException("ID người dùng không hợp lệ");

            user.Role = role ?? throw new BadRequestException("ID vai trò không hợp lệ");

            shoppingEntities.SaveChanges();
            return Get(userId);
        }
    }
}