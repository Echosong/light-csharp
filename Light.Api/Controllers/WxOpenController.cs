using Microsoft.AspNetCore.Mvc;
using NewLife.Serialization;
using Senparc.Weixin;
using Senparc.Weixin.WxOpen.AdvancedAPIs.Sns;
using Senparc.Weixin.WxOpen.AdvancedAPIs.WxApp;
using Senparc.Weixin.WxOpen.Containers;
using Light.Common.Configuration;
using Light.Common.Dto;
using Light.Common.Error;
using Light.Common.Filter;
using Light.Common.RedisCache;
using Light.Common.Utils;
using Light.Entity;
using Light.Service;

namespace Light.Api.Controllers {
    /// <summary>
    /// 微信小程序 控制器
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class WxOpenController : BaseController {
        private readonly IUserService _userService;

        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="db"></param>
        /// <param name="userService"></param>
        public WxOpenController(Db db, IUserService userService) {
            _userService = userService;
            _db = db;
            //全局注册微信相关
            AccessTokenContainer.RegisterAsync(AppSettingsConstVars.WxOpenAppId, AppSettingsConstVars.WxOpenAppSecret);
        }

        /// <summary>
        /// 小程序获取手机号码
        /// </summary>
        /// <param name="code"></param>
        ///<param name="openId"></param>
        [Route("{code}")]
        [HttpGet]
        [NoPermission]
        public async Task<UserDto> GetMobile(string code, string openId) {

            try {

                var result = await BusinessApi.GetUserPhoneNumberAsync(AppSettingsConstVars.WxOpenAppId, code);
                var userDto = Redis.CreateInstance().GetCache<UserDto>(openId);
                userDto.Username = result.phone_info.phoneNumber;
                if (userDto == null) {
                    throw new BaseException(System.Net.HttpStatusCode.Ambiguous, "没有授权");
                }
                var token = Guid.NewGuid().ToString();
                var resultDto = _userService.WxLogin(userDto);
                resultDto.OpenId = userDto.OpenId;
                resultDto.Unionid = userDto.Unionid;
                resultDto.Code = token;
                Redis.CreateInstance().SetCache<UserDto>(token, resultDto, TimeSpan.FromDays(1));
                return resultDto;

            } catch (Exception ex) {
                throw new BaseException(ex.Message);
            }
        }

        
        /// <summary>
        /// 获取二维码
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{userId}")]
        [IgnoreResult]
        [NoPermission]
        public string GetQrCode(int userId) {
            var user = _db.Users.Find(userId);
            var ms = new MemoryStream();
            var openId = user.OpenId;
            var page = "pages/index";
            var scene = $"{userId}";
            var data = new {
                scene = scene
            };

            string accessToken = AccessTokenContainer.GetAccessToken(AppSettingsConstVars.WxOpenAppId);
            string url = "https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token=" + accessToken;
            return FunctionUtil.PostHttpDownLoad(url, data.ToJson(), userId+".png");

        }

        /// <summary>
        /// 小程序登录
        ///  使用openId 查询存在就直接登录成功，不存在就直接下一步
        /// </summary>
        /// <param name="userDto"></param>
        [Route("{code}")]
        [HttpPost]
        [NoPermission]
        public UserDto Login(UserDto userDto, string code) {
            userDto.Code = code;
            try {
                var jsonResult = SnsApi.JsCode2Json(AppSettingsConstVars.WxOpenAppId, AppSettingsConstVars.WxOpenAppSecret, code);
                if (jsonResult.errcode == ReturnCode.请求成功) {
                    var sessionBag = SessionContainer.UpdateSession(null, jsonResult.openid, jsonResult.session_key, jsonResult.unionid);
                    userDto.OpenId = jsonResult.openid;
                    userDto.Unionid = jsonResult.unionid;
                    userDto.RegIp = this.GetIp();
                    userDto.LoginIp = this.GetIp();

                    //处理登录信息
                    var token = Guid.NewGuid().ToString();
                    //如果不存在用户先暂存然后等手机号码过来一起处理
                    var user = _db.Users.FirstOrDefault(t => t.OpenId == userDto.OpenId);
                    if (user != null) {
                        var resultDto = _userService.WxLogin(userDto);
                        resultDto.OpenId = jsonResult.openid;
                        resultDto.Unionid = jsonResult.unionid;
                        Redis.CreateInstance().SetCache<UserDto>(token, resultDto, TimeSpan.FromDays(1));
                        UserExtend userExtend = _db.UserExtends.FirstOrDefault(t => t.UserId == user.Id);
                        resultDto.Code = token;
                        resultDto.Address = userExtend.Avatar;
                        resultDto.Level = userExtend?.Level;
                        return resultDto;
                    } else {
                        Redis.CreateInstance().SetCache<UserDto>(jsonResult.openid, userDto, TimeSpan.FromMinutes(10));
                        return userDto;
                    }
                } else {
                    throw new BaseException(System.Net.HttpStatusCode.InsufficientStorage, jsonResult.errmsg);
                }
            } catch (Exception ex) {
                throw new BaseException(ex.Message);
            }
        }

        /// <summary>
        /// 更新当前用户的openId
        /// 手机号码登录成功时候记得要更新下openId
        /// <param name="code"></param>
        /// <param name="parentId"></param>
        /// </summary>

        [HttpGet]
        [Route("{code}")]
        [NoPermission]
        public string UpdateOpenId(string code, int parentId) {
          
            var jsonResult = SnsApi.JsCode2Json(AppSettingsConstVars.WxOpenAppId, AppSettingsConstVars.WxOpenAppSecret,
                code);
            if (jsonResult.errcode != ReturnCode.请求成功) {
                throw new BaseException("获取微信code失败");
            }

            var firstUser = this._db.Users.FirstOrDefault(t => t.Id == this._user.Id);
            if (firstUser == null) {
                throw new BaseException("用户不存在");
            }
            if (firstUser.ParentId == 0 && parentId != 0) {
                firstUser.ParentId = parentId;
            }
            firstUser.OpenId = jsonResult.openid;
            this._db.SaveChanges();
            return jsonResult.openid;
        }
    }
}
