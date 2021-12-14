using System;
using System.Web;
using System.Web.Mvc;

namespace Planetarium.Controllers
{
    public class AuthorizationController : Controller
    {
        public HttpCookie CreateCookie(string cookieName, string cookieValue, DateTime expirationTime) {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Value = cookieValue;
            cookie.Expires = expirationTime;
            return cookie;
        }

        public HttpCookie UpdateCookie(string cookieName, string value) {
            HttpCookie cookie = null;
            string currentValue = FetchCookieValue(cookieName);

            if(currentValue == "0") {
                cookie = CreateCookie(cookieName, value, DateTime.Now.AddHours(1));
            } else {
                cookie = Request.Cookies[cookieName];
                cookie.Value = value;
            }
            return cookie;
        }

        public void DeleteCookie(string cookieName) {
            Request.Cookies[cookieName].Expires = DateTime.Now.AddSeconds(1);
        }


        public string FetchCookieValue(string cookieName) {
            string value = null;

            try {
                value = Request.Cookies[cookieName].Value;
            } catch {
                value = "0";
            }
            return value;
        }

    }
}