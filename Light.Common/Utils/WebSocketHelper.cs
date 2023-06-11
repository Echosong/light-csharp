using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Light.Common.Utils {
    public class WebSocketHelper {

        /// <summary>
        /// 全局socket存储
        /// </summary>
        public static ConcurrentDictionary<string, WebSocket> socketsList
            = new ConcurrentDictionary<string, WebSocket>();

        /// <summary>
        /// 缓冲区大小
        /// </summary>
        private const int bufferSize = 1024 * 4;

        /// <summary>
        /// 传入用户 发送socket
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="userId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static async Task SendMessage<TEntity>(int userId, TEntity entity) {
            WebSocket reviceWebSocket;
            socketsList.TryGetValue(userId.ToString(), out reviceWebSocket);
            if (reviceWebSocket != null) {
                await SendMessage(reviceWebSocket, entity);
            }
        }


        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="webSocket">WebSocket</param>
        /// <param name="entity">消息实体</param>
        /// <typeparam name="TEntity">typeof(TEntity)</typeparam>
        /// <returns></returns>
        public static async Task SendMessage<TEntity>(WebSocket webSocket, TEntity entity) {

            var serializerSettings = new JsonSerializerSettings {
                // 设置为驼峰命名
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatString = "yyyy-MM-dd HH:mm:ss"
            };
            var Json = JsonConvert.SerializeObject(entity, Formatting.None, serializerSettings);

            var bytes = Encoding.UTF8.GetBytes(Json);

            await webSocket.SendAsync(
                new ArraySegment<byte>(bytes),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None
            );
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="webSocket">WebSocket</param>
        /// <returns></returns>
        public static async Task<TEntity> Receiveentity<TEntity>(WebSocket webSocket) {
            var buffer = new ArraySegment<byte>(new byte[bufferSize]);
            var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
            while (!result.EndOfMessage) {
                result = await webSocket.ReceiveAsync(buffer, default(CancellationToken));
            }
            var json = Encoding.UTF8.GetString(buffer.Array);
            json = json.Replace("\0", "").Trim();
            if (json != null) {
                try {
                    var chat = JsonConvert.DeserializeObject<TEntity>(json, new JsonSerializerSettings() {
                        DateTimeZoneHandling = DateTimeZoneHandling.Local
                    });
                    if (chat != null) {
                        //记录写入数据库
                        return chat;
                    }
                } catch (Exception ex) {
                    LogHelper.Error("格式错误");
                }
            }
            return default(TEntity);
        }

    }
}
