using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Senparc.CO2NET.Extensions;
using SM23Crypto;
using Light.Common.Configuration;
using Light.Common.Dto;
using Light.Common.Enums;
using Light.Common.Error;
using Light.Common.Filter;
using Light.Common.RedisCache;
using Light.Entity;
using Light.Service;

namespace Light.Admin.Controllers {
    /// <summary>
    /// 用户表 控制器
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : BaseController {

        private readonly IUserService _userService;
        public UserController(Db db, IUserService userService) {
            this._db = db;
            this._userService = userService;
        }

        /// <summary>
        /// 分页查询 用户表 
        /// </summary>
        /// <param name="queryDto">分页条件</param>
        /// <returns></returns>
        [HttpPut]
        public Page<User> ListPage(UserQueryDto queryDto) {
            var where = PredicateExtend.True<User>();

            if (!string.IsNullOrEmpty(queryDto.Name)) {
                where = where.And(t => t.Name.Contains(queryDto.Name));
            }

            if (!string.IsNullOrEmpty(queryDto.Username)) {
                where = where.And(t => t.Username.Contains(queryDto.Username));
            }
            if (!queryDto.Pusername.IsNullOrEmpty()) {
                var pUser = this._db.Users.Where(t => t.Username == queryDto.Pusername).FirstOrDefault();
                if (pUser != null) {
                    where = where.And(t => t.ParentId == pUser.Id);
                }
            }

            if (queryDto.Type != 0) {
                where = where.And(t => t.Type == queryDto.Type);
            }
            var queryWhere = _db.Users
                .Where(where);


            var query = queryWhere.OrderByDescending(t => t.Id)
                .Skip(queryDto.pageSize * (queryDto.page - 1))
                .Take(queryDto.pageSize);

            var list = query.ToList();
            var parentIds = list.Select(t => t.ParentId).ToList();
            var pUsers = _db.Users.Where(t => parentIds.Contains(t.Id))
                .Select(t => new { Id = t.Id, Name = t.Name, Username = t.Username }).ToList();
            list.ForEach(item => {
                var pUser = pUsers.FirstOrDefault(t => t.Id == item.ParentId);
                if (pUser != null) {
                    item.PUsername = pUser.Username;
                }
            });

            if (list.Count > 0) {
                queryDto.totalElements = queryWhere.Count();
            }
            return new Page<User>(list, queryDto);
        }


        /// <summary>
        /// 分销中心数据获取
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public Page<UserDto> ListSale(UserQueryDto queryDto) {

            //获取当前的用户类型
            var user = _db.Users.Find(this._user.Id);


            var where = PredicateExtend.True<UserDto>();

            if (!string.IsNullOrEmpty(queryDto.Name)) {
                where = where.And(t => t.Name.Contains(queryDto.Name));
            }

            if (!string.IsNullOrEmpty(queryDto.Username)) {
                where = where.And(t => t.Username.Contains(queryDto.Username));
            }
            //处理分销商进来的时候处理
            bool flg = true;
            if (!queryDto.Pusername.IsNullOrEmpty()) {
                var pUser = this._db.Users.FirstOrDefault(t => t.Username == queryDto.Pusername);
                if (pUser != null) {
                    if (user.RoleId == GlobalConsts.USER_ROLEID) {
                        if (pUser.ParentId == user.Id) {
                            where = where.And(t => t.ParentId == pUser.Id);
                            flg = false;
                        }
                    } else {
                        where = where.And(t => t.ParentId == pUser.Id);
                        flg = false;
                    }
                }
            }
            if(flg) {
                if (user.RoleId == GlobalConsts.USER_ROLEID) {
                   where = where.And(t => t.ParentId == user.Id);
                }
            }



            if (queryDto.Level != 0) {
                where = where.And(t => t.Level == queryDto.Level);
            }

            if (queryDto.Type != 0) {
                where = where.And(t => t.Type == queryDto.Type);
            }
            var queryWhere = (from u in _db.Users join e in _db.UserExtends on u.Id equals e.UserId select new UserDto {
                Name = u.Name,
                Username = u.Username,
                ParentId = u.ParentId,
                Type = u.Type,
                Id = u.Id,
                BalanceTotal = e.BalanceTotal,
                Balance = e.Balance,
                Level = e.Level,
                CreateDateTime = u.CreateDateTime,
                UpdateDateTime = u.UpdateDateTime,
                Avatar = e.Avatar,
                Email = u.Email,
                Sex = u.Sex
                
            }).Where(where);


            var query = queryWhere.OrderByDescending(t => t.Id)
                .Skip(queryDto.pageSize * (queryDto.page - 1))
                .Take(queryDto.pageSize);

            var list = query.ToList();
            var parentIds = list.Select(t => t.ParentId).ToList();
            var pUsers = _db.Users.Where(t => parentIds.Contains(t.Id))
                .Select(t => new { Id = t.Id, Name = t.Name, Username = t.Username }).ToList();
            list.ForEach(item => {
                var pUser = pUsers.FirstOrDefault(t => t.Id == item.ParentId);
                if (pUser != null) {
                    item.PUsername = pUser.Username;
                }
            });

            if (list.Count > 0) {
                queryDto.totalElements = queryWhere.Count();
            }
            return new Page<UserDto>(list, queryDto);
        }

        /// <summary>
        ///     获取当前用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public UserDto GetCurrent() {
            return _user;
        }

