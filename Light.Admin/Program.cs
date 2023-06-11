using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Senparc.Weixin.AspNet;
using Senparc.Weixin.MP;
using Senparc.Weixin.RegisterServices;
using Light.Common.Configuration;
using Light.Common.Filter;
using Light.Common.RedisCache;
using Light.Common.Setup;
using Light.Entity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton(new AppSettingsHelper(builder.Environment.ContentRootPath));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//ȫ��ע��һ��redis
builder.Services.AddScoped<Redis>();

//swagger
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo {
        Title = "��־Ը�����ĵ�-Admin",
        Description = "�ӿ�˵�� --������"
    });

    //���Ӱ�ȫ����
    c.AddSecurityDefinition("Token", new OpenApiSecurityScheme {
        Description = "������token,��ʽΪ Bearer xxxxxxxx��ע���м�����пո�",
        Name = "token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = ""
    });
    //���Ӱ�ȫҪ��
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
    var xmlPath = Path.Combine(basePath, "Light.Admin.xml");
    c.IncludeXmlComments(xmlPath, true);
});


// ע��filter
builder.Services.AddMvc(t => {
    t.Filters.Add(new ExceptionsFilterForApi());
    t.Filters.Add(new ResultAttribute());
    t.Filters.Add(new AuthFilterForAdmin());
}).AddNewtonsoftJson(p => {
    //���ݸ�ʽ����ĸСд ��ʹ���շ�
    p.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    //��ʹ���շ���ʽ��key
    //p.SerializerSettings.ContractResolver = new DefaultContractResolver();
    //����ѭ������
    p.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    //����ʱ���ʽ������ʹ��yyyy/MM/dd��ʽ����Ϊiosϵͳ��֧��2018-03-29��ʽ��ʱ�䣬ֻʶ��2018/03/09���ָ�ʽ����
    p.SerializerSettings.DateFormatString = "yyyy/MM/dd HH:mm:ss";
    //����null ֵ
    p.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

});

#region ����΢������

//ʹ�ñ��ػ����������
builder.Services.AddMemoryCache();

//Senparc.Weixin ע�ᣨ���룩
builder.Services.AddSenparcWeixinServices(builder.Configuration);

#endregion

//����Dbcontext����
//Enable-Migrations  -ProjectName "Light.Entity" -StartUpProjectName "Light.Api"  -Verbose
builder.Services.AddDbContext<Db>(options => { options.UseSqlServer(AppSettingsConstVars.DbSqlConnection); });

//�Զ�ע�� service
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>((host, containerBuilder) => {

    var controllerBaseType = typeof(ControllerBase);
    containerBuilder.RegisterAssemblyTypes(typeof(Program).Assembly)
        .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
        .PropertiesAutowired();
    containerBuilder.RegisterModule(new AutofacModuleRegister());
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}


#region ����΢������

//����΢�����ã����룩
var registerService = app.UseSenparcWeixin(app.Environment,
    null /* ��Ϊ null �򸲸� appsettings  �е� SenpacSetting ����*/,
    null /* ��Ϊ null �򸲸� appsettings  �е� SenpacWeixinSetting ����*/,
    register => { /* CO2NET ȫ������ */ },
    (register, weixinSetting) => {
        //ע�ṫ�ں���Ϣ������ִ�ж�Σ�ע�������ںţ�
        register.RegisterMpAccount(weixinSetting, "��SNS�����ں�");
    });


#endregion



app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
