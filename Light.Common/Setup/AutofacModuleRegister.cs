using System.Reflection;
using Autofac;

namespace Light.Common.Setup {
    public class AutofacModuleRegister : Autofac.Module {
        protected override void Load(ContainerBuilder builder) {
            var basePath = AppContext.BaseDirectory;

            #region 带有接口层的服务注入

            var servicesDllFile = Path.Combine(basePath, "Light.Service.dll");


            if (!(File.Exists(servicesDllFile))) {
                var msg = "Services.dll 丢失，因为项目解耦了，所以需要先F6编译，再F5运行，请检查 bin 文件夹，并拷贝。";
                throw new Exception(msg);
            }
            // 获取 Service.dll 程序集服务，并注册
            var assemblysServices = Assembly.LoadFrom(servicesDllFile);
            //支持属性注入依赖重复
            builder.RegisterAssemblyTypes(assemblysServices).AsImplementedInterfaces().InstancePerDependency()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            #endregion

        }
    }
}
