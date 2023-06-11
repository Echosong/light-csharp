module.exports = {
	runtimeCompiler: true,
	devServer:{
		port: 8081,
		proxy: {
			"/api":{
				target: "http://v-admin.tongchenghy.cn/",
				changeOrigin: true,
				pathRewrite:{
					'^/api':''
				}
			},
			'/upload':{
				target: "http://localhost:60029/upload",
				changeOrigin: true,
				pathRewrite: {
					'^/upload':''
				}
			}
			
		}
	},
	lintOnSave:false
}