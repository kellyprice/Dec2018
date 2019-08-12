string[] roles = { "Admin", "User" };

var ticket = new FormsAuthenticationTicket(
version: 1,
name: "e66ef343-f337-4f7e-8032-91e9a1227376",
issueDate: DateTime.Now,
expiration: DateTime.Now.AddSeconds(2000),
isPersistent: false,
userData: String.Join("|", roles));

var encryptedTicket = FormsAuthentication.Encrypt(ticket);
var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

Response.Cookies.Add(cookie);

//FormsAuthentication.SetAuthCookie(userSysId.ToString(), false);

if (Url.IsLocalUrl(returnurl) && returnurl.Length > 1 && returnurl.StartsWith("/")
    && !returnurl.StartsWith("//") && !returnurl.StartsWith("/\\"))
{
    return Redirect(returnurl);
}

return RedirectToAction("Index", "Home");


string json = new WebClient().DownloadString("http://localhost:60853/api/getdataapi/GetTestItems");
var items = JsonConvert.DeserializeObject<List<TestItem>>(json);
