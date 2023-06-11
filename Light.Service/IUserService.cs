using Light.Common.Dto;
using Light.Common.Dto.Api;
using Light.Entity;

namespace Light.Service {
    public interface IUserService {

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string Login(LoginUserDto user);

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userExtend"></param>
        public User Create(User user, UserExtend userExtend);

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="user"></param>
        public void Update(User user);

        /// <summary>
        /// 微信小程序授权登录
        /// </summary>
        /// <param name="userDto"></param>
        public UserDto WxLogin(UserDto userDto);

        /// <summary>
        /// 获取头衔
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<UserHeaderApiDto> Headers(List<int> ids);

        /// <summary>
        /// 前端手机号码登录
        /// </summary>
        /// <param name="userApiDto"></param>
        /// <returns></returns>
        public string LoginApi(LoginUserApiDto userApiDto);

        /// <summary>
        /// 检查是否为vip
        /// </summary>
        /// <param name="userId"></param>
        void CheckVip(int userId);

    }
}
