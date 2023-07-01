using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Senparc.Weixin;
using Senparc.Weixin.AspNet;
using Senparc.Weixin.MP;
using Senparc.Weixin.RegisterServices;
using Senparc.Weixin.TenPayV3;
using Light.Common.Configuration;
using Light.Common.Filter;
using Light.Common.RedisCache;
using Light.Common.Setup;
using Light.Entity;
using Light.Job;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton(new AppSettingsHelper(builder.Environment.ContentRootPath));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//全局注册一个redis
builder.Services.AddScoped<Redis>();

//swagger
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo {
        Title = "Light在线文档-Api",
        Description = "接口说明 --二胡子"
    });

    //添加安全定义
    c.AddSecurityDefinition("Token", new OpenApiSecurityScheme {
        Description = "请输入token,格式为 Bearer xxxxxxxx（注意中间必须有空格）",
        Name = "token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = ""
    });
    //添加安全要求
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme{
                Reference =new OpenApiReference{
                    Type = ReferenceType.SecurityScheme,
                    Id ="Token"
                }
            },new string[]{ }
        }
    });
    var basePath = AppContext.BaseDirectory;
    var xmlPath = Path.Combine(basePath, "Voluntary.Api.xml");
    c.IncludeXmlComments(xmlPath, true);
});

//注册Hangfire定时任务
//注意这里可能会影响iis 应用池问题
builder.Services.AddHangFireSetup();

#region 添加微信配置

//使用本地缓存必须添加
builder.Services.AddMemoryCache();

//Senparc.Weixin 注册（必须）
builder.Services.AddSenparcWeixinServices(builder.Configuration);

#endregion


//添加Dbcontext服务
//Enable-Migrations  -ProjectName "Voluntary.Entity" -StartUpProjectName "Voluntary.Api"  -Verbose
builder.Services.AddDbContext<Db>(options => { options.UseSqlServer(AppSettingsConstVars.DbSqlConnection); });


// 注入filter
builder.Services.AddMvc(t => {
    t.Filters.Add(new ExceptionsFilterForApi());
    t.Filters.Add(new ResultAttribute());
    t.Filters.Add(new AuthFilterForAdmin());
}).AddNewtonsoftJson(p => {
    //数据格式首字母小写 不使用驼峰
    p.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    //不使用驼峰样式的key
    //p.SerializerSettings.ContractResolver = new DefaultContractResolver();
    //忽略循环引用
    p.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    //设置时间格式（必须使用yyyy/MM/dd格式，因为ios系统不支持2018-03-29格式的时间，只识别2018/03/09这种格式。）
    p.SerializerSettings.DateFormatString = "yyyy/MM/dd HH:mm:ss";
    //忽略null 值
    p.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

});

//自动注入 service
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>((host, containerBuilder) => {

    var controllerBaseType = typeof(ControllerBase);
    containerBuilder.RegisterAssemblyTypes(typeof(Program).Assembly)
        .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
        .PropertiesAutowired();
    containerBuilder.RegisterModule(new AutofacModuleRegister());
});



var app = builder.Build();


#region 启用微信配置

//启用微信配置（必须）
var registerService = app.UseSenparcWeixin(app.Environment,
    null /* 不为 null 则覆盖 appsettings  中的 SenpacSetting 配置*/,
    null /* 不为 null 则覆盖 appsettings  中的 SenpacWeixinSetting 配置*/,
    register => { /* CO2NET 全局配置 */ },
    (register, weixinSetting) => {
        //注册公众号信息（可以执行多次，注册多个公众号）
        register.RegisterMpAccount(weixinSetting, "【SNS】公众号");
        //注册微信支付（可以执行多次，注册多个微信支付）
        register.RegisterTenpayRealV3(weixinSetting, "【SNS】微信支付（ApiV3）");
    });


#endregion

#region Hangfire定时任务

app.UseHangfireDashboard("/echosong"); //可以改变Dashboard的url

HangfireDispose.HangfireService();

#endregion


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment()) {
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseStaticFiles();
//app.UseHttpsRedirection();

app.MapControllers();

//启动websocket
app.UseWebSockets();

app.Run();
