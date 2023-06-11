using System.Net;

namespace Light.Common.Error {
    public class BaseException : Exception {

        public HttpStatusCode Code { get; }

        public BaseException(string message) : base(message) {
            Code = HttpStatusCode.InternalServerError;
        }

        public BaseException(HttpStatusCode code, string message) : base(message) {
            this.Code = code;
        }


    }
}
