using System;
using Nancy;

public class HomeModule : NancyModule
{
    public HomeModule()
    {
        Get["/api/version"] = p => "version 0.1.4.7";
        Get["/default.html"] = p => View["default"];
        Get["/{path}"] = p => {
            return string.Format("Hello from Nancy! <br/> path : {0}", string.IsNullOrWhiteSpace(p.path) ? "/" : p.path);
        };

    }
}