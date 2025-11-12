# 🤖 GitHub Copilot Instructions — Mercury Platform

This document defines how GitHub Copilot should assist in **developing and maintaining the Mercury Platform** project — including both code generation and commit message conventions — to ensure a professional, consistent, and production-ready workflow.

---

## 🧭 Project Overview

**Mercury Platform** is a distributed, event-driven backend architecture built with **.NET 8**, **RabbitMQ**, **PostgreSQL**, **MongoDB**, and **Redis**.
The platform demonstrates advanced backend engineering concepts such as **Clean Architecture**, **Domain-Driven Design (DDD)**, **CQRS**, and **asynchronous communication** through message queues.

### 🎯 Objectives

* Implement multiple microservices: **Orders**, **Inventory**, **Payments**, and **Checkout Orchestrator**.
* Communicate asynchronously using **RabbitMQ**.
* Apply **DDD** and **CQRS** to separate command and query responsibilities.
* Use **PostgreSQL** for relational persistence, **MongoDB** for read models, and **Redis** for caching and distributed locks.
* Follow **Conventional Commits** and professional coding standards.
* Integrate **MediatR**, **AutoMapper**, **Serilog**, and **Polly**.
* Provide **OpenTelemetry** tracing, **Prometheus/Grafana** observability, and containerized deployment via **Docker**.
* Automate CI/CD pipelines using **GitHub Actions**.

### 🧠 Copilot’s Role

You are assisting as an intelligent coding partner for a **senior backend engineer**.
When generating suggestions or commits, always:

* Write clean, idiomatic, and async C# code.
* Follow **SOLID**, **Clean Architecture**, and **DDD** principles.
* Propose meaningful abstractions (repositories, handlers, value objects, etc.).
* Maintain consistency across microservices.
* Provide short explanations when generating handlers, consumers, or workflows.
* Use professional commit messages (see below).

---

## 🧩 Code Generation Guidelines

1. **Framework & Language**: Always use C# targeting .NET 8.
2. **Architecture**: Follow Clean Architecture and DDD layering (`Domain`, `Application`, `Infrastructure`, `API`).
3. **MediatR Usage**:

   * Commands → Write operations (e.g., `CreateOrderCommandHandler`).
   * Queries → Read operations (e.g., `GetOrderByIdQueryHandler`).
4. **Error Handling**:

   * Use `FluentValidation` for input validation.
   * Apply `Polly` for retries and circuit breakers.
5. **Logging & Telemetry**:

   * Use `Serilog` for structured logs.
   * Use `OpenTelemetry` for traces and metrics.
6. **Persistence**:

   * PostgreSQL → Write models via EF Core.
   * MongoDB → Read models for fast queries.
   * Redis → Caching and locks.
7. **Testing**:

   * Use xUnit + Moq for unit tests.
   * Use Testcontainers for integration tests.
8. **Documentation**:

   * Generate XML summaries for public classes and methods.
   * Maintain API documentation using Swagger.

---

## 🧾 Commit Message Rules

When generating commit messages, always follow the **Conventional Commits** format:

```
<type>(<scope>): <short summary>
```

### **Allowed `<type>` values:**

* `feat` – New feature
* `fix` – Bug fix
* `refactor` – Code restructuring without behavior change
* `chore` – Maintenance, tooling, or config updates
* `docs` – Documentation only
* `test` – Adding or updating tests
* `perf` – Performance improvements
* `style` – Code formatting, naming, or whitespace changes
* `build` – CI/CD or dependency updates
* `revert` – Reverting a previous commit

### **Accepted `<scope>` values:**

`orders`, `payments`, `inventory`, `orchestrator`, `infra`, `core`, `docs`, `tests`, `ci`

### **Formatting Guidelines:**

* Use **present tense** verbs (e.g., `add`, `update`, `remove`).
* Keep summary lines **under 72 characters**.
* Include a body (3–5 lines) explaining **what** changed and **why**.
* Reference issues in the footer using `Closes #<issue-number>`.
* Avoid prefixes like `#12` at the top.
* Wrap lines for readability in CLI tools.

### **Commit Template Example:**

```
refactor(core): remove weather forecast functionality

Simplifies the base application by removing weather forecast-related code,
including the `/weatherforecast` endpoint and `WeatherForecast` record.
Retains Swagger and HTTPS configuration.

Closes #12
```

### **Bad Examples (Avoid):**

```
#12 fixed stuff
update files
minor bugfix for order system
```

---

## 🧱 Commit Tone

* Be **specific**, **concise**, and **neutral-professional**.
* Explain *what* and *why*, not *how*.
* Avoid emojis or informal language.

---

## 🧰 Summary Template

```
<type>(<scope>): <short summary>

<detailed explanation of what changed>
<reason for the change, if applicable>
<impact or follow-up notes>

Closes #<issue-number>
```

---

## 🚀 Startup Prompt for Visual Studio Copilot Chat

Paste this prompt in Copilot Chat before starting development to give context:

```
You are assisting in the development of the Mercury Platform project.

Mercury Platform is a distributed, event-driven backend built with .NET 8,
RabbitMQ, PostgreSQL, MongoDB, and Redis. The goal is to showcase clean
architecture, DDD, CQRS, and microservices best practices.

Objectives:
- Multiple microservices (Orders, Inventory, Payments, Orchestrator)
- RabbitMQ for event-driven communication
- PostgreSQL, MongoDB, Redis for persistence and caching
- MediatR, AutoMapper, Serilog, Polly, OpenTelemetry integration
- Dockerized environment and GitHub Actions CI/CD

Copilot’s responsibilities:
- Generate idiomatic, async C# code with clean architecture
- Follow SOLID and DDD principles
- Use MediatR for commands/queries
- Suggest clean abstractions and patterns
- Produce Conventional Commit messages
```

---

✅ Following these rules ensures **consistent code quality**, **semantic commits**, and a **professional workflow** across the entire Mercury Platform project.