using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Light.Common.Enums;
using Light.Common.Utils;
using Light.Entity;

namespace Light.Admin.Controllers {
    /// <summary>
    ///     首页处理
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class HomeController : BaseController {


        /// <summary>
        ///     构造
        /// </summary>
        public HomeController(Db db) {
            this._db = db;
        }

        /// <summary>
        /// 首页统计数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Dictionary<string, Dictionary<string, decimal?>> Statistics() {
            var find = _db.Users.Find(this._user.Id);

            var sUserIds = new List<int>();
            if (find != null) {
                if (find.Type == (int)UserTypeEnum.分销商) {
                    sUserIds = _db.Users.Where(t => t.ParentId == this._user.Id).Select(t => t.Id).ToList();
                }
            }

            //各种数据统计
            Dictionary<string, decimal?> decimals = new Dictionary<string, decimal?>();

            var where = PredicateExtend.True<FinanceOp>();
            where = where.And(t => t.State == (int)FinanceOpStateEnum.成功);
            if (sUserIds.Count > 0) {
                where = where.And(t => sUserIds.Contains(t.UserId!.Value));
            }

            //统计订单
            var financeAccounts = _db.FinanceOps.Where(where)
                .Select(t=>new{ t.Account, t.BusinessType, t.Description}).ToList();


            if (find != null && find.Type == (int)UserTypeEnum.分销商) {
                var list = _db.Users.Where(t =>t.CreateDateTime != null &&  t.CreateDateTime > DateTime.Now.AddDays(-30) &&
                                               (t.ParentId == this._user.Id || t.GrandpaId == this._user.Id))
                    .Select(t=>new {t.Id, t.CreateDateTime}).ToList();

                decimals.Add("今日新增用户",list.Count(t=>t.CreateDateTime > DateTime.Now.AddDays(-1)));
                decimals.Add("近一周新增用户", list.Count(t => t.CreateDateTime > DateTime.Now.AddDays(-7)));
                decimals.Add("近一月新增用户", list.Count());

                var finances = _db.FinanceOps.Where(t=>t.CreateDateTime > DateTime.Now.AddDays(-30) && t.State == (int)FinanceOpStateEnum.成功 && t.UserId == this._user.Id)
                    .Select(t=>new{t.Id, t.CreateDateTime, t.Account}).ToList();

                decimals.Add("今日收入", finances.Where(t => t.CreateDateTime > DateTime.Now.AddDays(-1)).Sum(t=>t.Account));
                decimals.Add("近一周收入", finances.Where(t => t.CreateDateTime > DateTime.Now.AddDays(-7)).Sum(t=>t.Account));
                decimals.Add("近一月收入", finances.Sum(t=>t.Account));
            } else {
                //总金额
                decimals.Add("支付总金额", financeAccounts.Where(t => t.BusinessType == (int)FinanceTypeEnum.购买会员).Sum(t => t.Account) ?? 0);

                //分销
                decimals.Add("总分销金额", financeAccounts.Where(t => t.Description != null && t.BusinessType == (int)FinanceTypeEnum.分润 && t.Description.Contains("分销"))
                    .Sum(t => t.Account) ?? 0);

                decimals.Add("总招商金额", financeAccounts.Where(t => t.Description != null && t.BusinessType == (int)FinanceTypeEnum.分润 && t.Description.Contains("招商")).Sum(t => t.Account) ?? 0);

                if (sUserIds.Count > 0) {
                    decimals.Add("VIP人数", _db.UserExtends.Count(t => t.Level == (int)UserLevelEnum.VIP会员 && sUserIds.Contains(t.UserId)));
                } else {
                    decimals.Add("VIP人数", _db.UserExtends.Count(t => t.Level == (int)UserLevelEnum.VIP会员));
                }
            }
            
            where = PredicateExtend.True<FinanceOp>();
            where = where.And(t => t.CreateDateTime > DateTime.Now.AddDays(-7) && t.State == (int)FinanceOpStateEnum.成功 && t.BusinessType == (int)FinanceTypeEnum.购买会员);
            if (sUserIds.Count > 0) {
                where = where.And(t => sUserIds.Contains(t.UserId!.Value));
            }

            //订单
            var dateTimes = _db.FinanceOps.Where(where)
                .Select(t=>t.CreateDateTime).ToList();

            //新增用户
            var whereUser = PredicateExtend.True<User>();
            whereUser = whereUser.And(t => t.CreateDateTime > DateTime.Now.AddDays(-7));
            if (sUserIds.Count > 0) {
                whereUser = whereUser.And(t => sUserIds.Contains(t.Id));
            }
            var users = _db.Users.Where(whereUser).Select(t=>t.CreateDateTime).ToList();

            var orderDictionary = new Dictionary<string, decimal?>();
            var userDictionary = new Dictionary<string, decimal?>();

            //统计七天订单
            for (int i = -6; i < 1; i++) {
                var dateTime = DateTime.Now.AddDays(i).ToString("MM/dd");

                orderDictionary.Add(dateTime, dateTimes.Count(t => t != null && t.Value.ToString("MM/dd") == dateTime));
                userDictionary.Add(dateTime, users.Count(t => t != null && t.Value.ToString("MM/dd") == dateTime));
            }

            return new Dictionary<string, Dictionary<string, decimal?>>() {
                { "总统计", decimals },
                { "订单统计", orderDictionary },
                { "用户统计", userDictionary }
            };


        }



        /// <summary>
        ///     约定通用方法（表得某个字段需要枚举时候，那么用(表名+字段+Enum) 比如
        ///     UserSateEnum 就表示 user 模型下 state 字段 对应得枚举）
        /// </summary>
        /// <param name="enumName"></param>
        /// <returns></returns>
        [HttpGet]
        public List<Dictionary<string, object>> GetEnums(string enumName) {
            return FunctionUtil.GetEnums(enumName);
        }
    }
}