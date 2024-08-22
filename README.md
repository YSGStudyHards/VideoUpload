# VideoUpload
APS.NET MVC+Layui视频上传！！！

# 前言
前段时间在使用APS.NET MVC+LayUI做视频上传功能的时，发现当上传一些内存比较大的视频就会提示上传失败，后来通过查阅相关资料发现.NET MVC框架为考虑安全问题，在运行时对请求的文件的长度（大小）做了限制默认为4MB（4096KB），因此我们需要在Web.Config中设置最大请求文件长度大小，本文主要讲解如何设置Web.Config中的最大请求文件大小配置和提供一个完整的ASP.NET MVC+LayUI上传视频的教程。

# 视频上传效果演示
![](https://img2020.cnblogs.com/blog/1336199/202008/1336199-20200801013541834-2117167042.gif)

# 博客详细描述地址
https://mp.weixin.qq.com/s/7fCEMbHpvkP07FwxqSzCbQ
