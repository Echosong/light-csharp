using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Light.Common.Dto;
using Light.Common.Dto.Api;
using Light.Common.Enums;
using Light.Common.Filter;
using Light.Entity;
using Light.Service;

namespace Light.Api.Controllers {

    /// <summary>
    /// 用户相关业务控制器
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]

    public class UserController : BaseController {
        private readonly IUserService _userService;
        private readonly ISmsService _msService;
    

        /// <summary>
        /// 构造注入入口
        /// </summary>
        /// <param name="db"></param>
        /// <param name="userService"></param>
        /// <param name="smsService"></param>
        /// <param name="universityService"></param>
        public UserController(Db db, IUserService userService, ISmsService smsService) {
            this._db = db;
            this._userService = userService;
            this._msService = smsService;

        }

        /// <summary>
        /// 获取我的页面统计信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [NoPermission]
        public List<Dictionary<string, string>> My() {

            List<Dictionary<string, string>> resitList = new List<Dictionary<string, string>>();


            var downCount = this._db.FinanceOps.Where(t => t.BusinessType == (int)FinanceTypeEnum.下载 && t.UserId == this._user.Id).Select(t=>t.RelationId)
                .Distinct().Count();
            resitList.Add(new Dictionary<string, string>() {
                {"我的下载", downCount.ToString()}
            });
            var vipCount = this._db.FinanceOps.Count(t => t.BusinessType == (int)FinanceTypeEnum.购买会员 && t.State == (int)FinanceOpStateEnum.成功
                && t.UserId == this._user.Id);

            resitList.Add(new Dictionary<string, string>() {
                {"我的订单", vipCount.ToString()}
            });
            return resitList;
        }

        /// <summary>
        /// 登录
        /// </summary>
        [HttpPost]
        [NoPermission]
        [Log("小程序手机号登录")]
        public string Login(LoginUserApiDto userApiDto) {
            _msService.Verification(userApiDto.Username, userApiDto.Code);
            userApiDto.LoginIp = this.GetIp();
            return _userService.LoginApi(userApiDto);

        }


        /// <summary>
        /// 完善用户资料
        /// </summary>
        [HttpPut]
        public void FillUser(UserInfoApiDto userInfo) {
            Assert.IsTrue(_user.Id != 0, "用户资料不存在");
            _db.Users.Where(t => t.Id == _user.Id)
                .BatchUpdate(t => new User {
                    Name = userInfo.Name,
                    Username = userInfo.Username,
                    Sex = userInfo.Sex,
                    Birthday = userInfo.Birthday
                });
            _db.UserExtends.Where(t => t.UserId == _user.Id)
                .BatchUpdate(t => new UserExtend {
                    Signlogo = userInfo.Signlogo,
                    Avatar = userInfo.Avatar
                });
        }



        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{userId?}")]
        public UserDto GetUser(int? userId) {
            if (userId == null || userId == 0) {
                userId = _user.Id;
            }
            return (from u in _db.Users
                    from e in _db.UserExtends
                    where u.Id == userId && u.Id == e.UserId
                    select new UserDto {
                        Id = u.Id,
                        Username = u.Username,
                        Avatar = e.Avatar,
                        Level = e.Level,
                        HitCount = e.HitCount,
                        Sex = u.Sex,
                        Address = e.Address,
                        Name = u.Name,
                        Balance = e.Balance,
                        Expire = e.Expire,
                        BalanceTotal = e.BalanceTotal,
                        Birthday = u.Birthday,
                        Signlogo = e.Signlogo
                    }).First();
        }

        /// <summary>
        /// 收藏| 取消收藏
        /// </summary>
        [HttpPut]
        public void SetFavorite(int type, int relateId) {
            UserFavorite userFavorite = new UserFavorite {
                UserId = this._user.Id,
                Type = type,
                RelateId = relateId
            };
         

            this._db.SaveChanges();
        }

        /// <summary>
        /// 收藏查询
        /// </summary>
        [HttpGet]
        public UserFavorite? Favorite(int type, int relateId) {
            return _db.UserFavorites.FirstOrDefault(t =>
                t.RelateId == relateId && t.Type == type && t.UserId == this._user.Id);
        }


        /// <summary>
        /// 获取用户自己收藏信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<UserFavorite> GetFavorites() {
            return this._db.UserFavorites.Where(t => t.UserId == this._user.Id).ToList();
        }

        /// <summary>
        /// 收藏量
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public int FavoriteCount(int type, int relateId) {
            if (relateId == 0) {
                return this._db.UserFavorites.Count(t => t.Type == type && t.UserId == this._user.Id);
            } else {
                return this._db.UserFavorites.Count(t => t.Type == type && t.RelateId == relateId);
            }
        }


    }
}
