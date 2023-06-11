using System.Reflection;
using Light.Entity;
using Light.Tool.Service;


namespace Light.Tool {
    /// <summary>
    /// 启动文件
    /// </summary>
    public class Start {

        private static bool IsSubClassOf(Type type, Type baseType) {
            var b = type.BaseType;
            while (b != null) {
                if (b == baseType) {
                    return true;
                }
                b = b.BaseType;
            }
            return false;
        }

        /// <summary>
        /// 入口
        /// </summary>
        public static void Main() {
            const string span = "Light.Entity";
            var q = from t in Assembly.Load(span).GetTypes()
                    where IsSubClassOf(t, typeof(SysBase)) && !t.Name.EndsWith("Base")
                    select t;
            Console.WriteLine(@"=========================================");
            Console.Write(@"输入特定的实体单独强制覆盖处理 为空就全部：");

            var entityName = Console.ReadLine();

            q.ToList().ForEach(t => {
                if (String.IsNullOrEmpty(entityName) || entityName.Split(',').Contains(t.Name)) {
                    var controllerService = new ControllerService(t);
                    controllerService.Start();
                    Console.WriteLine(t.Name + @" 控制器 处理完成......");

                    var dtoService = new DtoService(t);
                    dtoService.Start();
                    Console.WriteLine(t.Name + @" Dto查询 处理完成......");

                    var viewService = new ViewService(t);
                    viewService.Start();
                    Console.WriteLine(t.Name + @" 视图 处理完成......");

                    var dictionaryService = new DictionaryService(t);
                    dictionaryService.Start();
                    Console.WriteLine(t.Name + @" 数据单表处理完成... ");
                }
            });

            DictionaryService.WriteDictionaryFile();
            Console.WriteLine(@"=========================================");
            Console.WriteLine(@"完成！！");
            Console.ReadLine();
        }
    }
}
