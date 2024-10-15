# EpohUI 是什么

EpohUI 是 C# 实现的跨平台框架，目的是整合浏览器和系统接口的能力。

## EpohUI 能做什么

* 作为轻量的 HTTP 服务器  
  EpohUI 首先是一个极简的 HTTP 服务器，这可以让你浏览应用的目录和文件。  
  这意味着你可以直接在应用内部署 Web 应用，例如 Vue 、 React 、 Angular 等，当然你也可以直接写一个原生的 HTML 文件。  
* 调用系统接口  
  EpohUI 允许你通过 HTTP 协议调用系统接口，例如 IO 、 Net 、 Database 、 Thread 、 Process 等。  
* 调用自定义接口  
  EpohUI 除了可以调用系统接口，还可以动态调用用户定义的接口，而不需要编译或者打包整个应用。  

## EpohUI 如何使用  

EpohUI 使用非常简单，只需要一步：  

* 将 EpohUI.Core.exe 放到带有入口文件 index.html 的文件夹内，运行 EpohUI.Core.exe 即可进入应用首页。  

如果需要调用系统接口，也非常容易：  

```JavaScript
// 路径中的 lib/hello-world 是系统接口的唯一标识
fetch("http://localhost:33/api/lib/hello-world")
  .then(res => res.text())
  .then(data => {
    // 这里可以得到系统接口的返回值
    console.log(data);
  });
```

如果需要调用自定义接口，只需要两步：  

* 复制用户 DLL 文件到 DLLs 文件夹内
* 通过 HTTP 调用

## EpohUI 现在和将来具备的能力

* \[TODO\] 文件操作
* \[TODO\] 数据库操作
* \[TODO\] 进程操作
* \[TODO\] 多线程任务
* \[TODO\] 安全
* \[TODO\] 跨平台

## EpohUI 架构
