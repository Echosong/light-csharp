
using Microsoft.EntityFrameworkCore;
using Light.Common.Error;

namespace Light.Entity {
    /// <summary>
    /// Update-Database -Force -ProjectName "Light.Entity" -StartUpProjectName "Light.Server" -ConnectionStringName "MesDbContext" 
    /// </summary>

    public class Db : DbContext {

        /// <summary>
        /// 当前用户
        /// </summary>
        public int UserId { get; set; } = 0;

        /// <summary>
        /// 分账id
        /// </summary>
        public int SiteId { get; set; } = 0;

        public Db(DbContextOptions options) : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
        }

        public string GetCode(Type entity) {
            var code = this.Codes.FirstOrDefault(t => t.Name == entity.Name);
            if (code == null) {
                code = new Code() {
                    Name = entity.Name,
                    Number = 1
                };
                this.Codes.Add(code);
            } else {
                code.Number += 1;
            }
            this.SaveChanges();
            return DateTime.Now.ToString("yyMMdd") + $"{code.Number:D5}";
        }

        /// <summary>
        /// 保存数据前处理
        /// </summary>
        /// <exception cref="BaseException"></exception>
        private void SaveChange() {
            var adds = ChangeTracker.Entries()
                 .Where(e => e.State == EntityState.Added && e.Entity is SysBase)
                 .ToList();

            //添加时候处理
            adds.ForEach(e => {
                ((SysBase)e.Entity).CreateDateTime = DateTime.Now;
                ((SysBase)e.Entity).CreatorId = UserId;
                ((SysBase)e.Entity).SiteId = SiteId;
            });

            var dels = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Deleted && e.Entity is SysBase)
                .ToList();

            //不允许删除 CreatorId == 0 的数据
            dels.ForEach(e => {
                if (((SysBase)e.Entity).CreatorId == 0) {
                    throw new BaseException("系统内置数据不允许删除");
                }
            });


            // 修改处理
            var mods = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified && e.Entity is SysBase)
                .ToList();

            mods.ForEach(e => {
                ((SysBase)e.Entity).UpdateDateTime = DateTime.Now;
                ((SysBase)e.Entity).UpdaterId = UserId;
            });

        }


        /// <summary>
        /// 拦截保存修改
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges() {
            this.SaveChange();

            return base.SaveChanges();
        }

        /// <summary>
        /// 异步保持
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
            this.SaveChange();
            return base.SaveChangesAsync(cancellationToken);
        }

        #region 常规
        public DbSet<Ad> Ads { get; set; }
        public DbSet<Notice> Notices { get; set; }

    


        public DbSet<Sms> Smss { get; set; }

        public DbSet<Template> Templates { get; set; }

        public DbSet<Area> Areas { get; set; }

        public DbSet<Article> Articles { get; set; }

        #endregion

        #region 用户信息体系
        public DbSet<Code> Codes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserExtend> UserExtends { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Config> Configs { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        public DbSet<WeiTemp> WeiTemps { get; set; }

        #endregion

        #region 帖子相关业务
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Attachment> Attachments { get; set; }




        #endregion

        #region 资金管理

        

        /// <summary>
        /// 下载产品
        /// </summary>
        public DbSet<Download> Downloads { get; set; }

        public DbSet<FinanceOp> FinanceOps { get; set; }

 

        #endregion

        #region 院校相关
        

        public DbSet<UserFavorite> UserFavorites { get; set; }


        #endregion


    }
}