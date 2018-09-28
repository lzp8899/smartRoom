1:安装以下包
install-package Microsoft.AspNetCore.Mvc
install-package Swashbuckle.AspNetCore
install-package Microsoft.AspNetCore.StaticFiles
install-package Microsoft.AspNetCore.Hosting
install-package Microsoft.AspNetCore.Server.kestrel
install-package Microsoft.AspNetCore.Diagnostics
install-package Microsoft.AspNetCore.Authentication
install-package Microsoft.Extensions.DependencyInjection
install-package Microsoft.AspNetCore.Authentication.Cookies

2:所有需要DI注入的对象,请在Startup.cs的ConfigureServices方法中注入
services.AddTransient每次请求生成新的对象
services.AddSingleton生成单例对象

3:在WPF启动程序中启动web服务
81:web的端口 ServiceController:表示Controller所在的项目
Web.Startup.StartWeb(81, typeof(ServiceController).Assembly);
启动成功后访问http://localhost:81/swagger 可以访问表示启动成功