        /// <summary>
        /// 获取 能够挂载下级的用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<SimpleUser> GetParents() {
            var list = _db.Users.Select(t => new SimpleUser { Id = t.Id, Name = t.Name ?? "", Username = t.Username ?? "", RoleId = t.RoleId, Type = t.Type, TypeEnum = "" })
                .Where(t => t.Type < (int)UserTypeEnum.普通用户)
                .ToList();
            list.ForEach(item => {
                item.TypeEnum = ((UserTypeEnum)item.Type).ToString();
            });
            return list;
        }

        /// <summary>
        /// 获取二维码
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{userId}")]
        public async Task<string> GetQrCode(int userId) {
            if (userId == 0) {
                userId = this._user.Id;
            }

            HttpClient client = new HttpClient();
            string url = AppSettingsConstVars.AppConfigAppInterFaceUrl + $"WxOpen/GetQrCode/{userId}";
            return AppSettingsConstVars.AppConfigAppInterFaceUrl + await client.GetStringAsync(url);
        }

        /// <summary>
        ///     登陆接口
        /// </summary>
        /// <param name="loginUser"></param>
        [HttpPost]
        [Log("用户登陆")]
        [NoPermission]
        public string Login(LoginUserDto loginUser) {
            loginUser.LoginIp = GetIp();
            return _userService.Login(loginUser);
        }

        /// <summary>
        /// 用户表 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpGet]
        [Route("{id?}")]
        public User? Find(int id) {
            return _db.Users.Find(id);
        }



        /// <summary>
        /// 修改密码
        /// </summary>
        [HttpPost]
        public void SetPassword(PasswordSetDto passwordSet) {

            Assert.IsTrue(passwordSet.NewPassword == passwordSet.RePassword, "两次密码输入不一致");
            var find = this._db.Users.Find(_user.Id);
            Assert.NotNull(find, "用户不存在");
            Assert.IsTrue(find.Password == SM3.StrSum(passwordSet.Password), "原密码错误");
            find.Password = SM3.StrSum(passwordSet.NewPassword);
            _db.SaveChanges();
        }

        /// <summary>
        ///     删除用户身份
        /// </summary>
        [HttpGet]
        [Log("用户注销")]
        public void Logout() {
            Redis.CreateInstance().Delete(this.GetToken());
        }

        /// <summary>
        /// 新增活更新用户表
        /// </summary>
        /// <param name="one">用户表</param>
		[HttpPost]
        [Log("新增用户")]
        public void Save(User one) {
            if (one == null) {
                throw new BaseException("参数错误");
            }

            //获取当前的用户类型
            var user = _db.Users.Find(this._user.Id);
            

            if (one.Id != 0) {
                if (one.ParentId != 0) {
                    User pUser = _db.Users.AsNoTracking().First(t => t.Id == one.ParentId);
                    if (pUser == null) {
                        throw new BaseException("上级不存在");
                    }
                }
                //只能修改备注
                if (user.RoleId == GlobalConsts.USER_ROLEID) {
                    var find = _db.Users.Find(one.Id);
                    if (find != null) {
                        find.Email = one.Email;
                    }
                }else {
                    _db.Users.Update(one);
                }
            } else {
                //分销商不允许设置
                if (user.RoleId != GlobalConsts.USER_ROLEID) {
                    _db.Users.Add(one);
                }
            }
            _db.SaveChanges();

        }

        /// <summary>
        ///  密码重置
        /// </summary>
        /// <param name="id"></param>
        [HttpGet]
        [Log("密码重置")]
        public void ResetPassword(int id) {
            _db.Users.Where(t => t.Id == id).BatchUpdate(t => new User {
                Password = SM3.StrSum("123456"),
            });
        }

        /// <summary>
        /// 状态重置
        /// </summary>
        /// <param name="id"></param>
        [HttpGet]
        [Log("重置状态")]
        public void ResetState(int id) {
            var user = this._db.Users.Find(id);
            var state = (int)UserStateEnum.正常;
            if (user.State == (int)UserStateEnum.正常) {
                user.State = (int)UserStateEnum.禁用;
            } else {
                user.State = state;
            }

            this._db.SaveChanges();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">id</param>
        [HttpDelete]
        [Route("{id?}")]
        public void Delete(int id) {
            var find = _db.Users.Find(id);
            if (find == null) {
                throw new BaseException("数据不存在");
            }
            _db.Users.Remove(find);
            _db.SaveChanges();
        }
    }
}