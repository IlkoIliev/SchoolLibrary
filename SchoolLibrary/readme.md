# 📚 SchoolLibrary – ASP.NET Core MVC + Entity Framework Core (Database First)

## 🧩 Описание на проекта

**SchoolLibrary** е учебно уеб приложение за управление на училищна библиотека.
Проектът демонстрира реална архитектура с:

* ASP.NET Core MVC
* Entity Framework Core (Database First подход)
* Repository + Service архитектура
* SQL Server база данни
* Razor Views (Bootstrap UI)

Приложението позволява управление на:

* 📖 Книги
* ✍️ Автори
* 👨‍🎓 Ученици
* 🔄 Заеми на книги (Loans)

---

## 🎯 Основни функционалности

### 📚 Books

* CRUD операции за книги
* Свързване с автор
* Година на издаване
* Визуализация на книги с автор

### ✍️ Authors

* CRUD операции за автори
* Свързване с книги

### 👨‍🎓 Students

* Регистрация на ученици
* Клас на ученика
* Използват се при заемане на книги

### 🔄 Loans (Заеми)

* Създаване на заем
* Връщане на книга
* Активни заеми
* История на заемите
* Бизнес правило: книга не може да бъде заемана два пъти едновременно

---

## 🏗️ Архитектура

Проектът използва многослойна структура:

### Data Layer

* DbContext
* Entity класове (Database First)

### Repository Layer

* Достъп до база данни
* CRUD операции

### Service Layer

* Бизнес логика
* Валидации
* Обработка на заявки

### Presentation Layer (MVC)

* Controllers
* ViewModels
* Razor Views

---

## 🗄️ База данни

SQL Server база със следните основни таблици:

* **Books**
* **Authors**
* **Students**
* **Loans**

Връзки:

* Author → Books (1:N)
* Student → Loans (1:N)
* Book → Loans (1:N)

---

## 🚀 Стартиране на проекта

### 1. Clone repository:

```bash
git clone <repo-url>
```

### 2. Настрой connection string:

В `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=...;Database=SchoolLibrary;Trusted_Connection=True;"
}
```

### 3. Стартирай проекта:

* Visual Studio → Run
  или:

```bash
dotnet run
```

---

## 🧑‍🏫 Учебна цел

Проектът е създаден като учебно помагало за:

* Интернет програмиране
* ORM с Entity Framework Core
* Database First подход
* MVC архитектура
* Реална бизнес логика

Подходящ за:

* ученици 11–12 клас „Приложен програмист“
* студенти начално ниво
* практически упражнения

---

## 🔮 Възможни бъдещи разширения

* Жанрове на книги
* Pagination и Search
* Authentication / роли
* REST API версия
* Отчети и статистики
* Docker deployment

---

## 👨‍💻 Автор

Учебен проект за демонстрация на разработка на уеб приложение с ASP.NET Core MVC и EF Core.

---
