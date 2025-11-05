# 🍪 Cookie Authentication - Quick Reference

## 🚀 Quick Start

### 1. Login

```http
POST /api/accounts/login
Content-Type: application/json

{
  "username": "admin",
  "password": "Admin@123"
}

Response: 200 OK
Set-Cookie: .AspNetCore.Identity.Application=CfDJ8...
```

### 2. Access Protected API

```http
GET /api/products
Cookie: .AspNetCore.Identity.Application=CfDJ8...
(Automatically sent by browser)

Response: 200 OK + Product data
```

### 3. Logout

```http
POST /api/accounts/logout
Cookie: .AspNetCore.Identity.Application=CfDJ8...

Response: 200 OK
Set-Cookie: .AspNetCore.Identity.Application=; expires=...
```

---

## 🧪 Testing Tools

### Swagger UI

```
✅ Cookies handled automatically
1. Login via Swagger
2. Cookie auto-set
3. All requests include cookie
```

### Postman

```
✅ Enable "Send cookies" in Settings
1. POST /api/accounts/login
2. Check Cookies tab
3. GET /api/products (auto-sent)
```

### cURL

```bash
# Save cookie
curl -c cookies.txt -X POST .../login -d '{...}'

# Use cookie
curl -b cookies.txt -X GET .../products
```

### Browser (JavaScript)

```javascript
// Must include credentials: 'include'
fetch('/api/login', {
  method: 'POST',
  credentials: 'include', // ← Important!
  body: JSON.stringify({...})
});
```

---

## 🔐 Cookie Properties

```
Name:      .AspNetCore.Identity.Application
Domain:    localhost / your-domain.com
Path:      /
Expires:   7 days
HttpOnly:  true  ← Prevents JavaScript access
Secure:    true  ← HTTPS only
SameSite:  Lax   ← CSRF protection
```

---

## ⚠️ Common Issues

### Cookie not set

- ✅ Check login returns 200 OK
- ✅ Check response has Set-Cookie header
- ✅ Check HTTPS is used (if Secure=true)

### Cookie not sent

- ✅ Check `credentials: 'include'` in fetch
- ✅ Check cookie hasn't expired
- ✅ Check domain matches

### 401 Unauthorized

- ✅ Login first
- ✅ Check cookie is valid
- ✅ Check session exists on server

### CORS issues

- ✅ Add `AllowCredentials()` in CORS policy
- ✅ Whitelist frontend domain
- ✅ Cannot use wildcard `*` with credentials

---

## 🔄 Cookie vs JWT

| Feature       | Cookie     | JWT         |
| ------------- | ---------- | ----------- |
| **Security**  | ⭐⭐⭐⭐⭐ | ⭐⭐⭐      |
| **Auto-send** | ✅ Yes     | ❌ No       |
| **Revoke**    | ✅ Easy    | ❌ Hard     |
| **ERP use**   | ✅ Perfect | ⚠️ Overkill |

---

## 📚 Full Documentation

- **COOKIE_AUTHENTICATION.md** - Complete guide
- **QUICK_START.md** - Getting started
- **Modules/Identity/README.md** - Identity module

---

## 💡 Pro Tips

### Development

```csharp
// appsettings.Development.json
// Disable RequireHttps for local testing
options.Cookie.SecurePolicy = CookieSecurePolicy.None;
```

### Production

```csharp
// Always use HTTPS
options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
options.Cookie.Domain = "your-domain.com";
```

### Frontend (React/Vue/Angular)

```javascript
// axios config
axios.defaults.withCredentials = true;
axios.defaults.baseURL = "https://api.domain.com";

// fetch config
const fetchWithAuth = (url, options = {}) => {
  return fetch(url, {
    ...options,
    credentials: "include",
  });
};
```

---

## 🎯 Key Takeaways

✅ **Cookie-based = Simple & Secure** cho ERP  
✅ **HttpOnly** prevents XSS attacks  
✅ **Browser handles automatically**  
✅ **Server controls sessions**  
✅ **Easy revocation** (logout = delete session)  
✅ **Built-in ASP.NET Identity**

---

**Cookie Name**: `.AspNetCore.Identity.Application`  
**Expiration**: 7 days (sliding)  
**Updated**: November 5, 2025
