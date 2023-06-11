
## 优点
- 上手简单：提供完整彻底的封装，不用修改一行源代码即可轻松集成
- 示例全面：提供大量常见增删改查示例，提高你的生产力



## 框架选型
- JS引擎：[Vue @2.6.11](https://cn.vuejs.org/)
- 脚手架：[@vue/cli @4.0.5](https://cli.vuejs.org/zh/)
- UI框架：[Element-UI @2.13.0](https://element.eleme.cn/#/zh-CN)
- web弹层：[layer @3.1.1](http://layer.layui.com/)
- 切页动画：[Swiper @4.5.0](https://www.swiper.com.cn/)


## 开始使用
#### 1、获取源码
- 方式1：通过gitee或者github下载
- 方式2：通过Git Bash：	
```
	git clone https://github.com/click33/sa-vue-admin.git
```

#### 2、运行
- 获取源码后，在项目根目录执行cmd命令：
```
	安装依赖：npm i
	运行项目：npm run serve
```
- 根据控制台输出的路径，访问浏览器，一般为：<a href="http://localhost:8080/" target="_blank">http://localhost:8080/</a>

#### 3、使用注意
- 在开发时，如无必要，请不要直接魔改模板的代码，以免在运行时出现意外bug
- 在 sa-resourecs 文件下，有个 sa-admin-init.js， 这是模板为你预留一个文件，用来对接你的业务逻辑
- 在这个文件里，你可以：
```
	sa_admin.title = '上海开赟软件';		// 设置模板标题
	sa_admin.logo_url = '图片地址';    	// 设置logo图标地址   默认值空, 代表使用：./admin-logo.png
	sa_admin.icon_url = '图片地址';   	 // 设置icon图标地址   默认值空, 代表使用：./favicon.ico
	sa_admin.initMenu(); // 初始化菜单, 不传参代表默认显示所有菜单 菜单在 ./sa-menu-list.js 里定义,
	// sa_admin.initMenu(['1', '1-1', '1-2', '4']); 	// 传入一个id数组, 显示指定菜单  
	// sa.checkAuth('a');		// 鉴权认证 （sa_admin内部封装了鉴权功能）
	sa_admin.init();	// 初始化模板, 必须调用
```

- 以上示例在 sa-admin-init.js 文件里均有详细的注释说明，如果还要明白的地方，可以加入qq群询问


## 贡献代码
1. 在github上fork一份到自己的仓库
2. clone自己的仓库到本地电脑
3. 在本地电脑修改、commit、push
4. 提交pr（点击：New Pull Request）（提交pr前请保证自己fork的仓库是最新版本，如若不是先强制更新一下）
5. 等待合并

## 建议贡献的地方
- 更多登录模板
- 修复源码现有bug，或增加新的实用功能（比如：流畅的tab左右拖拽排序）
- 更多demo示例：比如针对element-ui一些复杂组件的示例，或者其它一些常见js库的集成使用
- 如果更新实用功能，可在文档友情链接处留下自己的推广链接


## QQ群
QQ交流群：[782974737 点击加入](https://jq.qq.com/?_wv=1027&k=5DHN5Ib)

## 截图

![截图](https://color-test.oss-cn-qingdao.aliyuncs.com/sa-vue-admin/2.jpg)
![截图](https://color-test.oss-cn-qingdao.aliyuncs.com/sa-vue-admin/1.jpg)
![截图](https://color-test.oss-cn-qingdao.aliyuncs.com/sa-vue-admin/3.jpg)

见演示说明地址


