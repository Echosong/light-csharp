namespace Light.Service {
    public interface IBaiduTextCensorService {
        /// <summary>
        /// 内容过审
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public string TextCensor(string content);

        /// <summary>
        /// 获取地址
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public Task<string> GetAddress(string address);
    }
}
