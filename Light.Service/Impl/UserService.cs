using System.Net;
using System.Text;
using Nelibur.ObjectMapper;
using NUnit.Framework;
using Senparc.CO2NET.Extensions;
using SM23Crypto;
using Light.Common.Configuration;
using Light.Common.Dto;
using Light.Common.Dto.Api;
using Light.Common.Enums;
using Light.Common.Error;
using Light.Common.RedisCache;
using Light.Common.Utils;
using Light.Entity;

namespace Light.Service.Impl {
    public class UserService : IUserService {
        private readonly Db _db;


        public UserService(Db db) {
            _db = db;
        }

        /// <summary>
        /// 获取头衔信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<UserHeaderApiDto> Headers(List<int> ids) {

            return (from u in _db.Users
                    from e in _db.UserExtends
                    where u.Id == e.UserId && ids.Contains(u.Id)
                    select new UserHeaderApiDto { Name = u.Name, Avatar = e.Avatar, Id = u.Id, Sex = u.Sex, Level = e.Level })
                            .ToList();
        }

        /// <summary>
        /// 前端api登录
        /// </summary>
        /// <param name="userApiDto"></param>
        /// <returns></returns>
        public string LoginApi(LoginUserApiDto userApiDto) {
            var user = _db.Users.FirstOrDefault(t => t.Username == userApiDto.Username);
            //走注册业务
            if (user == null) {
                user = new User() {
                    Username = userApiDto.Username,
                    LoginIp = userApiDto.LoginIp,
                    State = (int)UserStateEnum.正常,
                    ParentId = userApiDto.ParentId,
                    RegIp = userApiDto.LoginIp,
                    RoleId = 0,
                    Type = (int)UserTypeEnum.普通用户,
                    Sex = (int)BaseSexEnum.男,
                    Name = GetNameHelper.GetManName()
                };
                var userExtend = new UserExtend() {
                    Username = userApiDto.Username,
                    Balance = 0,
                    BalanceTotal = 0,
                    Level = (int)UserLevelEnum.普通会员
                };
                user = Create(user, userExtend);
            } else {
                user.LoginIp = userApiDto.LoginIp;
                if (string.IsNullOrEmpty(user.Name)) {
                    user.Name = GetNameHelper.GetManName();
                }
                if (userApiDto.ParentId != 0 && user.ParentId == 0) {
                    user.ParentId = userApiDto.ParentId;
                }
                _db.SaveChanges();
            }

            var token = Guid.NewGuid().ToString("N");
            UserDto userDto = new UserDto {
                Id = user.Id,
                Username = userApiDto.Username,
                Name = user.Name,
                RoleId = user.RoleId,
            };
            Redis.CreateInstance().SetCache(token, userDto);
            return token;
        }

        /// <summary>
        /// 登录代码逻辑处理
        /// </summary>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        public string Login(LoginUserDto loginUser) {
            byte[] bytes = Convert.FromBase64String(loginUser.Password);
            var password = Encoding.UTF8.GetString(bytes);
            var user = _db.Users.FirstOrDefault(t => t.Username == loginUser.Username);

            Assert.IsTrue(user != null, "账号或者密码错误");

            Assert.IsTrue(user?.Password == SM3.StrSum(password), "密码错误");
            Assert.IsTrue(user?.State == (int?)UserStateEnum.正常, "账号不允许登陆");

            if (user != null) {
                user.LoginIp = loginUser.LoginIp;
                _db.SaveChanges();

                var token = Guid.NewGuid().ToString("N");
                UserDto userDto = new UserDto {
                    Id = user.Id,
                    Username = loginUser.Username,
                    Name = user.Name,
                    RoleId = user.RoleId,
                };
                Redis.CreateInstance().SetCache(token, userDto);
                return token;
            }

            return "";
        }


        /// <summary>
        ///     新增用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userExtend"></param>
        public User Create(User user, UserExtend userExtend) {
            if (user.Password.IsNullOrEmpty()) {
                user.Password = "123456";
            }
            user.Password = SM3.StrSum(user.Password);
            user.RoleId = 2;
            var userDb = _db.Users.FirstOrDefault(t => t.Username == user.Username);
            Assert.IsTrue(userDb == null, "账号已经存在，不能重复添加");

            var roleDb = _db.Roles.FirstOrDefault(t => t.Id == user.RoleId);
            Assert.IsTrue(roleDb != null, "角色不存在");

            _db.Users.Add(user);
            //创建扩展
            _db.SaveChanges();
            _db.Entry(user);

            userExtend.UserId = user.Id;
            userExtend.Username = user.Username;
            _db.UserExtends.Add(userExtend);

            _db.SaveChanges();
            return user;
        }



        /// <summary>
        ///     更新用户
        /// </summary>
        /// <param name="user"></param>
        public void Update(User user) {
            var firstUser = _db.Users.FirstOrDefault(t => t.Id == user.Id);
            if (firstUser != null) {
                firstUser.Name = user.Name;
                firstUser.RoleId = user.RoleId;
                firstUser.Email = user.Email;
                firstUser.Sex = user.Sex;
                firstUser.Lead = user.Lead;
                if (!string.IsNullOrEmpty(user.Password)) firstUser.Password = SM3.StrSum(user.Password);
                _db.SaveChanges();
            }
        }

        /// <summary>
        /// 检查是否为vip
        /// </summary>
        /// <param name="userId"></param>
        /// <exception cref="BaseException"></exception>
        public void CheckVip(int userId) {
            var userExtend = _db.UserExtends.FirstOrDefault(t => t.Level == (int)UserLevelEnum.VIP会员 && t.UserId == userId);
            if (userExtend == null) {
                throw new BaseException(HttpStatusCode.NetworkAuthenticationRequired, "非VIP会员");
            }
        }

        /// <summary>
        /// 微信小程序授权登录
        /// </summary>
        /// <param name="userDto"></param>
        public UserDto WxLogin(UserDto userDto) {
            TinyMapper.Bind<UserDto, User>();
            User user = TinyMapper.Map<User>(userDto);

            var userDb = _db.Users.FirstOrDefault(t => t.OpenId == user.OpenId);
            TinyMapper.Bind<User, UserDto>();

            if (userDb == null) {
                //注册
                user.Password = userDto.OpenId;
                user.RoleId = GlobalConsts.NORMAL_ROLEID;
                var userExtend = new UserExtend {
                    UnionId = userDto.Unionid,
                    Avatar = userDto.Avatar
                };
                //如果是公众号先授权的那么这里就可以处理
                var wei = _db.WeiTemps.FirstOrDefault(w => w.UnionId == userDto.Unionid);
                if (wei != null) {
                    userExtend.OpenId = wei.OpenId;
                }
                user = Create(user, userExtend);
                UserDto userDto1 = TinyMapper.Map<UserDto>(user);
                userDto1.Avatar = userDto.Avatar;
                userDto1.Level = userDto.Level;
                return userDto1;
            } else {
                userDb.LoginIp = userDto.LoginIp;
                var userExtend = _db.UserExtends.FirstOrDefault(t => t.UserId == userDb.Id);
                _db.SaveChanges();
                UserDto userDto1 = TinyMapper.Map<UserDto>(userDb);
                userDto1.Avatar = userDto.Avatar;
                userDto1.Level = userExtend?.Level;
                return userDto1;
            }


        }
    }
}
