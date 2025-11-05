# Cookie-Based Authentication trong ERP System

## 📋 Tổng quan

Project ERP-API sử dụng **Cookie-based Authentication** với ASP.NET Core Identity thay vì JWT tokens.

## 🔐 Cách hoạt động

### 1. Login Flow

```
Client                    Server
  │                         │
  ├─POST /api/accounts/login──▶│
  │  {username, password}   │
  │                         │
  │                     Validate credentials
  │                     Create session
  │                     Generate cookie
  │                         │
  │◀─Set-Cookie: .AspNetCore.Identity.Application=...
  │    200 OK               │
  │                         │
```

### 2. Authenticated Request Flow

```
Client                    Server
  │                         │
  ├─GET /api/products──────▶│
  │  Cookie: .AspNetCore... │
  │                         │
  │                     Validate cookie
  │                     Check session
  │                     Authorize request
  │                         │
  │◀─────200 OK + Data──────┤
  │                         │
```

### 3. Logout Flow

```
Client                    Server
  │                         │
  ├─POST /api/accounts/logout──▶│
  │  Cookie: .AspNetCore... │
  │                         │
  │                     Invalidate session
  │                     Clear cookie
  │                         │
  │◀─Set-Cookie: (expired)──┤
  │    200 OK               │
  │                         │
```

## ⚙️ Configuration

### Program.cs / Startup.cs

```csharp
// Add Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = ".AspNetCore.Identity.Application";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
        options.LoginPath = "/api/accounts/login";
        options.LogoutPath = "/api/accounts/logout";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPS only
    });

// Add Identity
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ErpDbContext>()
    .AddDefaultTokenProviders();
```

## 🍪 Cookie Properties

### Cookie Name

```
.AspNetCore.Identity.Application
```

### Cookie Attributes

| Attribute         | Value  | Purpose                                        |
| ----------------- | ------ | ---------------------------------------------- |
| HttpOnly          | true   | Ngăn JavaScript access cookie (XSS protection) |
| Secure            | true   | Chỉ gửi qua HTTPS                              |
| SameSite          | Lax    | CSRF protection                                |
| Expires           | 7 days | Thời gian tồn tại                              |
| SlidingExpiration | true   | Tự động gia hạn khi dùng                       |

## 📝 Usage Examples

### Testing với Swagger UI

1. **Login**:

   - Go to `/swagger`
   - Execute `POST /api/accounts/login`
   - Enter credentials
   - Cookie tự động được set

2. **Call Protected APIs**:
   - Cookie tự động được gửi với mọi request
   - Không cần thêm Authorization header

### Testing với Postman

1. **Enable Cookie Handling**:

   ```
   Settings → General → Enable "Automatically follow redirects"
   Settings → General → Enable "Send cookies"
   ```

2. **Login**:

   ```http
   POST https://localhost:7012/api/accounts/login
   Content-Type: application/json

   {
     "username": "admin",
     "password": "Admin@123"
   }
   ```

3. **Check Cookie**:

   - Go to Cookies tab
   - Verify `.AspNetCore.Identity.Application` exists

4. **Call Protected API**:
   ```http
   GET https://localhost:7012/api/products
   ```
   Cookie tự động được gửi!

### Testing với cURL

```bash
# 1. Login and save cookies
curl -X POST https://localhost:7012/api/accounts/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"Admin@123"}' \
  -c cookies.txt

# 2. Use saved cookies for protected request
curl -X GET https://localhost:7012/api/products \
  -b cookies.txt
```

### Testing với Browser (JavaScript)

```javascript
// 1. Login
fetch("https://localhost:7012/api/accounts/login", {
  method: "POST",
  headers: {
    "Content-Type": "application/json",
  },
  body: JSON.stringify({
    username: "admin",
    password: "Admin@123",
  }),
  credentials: "include", // IMPORTANT: Send cookies
})
  .then((response) => response.json())
  .then((data) => console.log("Login success:", data));

// 2. Call protected API
fetch("https://localhost:7012/api/products", {
  credentials: "include", // IMPORTANT: Send cookies
})
  .then((response) => response.json())
  .then((data) => console.log("Products:", data));
```

