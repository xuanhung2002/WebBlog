﻿namespace WebBlog.API.Authorization
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AllowAnonymousAttribute : Attribute
    {
    }
}
