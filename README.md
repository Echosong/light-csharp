

## 概述
### 关于light

`light-csharp` 是一个基于 `.NET` 平台的快速开发框架，采用现代化的架构设计和最佳实践，面向对象领域设计驱动，  框架是一个值得尝试的开源框架。


### light-csharp有什么优点?

- 灵活性：框架采用现代化的架构设计，支持经典三层和`DDD`架构开发模式。
- 易用性：框架提供了完善的文档和示例代码，同时也集成了一系列主流的微服务技术栈，使用起来比较容易上手。
- 代码自动生成： 基于entity 实体对应自定义 特性实现 后端各层，前端ElementUI  全栈自动生成
- 开放性：框架是一个开源项目，采用 `MIT` 许可证发布，用户可以自由地使用、修改和分享该框架的源代码。

## 架构设计

### 目录功能模块

```
Light 
├── Light.Admin 后台相应接口模块
├── Light.Admin-UI 技术文档模块
├── Light.Api 前端app相关接口
├── Light.Common 通用类库模块包括DTO Enum
├── Light.Entity 实体库对应数据库表
├── Light.Job 定时任务相关
├── Light.Service 核心业务处理
├── Light.Test 测试使用模块
├── Light.Tool 自动生成代码模块
├── .gitignore
├── README.MD
└── LICENSE
```

### 技术栈

| 名称                                                         | 描述                                                         |
| ------------------------------------------------------------ | ------------------------------------------------------------ |
| <a target="_blank" href="https://github.com/NewLifeX/X">NewLeft core</a> | 核心库，日志、配置、缓存、网络、序列化、APM性能追踪                                 |
| <a target="_blank" href="https://github.com/HangfireIO/Hangfire">Hangfire</a> | 定时任务处理库                                      |
| <a target="_blank" href="https://github.com/reactiveui/refit">Refit</a> | 一个声明式自动类型安全的RESTful服务调用组件，用于同步调用其他微服务 | 
| <a target="_blank" href="https://github.com/nunit/nunit">nunit</a> | 测试框架 | 
| <a target="_blank" href="https://entityframework-plus.net">Z.EntityFramework.Plus.EFCore</a> | 第三方高性能的EfCore组件                                     |
| <a target="_blank" href="https://github.com/NLog/NLog">NLog</a><br />Nlog<br />Nlog.Loki | 日志记录组件                                                 |
| <a target="_blank" href="https://github.com/TinyMapper/TinyMapper">TinyMapper</a> | 模型映射组件     
| <a target="_blank" href="https://github.com/Senparc/WeiXinMPSDK">Senparc.Weixin</a> | 包括微信公众号、小程序、小游戏、企业号、开放平台、微信支付、JS-SDK、微信硬件/蓝牙，等等
| <a target="_blank" href="https://github.com/domaindrivendev/Swashbuckle.AspNetCore">Swashbuckle.AspNetCore</a> | APIs文档生成工具(swagger)                                    |
| <a target="_blank" href="https://github.com/StackExchange/StackExchange.Redis">StackExchange.Redis</a> | 开源的Redis客户端SDK                                         |

## 后端步骤流程



## 前端

### 项目地址

> 到 Light.Admin.UI 下面 执行

```javascript
npm i

npm serve

```

### 界面截图

![运行效果](https://github.com/Echosong/light/blob/main/web.png?raw=true)

## 其他

### 问题交流
- QQ群号：571627871

![img](https://github.com/Echosong/beego_element_cms/blob/main/doc/wx.png?raw=true)

- 都看到这里了，那就点个`star`吧！

## License

**MIT**   
**Free Software, Hell Yeah!**