## ✅ Advantages of Cookie-based Authentication

### 1. **Security**

- ✅ HttpOnly flag prevents XSS attacks
- ✅ Secure flag ensures HTTPS only
- ✅ SameSite prevents CSRF attacks
- ✅ Server-side session management

### 2. **Simplicity**

- ✅ Browser handles cookies automatically
- ✅ No need to store tokens in localStorage
- ✅ No need to manually add Authorization headers
- ✅ Built-in with ASP.NET Core Identity

### 3. **User Experience**

- ✅ "Remember me" functionality
- ✅ Sliding expiration (auto-renew)
- ✅ Easy logout (server invalidates session)

## ⚠️ Considerations

### 1. **CORS Configuration**

For frontend apps on different domains:

```csharp
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://your-frontend-domain.com")
              .AllowCredentials()  // IMPORTANT for cookies
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
```

### 2. **Frontend Configuration**

Must include `credentials: 'include'` in fetch/axios:

```javascript
// Fetch API
fetch(url, { credentials: "include" });

// Axios
axios.defaults.withCredentials = true;
```

### 3. **Mobile Apps**

Cookie-based auth works best for:

- ✅ Web applications
- ✅ Server-side rendered apps
- ⚠️ Mobile apps (consider JWT for better control)

## 🔄 Comparison: Cookies vs JWT

| Aspect              | Cookie-based              | JWT                              |
| ------------------- | ------------------------- | -------------------------------- |
| **Storage**         | Server-side session       | Client-side token                |
| **Security**        | More secure (HttpOnly)    | Can be stolen if in localStorage |
| **Stateful**        | Yes                       | No                               |
| **Mobile-friendly** | Less                      | More                             |
| **CORS**            | Need credentials:include  | Simpler                          |
| **Revocation**      | Easy (invalidate session) | Hard (need blacklist)            |
| **Expiration**      | Can extend automatically  | Fixed expiration                 |
| **Scalability**     | Need shared session store | More scalable                    |

## 🎯 Why Cookie-based cho ERP System?

### 1. **Internal System**

- ERP thường là internal system
- Users access qua web browser
- Don't need mobile apps initially

### 2. **Security Priority**

- Sensitive business data
- HttpOnly cookies prevent XSS
- Server-side session control

### 3. **ASP.NET Core Identity**

- Built-in cookie support
- Easy to implement
- Well-tested và secure

### 4. **User Experience**

- "Remember me" for 7 days
- Auto-renew session when active
- No manual token management

## 🔧 Troubleshooting

### Cookie not being set

**Check**:

1. Login response status = 200 OK
2. Response headers có `Set-Cookie`
3. Cookie domain matches current domain
4. HTTPS is being used (if Secure=true)

### Cookie not being sent

**Check**:

1. Browser/tool settings allow cookies
2. `credentials: 'include'` in fetch/axios
3. Cookie hasn't expired
4. Domain và path match

### CORS issues

**Check**:

1. CORS policy includes `AllowCredentials()`
2. Origin is whitelisted
3. Can't use wildcard `*` with credentials

### 401 Unauthorized

**Check**:

1. Cookie is valid (not expired)
2. Session still exists on server
3. User hasn't been deleted/disabled
4. Cookie name matches configuration

## 📚 Resources

- [ASP.NET Core Authentication](https://docs.microsoft.com/aspnet/core/security/authentication/)
- [ASP.NET Core Identity](https://docs.microsoft.com/aspnet/core/security/authentication/identity)
- [Cookie Authentication](https://docs.microsoft.com/aspnet/core/security/authentication/cookie)
- [OWASP Cookie Security](https://owasp.org/www-community/controls/SecureCookieAttribute)

---

**Last Updated**: November 5, 2025  
**Authentication Type**: Cookie-based with ASP.NET Core Identity  
**Cookie Name**: `.AspNetCore.Identity.Application`
