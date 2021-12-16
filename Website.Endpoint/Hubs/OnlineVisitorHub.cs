using Application.Visitors.VisitorOnline;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Endpoint.Hubs
{
    public class OnlineVisitorHub : Hub
    {
        private readonly IVisitorOnlineService _visitorOnlineService;

        public OnlineVisitorHub(IVisitorOnlineService visitorOnlineService)
        {
            _visitorOnlineService = visitorOnlineService;
        }

        public override Task OnConnectedAsync()
        {
            var visitorId = Context.GetHttpContext().Request.Cookies["visitorId"];
            _visitorOnlineService.ConnectUser(visitorId); 
            var count = _visitorOnlineService.GetCount();
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var visitorId = Context.GetHttpContext().Request.Cookies["visitorId"];
            _visitorOnlineService.DisConnectUser(visitorId);
            var count = _visitorOnlineService.GetCount();
            return base.OnDisconnectedAsync(exception);
        }
    }
}
