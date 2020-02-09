using System.Collections;
using System.Collections.Specialized;
using System.Web.SessionState;

namespace System.Web
{
    public class HttpSessionStateImpl : IHttpSessionState
    {
        private readonly HttpSessionState _baseSession;

        internal HttpSessionStateImpl(HttpSessionState session)
        {
            _baseSession = session;
        }

        public void Abandon()
        {
            _baseSession.Abandon();
        }

        public void Add(string name, object value)
        {
            _baseSession.Add(name, value);
        }

        public void Remove(string name)
        {
            _baseSession.Remove(name);
        }

        public void RemoveAt(int index)
        {
            _baseSession.RemoveAt(index);
        }

        public void Clear()
        {
            _baseSession.Clear();
        }

        public void RemoveAll()
        {
            _baseSession.RemoveAll();
        }

        public IEnumerator GetEnumerator()
        {
            return _baseSession.GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            _baseSession.CopyTo(array, index);
        }

        public string SessionID
        {
            get { return _baseSession.SessionID; }
        }

        public int Timeout
        {
            get { return _baseSession.Timeout; }
            set { _baseSession.Timeout = value; }
        }

        public bool IsNewSession
        {
            get { return _baseSession.IsNewSession; }
        }

        public SessionStateMode Mode
        {
            get { return _baseSession.Mode; }
        }

        public bool IsCookieless
        {
            get { return _baseSession.IsCookieless; }
        }

        public HttpCookieMode CookieMode
        {
            get { return _baseSession.CookieMode; }
        }

        public int LCID
        {
            get { return _baseSession.LCID; }
            set { _baseSession.LCID = value; }
        }

        public int CodePage
        {
            get { return _baseSession.CodePage; }
            set { _baseSession.CodePage = value; }
        }

        public HttpStaticObjectsCollection StaticObjects
        {
            get { return _baseSession.StaticObjects; }
        }

        object IHttpSessionState.this[string name]
        {
            get { return _baseSession[name]; }
            set { _baseSession[name] = value; }
        }

        object IHttpSessionState.this[int index]
        {
            get { return _baseSession[index]; }
            set { _baseSession[index] = value; }
        }

        public int Count
        {
            get { return _baseSession.Count; }
        }

        public NameObjectCollectionBase.KeysCollection Keys
        {
            get { return _baseSession.Keys; }
        }

        public object SyncRoot
        {
            get { return _baseSession.SyncRoot; }
        }

        public bool IsReadOnly
        {
            get { return _baseSession.IsReadOnly; }
        }

        public bool IsSynchronized
        {
            get { return _baseSession.IsSynchronized; }
        }
    }
}