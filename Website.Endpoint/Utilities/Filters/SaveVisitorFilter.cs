using Application.Visitors.SaveVisitorInfo;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UAParser;

namespace Website.Endpoint.Utilities.Filters
{
    public class SaveVisitorFilter : IActionFilter
    {
        private readonly ISaveVisitorInfoService _saveVisitorInfoService;

        public SaveVisitorFilter(ISaveVisitorInfoService saveVisitorInfoService)
        {
            _saveVisitorInfoService = saveVisitorInfoService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

            string ip = context.HttpContext.Request.HttpContext.Connection.RemoteIpAddress.ToString();
            var actionName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName;
            var controllerName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ControllerName;
            var userAgent = context.HttpContext.Request.Headers["User-Agent"];

            var uaParser = Parser.GetDefault();
            ClientInfo clientInfo = uaParser.Parse(userAgent);
            string referer = context.HttpContext.Request.Headers["Referer"].ToString();
            var currentUrl = context.HttpContext.Request.Path;
            var request = context.HttpContext.Request;


            _saveVisitorInfoService.Execute(new RequestSaveVisitorInfoDto()
            {

                OperationSystem = new VisitorVersionDto()
                {
                    Family = clientInfo.OS.Family,
                    Version = $"{clientInfo.OS.Major}.{clientInfo.OS.Minor}.{clientInfo.OS.Patch}",
                },
                Browser = new VisitorVersionDto()
                {
                    Family = clientInfo.UA.Family,
                    Version = $"{clientInfo.UA.Major}.{clientInfo.UA.Minor}.{clientInfo.UA.Patch}",
                },
                Device = new DeviceDto()
                {
                    Brand = clientInfo.Device.Brand,
                    Family = clientInfo.Device.Family,
                    IsSpider = clientInfo.Device.IsSpider,
                    Model = clientInfo.Device.Model
                },
                Ip = ip,
                Method = request.Method,
                PhysicalPath = $"{controllerName}/{actionName}",
                Protocol = request.Protocol,
                ReferrerLink = referer,
                CurrentLink = currentUrl
            });
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            throw new NotImplementedException();
        }

    }
}
