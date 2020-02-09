using System.Web.SessionState;

namespace System.Web
{
    public static class HttpSessionStateExtension
    {
        public static IHttpSessionState AsInterface(this HttpSessionState session)
        {
            return new HttpSessionStateImpl(session);
        }
    }
}