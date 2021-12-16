using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Visitors.VisitorOnline
{
    public interface IVisitorOnlineService
    {
        void ConnectUser(string clientId);
        void DisConnectUser(string clientId);
        int GetCount();
    }
}
