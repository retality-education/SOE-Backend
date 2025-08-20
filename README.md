# 🚀 SOE Backend API

## 📋 О проекте

Backend-часть веб-приложения, разработанная на современном стэке технологий .NET. API предоставляет функционал аутентификации, управления пользователями и работы с файлами.

## 🛠 Технологический стэк

### 🔧 Основные технологии
- **Язык программирования**: C# 11.0
- **Фреймворк**: ASP.NET Core 8.0
- **База данных**: PostgreSQL 15+
- **Реальное время**: SignalR
- **Кэширование**: Redis

### 📚 Вспомогательные библиотеки
- **Маппинг объектов**: AutoMapper
- **Аутентификация**: JWT Bearer
- **Логирование**: Serilog

### 🌐 Интеграции со сторонними сервисами
- **Email рассылки**: SendGrid
- **Хранилище файлов**: Cloudinary

## 🔐 API Endpoints

### 🔑 Аутентификация
| Метод | Endpoint | Описание | Авторизация |
|-------|----------|----------|-------------|
| `POST` | `api/auth/register` | Регистрация нового пользователя | ❌ |
| `POST` | `api/auth/login` | Вход в систему | ❌ |
| `POST` | `api/auth/refresh` | Обновление токенов | ❌ |
| `POST` | `api/auth/logout` | Выход из системы | ✅ |

### 👤 Управление пользователями
| Метод | Endpoint | Описание | Авторизация |
|-------|----------|----------|-------------|
| `GET` | `api/user/me` | Получение информации о текущем пользователе | ✅ |
| `POST` | `api/user/forgot-password` | Запрос на сброс пароля | ❌ |
| `POST` | `api/user/reset-password` | Сброс пароля по коду | ❌ |
| `POST` | `api/user/change-password` | Изменение пароля | ✅ |
| `POST` | `api/user/change-avatar` | Изменение аватара пользователя | ✅ |

## 🧪 Тестирование API

### 🛠 Инструменты тестирования
- **Swagger/OpenAPI**: Документация и интерактивное тестирование
- **Postman**: Коллекции запросов и автоматизированное тестирование
- **HTTP-файлы**: Нативные .http файлы для тестирования в VS

### 🚀 Быстрый старт
```http
### Регистрация пользователя
POST {{baseUrl}}/api/auth/register
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePassword123!",
  "userName": "john_doe"
}

### Вход в систему
POST {{baseUrl}}/api/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePassword123!"
}



```
```bash
# ⚙️ Настройка окружения

## 🔧 Требуемые переменные окружения

# Database
ConnectionStrings__DefaultConnection=Host=localhost;Database=soe_db;Username=postgres;Password=your_password

# JWT
JwtOptions__SecretKey=your_super_secret_key_here
JwtOptions__RefreshSecretKey=your_refresh_secret_key_here
JwtOptions__ExpiresHours=2
JwtOptions__RefreshExpiresHours=72

# SendGrid
SendGrid__ApiKey=your_sendgrid_api_key
SendGrid__FromEmail=noreply@yourapp.com
SendGrid__FromName=Your App Name

# Cloudinary
Cloudinary__CloudName=your_cloud_name
Cloudinary__ApiKey=your_api_key
Cloudinary__ApiSecret=your_api_secret

# Redis
Redis__Configuration=localhost:6379
Redis__InstanceName=soe_backend
```

### 🎨 Code style
- **Async/await для всех I/O операций**

- **Clean Architecture principles**

- **Dependency Injection**

- **Repository pattern для доступа к данным**
