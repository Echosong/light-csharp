using Light.Common.Dto.Api;
using Light.Entity;

namespace Light.Service {
    public interface IPaymentService {

        public FinanceOp Pay(PayProductDto payProduct, int userId, string outOrderNo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outOrderNo">流水id</param>
        /// <returns></returns>
        public FinanceOp Notify(string transaction_id, string outOrderNo);
    }
}
