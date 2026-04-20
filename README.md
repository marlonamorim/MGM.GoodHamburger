# 🍔 Good Hamburger API

API RESTful para gerenciamento de pedidos da hamburgueria Good Hamburger, desenvolvida com **.NET 10** seguindo os princípios de **Clean Architecture** e **CQRS**.

## 📋 Índice

- [Tecnologias](#-tecnologias)
- [Arquitetura](#-arquitetura)
- [Pré-requisitos](#-pré-requisitos)
- [Configuração do Ambiente](#-configuração-do-ambiente)
- [Executando a Aplicação](#-executando-a-aplicação)
- [Endpoints da API](#-endpoints-da-api)
- [Regras de Negócio](#-regras-de-negócio)
- [Testes](#-testes)

---

## 🚀 Tecnologias

- **.NET 10**
- **PostgreSQL 16** (Alpine)
- **Entity Framework Core 10**
- **MediatR** (CQRS Pattern)
- **Swagger/OpenAPI**
- **xUnit** + **FluentAssertions** (Testes)
- **Testcontainers** (Testes de Integração)
- **Docker** & **Docker Compose**

---

## 🏗️ Arquitetura

O projeto segue a **Clean Architecture** organizada em camadas:

```
MGM.GoodHamburger/
├── MGM.GoodHamburger.Api/              # Camada de apresentação (Controllers, Middlewares)
├── MGM.GoodHamburger.Application/      # Casos de uso, Commands, Queries, DTOs
├── MGM.GoodHamburger.Domain/           # Entidades, Interfaces, Regras de Negócio
├── MGM.GoodHamburger.Infrastructure/   # Persistência, Repositórios, DbContext
└── MGM.GoodHamburger.IntegrationTests/ # Testes de integração com Testcontainers
```

**Padrões utilizados:**
- ✅ **CQRS** (Command Query Responsibility Segregation)
- ✅ **Repository Pattern**
- ✅ **Dependency Injection**
- ✅ **Mediator Pattern**

---

## 📦 Pré-requisitos

Antes de começar, certifique-se de ter instalado:

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [Git](https://git-scm.com/)

---

## ⚙️ Configuração do Ambiente

### 1️⃣ Clone o repositório

```bash
git clone https://github.com/seu-usuario/MGM.GoodHamburger.git
cd MGM.GoodHamburger
```

### 2️⃣ Suba o banco de dados PostgreSQL com Docker Compose

Na raiz do projeto `MGM.GoodHamburger.Api`, execute:

```bash
cd MGM.GoodHamburger.Api
docker-compose up -d
```

Isso irá:
- ✅ Criar um container PostgreSQL 16 (Alpine)
- ✅ Expor a porta **5432**
- ✅ Criar o banco de dados **goodhamburger**
- ✅ Configurar usuário: `postgres` / senha: `postgres123`

**Verificar se o container está rodando:**

```bash
docker ps
```

Você deve ver algo como:

```
CONTAINER ID   IMAGE                PORTS                    NAMES
abc123def456   postgres:16-alpine   0.0.0.0:5432->5432/tcp   goodhamburger-postgres
```

### 3️⃣ Aplicar as Migrations

Com o banco de dados rodando, aplique as migrations:

```bash
# Volte para a raiz do projeto
cd ..

# Aplique as migrations
dotnet ef database update --project MGM.GoodHamburger.Infrastructure --startup-project MGM.GoodHamburger.Api
```

Isso irá:
- ✅ Criar as tabelas `MenuItems` e `Orders`
- ✅ Popular o banco com dados de exemplo (seed)

**Dados inseridos automaticamente:**

| ID | Nome | Preço | Tipo |
|----|------|-------|------|
| 1 | X Burger | R$ 5,00 | Sanduíche |
| 2 | X Egg | R$ 4,50 | Sanduíche |
| 3 | X Bacon | R$ 7,00 | Sanduíche |
| 4 | Batata frita | R$ 2,00 | Acompanhamento |
| 5 | Refrigerante | R$ 2,50 | Bebida |

---

## 🎯 Executando a Aplicação

### Opção 1: Via Visual Studio

1. Abra a solução `MGM.GoodHamburger.sln` no Visual Studio
2. Defina `MGM.GoodHamburger.Api` como projeto de inicialização
3. Pressione **F5** ou clique em **Run**
4. O navegador abrirá automaticamente no Swagger: `https://localhost:7098/swagger`

### Opção 2: Via .NET CLI

```bash
cd MGM.GoodHamburger.Api
dotnet run
```

A API estará disponível em:
- **HTTPS**: `https://localhost:7098`
- **HTTP**: `http://localhost:5256`
- **Swagger UI**: `https://localhost:7098/swagger`

---

## 📡 Endpoints da API

### **Menu**

#### `GET /api/menu`
Retorna todos os itens do menu disponíveis.

**Resposta de exemplo:**
```json
{
  "items": [
    {
      "id": 1,
      "name": "X Burger",
      "price": 5.00,
      "type": "Sandwich"
    },
    {
      "id": 4,
      "name": "Batata frita",
      "price": 2.00,
      "type": "SideDish"
    }
  ]
}
```

---

### **Pedidos (Orders)**

#### `POST /api/orders` ⭐
Cria um novo pedido com cálculo automático de descontos.

**Request Body:**
```json
{
  "sandwichId": 1,
  "sideDishId": 4,
  "drinkId": 5
}
```

**Resposta de exemplo:**
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "createdAt": "2024-04-20T15:30:00Z",
  "updatedAt": null,
  "sandwich": {
    "id": 1,
    "name": "X Burger",
    "price": 5.00,
    "type": "Sandwich"
  },
  "sideDish": {
    "id": 4,
    "name": "Batata frita",
    "price": 2.00,
    "type": "SideDish"
  },
  "drink": {
    "id": 5,
    "name": "Refrigerante",
    "price": 2.50,
    "type": "Drink"
  },
  "subtotal": 9.50,
  "discountPercentage": 20,
  "discountAmount": 1.90,
  "total": 7.60
}
```

---

#### `GET /api/orders`
Lista todos os pedidos.

---

#### `GET /api/orders/{id}`
Obtém um pedido específico por ID.

---

#### `PUT /api/orders/{id}`
Atualiza um pedido existente.

**Request Body:**
```json
{
  "sandwichId": 3,
  "sideDishId": null,
  "drinkId": 5
}
```

---

#### `DELETE /api/orders/{id}`
Remove um pedido.

---

## 💰 Regras de Negócio - Descontos Automáticos

A API aplica **descontos automáticos** baseados na combinação de itens do pedido:

### 📊 Tabela de Descontos

| Combinação | Desconto | Exemplo |
|-----------|----------|---------|
| 🍔 **Sanduíche + Acompanhamento** | **10%** | X Burger (R$ 5,00) + Batata (R$ 2,00) = **R$ 6,30** |
| 🍔 **Sanduíche + Bebida** | **15%** | X Egg (R$ 4,50) + Refrigerante (R$ 2,50) = **R$ 5,95** |
| 🍔 **Sanduíche + Acompanhamento + Bebida** | **20%** | X Bacon (R$ 7,00) + Batata (R$ 2,00) + Refrigerante (R$ 2,50) = **R$ 9,20** |

### 📝 Regras de Validação

- ✅ O pedido deve conter **pelo menos 1 item**
- ✅ Apenas itens do tipo correto são aceitos:
  - `SandwichId` → Deve ser um item do tipo **Sandwich**
  - `SideDishId` → Deve ser um item do tipo **SideDish**
  - `DrinkId` → Deve ser um item do tipo **Drink**
- ✅ IDs de itens inexistentes retornam erro **400 Bad Request**

### 🧮 Cálculo do Desconto

```csharp
// Regras implementadas no DiscountCalculator
if (hasSandwich && hasSideDish && hasDrink)
    return 20%; // Combo completo

if (hasSandwich && hasDrink)
    return 15%; // Sanduíche + Bebida

if (hasSandwich && hasSideDish)
    return 10%; // Sanduíche + Acompanhamento

return 0%; // Sem desconto
```

### 💡 Exemplos Práticos

**Exemplo 1: Desconto de 10%**
```bash
curl -X POST https://localhost:7098/api/orders \
  -H "Content-Type: application/json" \
  -d '{
    "sandwichId": 1,
    "sideDishId": 4,
    "drinkId": null
  }'
```

**Cálculo:**
- Subtotal: R$ 7,00 (5,00 + 2,00)
- Desconto: R$ 0,70 (10%)
- **Total: R$ 6,30**

---

**Exemplo 2: Desconto de 20% (Combo Completo)**
```bash
curl -X POST https://localhost:7098/api/orders \
  -H "Content-Type: application/json" \
  -d '{
    "sandwichId": 3,
    "sideDishId": 4,
    "drinkId": 5
  }'
```

**Cálculo:**
- Subtotal: R$ 11,50 (7,00 + 2,00 + 2,50)
- Desconto: R$ 2,30 (20%)
- **Total: R$ 9,20**

---

## 🧪 Testes

### Testes de Integração

Os testes de integração utilizam **Testcontainers** para criar containers PostgreSQL isolados automaticamente.

**Executar todos os testes:**

```bash
dotnet test
```

**Executar apenas testes de integração:**

```bash
dotnet test --filter FullyQualifiedName~IntegrationTests
```

### Estrutura dos Testes

```
MGM.GoodHamburger.IntegrationTests/
├── Infrastructure/
│   └── IntegrationTestBase.cs       # Classe base com Testcontainers
└── UseCases/
    └── Orders/
        └── CreateOrderCommandTests.cs  # Testes de criação de pedidos
```

**Testes implementados:**
- ✅ Desconto de 10% (Sanduíche + Acompanhamento)
- ✅ Desconto de 15% (Sanduíche + Bebida)
- ✅ Desconto de 20% (Combo Completo)

Cada teste:
1. 🐳 Cria um container PostgreSQL isolado
2. 📊 Aplica as migrations
3. ✅ Executa o teste
4. 🗑️ Destrói o container automaticamente

---

## 🛠️ Comandos Úteis

### Docker Compose

```bash
# Subir o banco
docker-compose up -d

# Ver logs
docker-compose logs -f

# Parar o banco
docker-compose stop

# Remover o banco (cuidado: apaga os dados!)
docker-compose down -v
```

### Entity Framework Migrations

```bash
# Criar uma nova migration
dotnet ef migrations add NomeDaMigration \
  --project MGM.GoodHamburger.Infrastructure \
  --startup-project MGM.GoodHamburger.Api

# Aplicar migrations
dotnet ef database update \
  --project MGM.GoodHamburger.Infrastructure \
  --startup-project MGM.GoodHamburger.Api

# Reverter última migration
dotnet ef migrations remove \
  --project MGM.GoodHamburger.Infrastructure \
  --startup-project MGM.GoodHamburger.Api

# Ver lista de migrations
dotnet ef migrations list \
  --project MGM.GoodHamburger.Infrastructure \
  --startup-project MGM.GoodHamburger.Api
```

---

## 📚 Documentação da API

Após executar a aplicação, acesse:

- **Swagger UI**: `https://localhost:7098/swagger`
- **Swagger JSON**: `https://localhost:7098/swagger/v1/swagger.json`

---

## 🤝 Contribuindo

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/MinhaFeature`)
3. Commit suas mudanças (`git commit -m 'Adiciona MinhaFeature'`)
4. Push para a branch (`git push origin feature/MinhaFeature`)
5. Abra um Pull Request

---

## 📄 Licença

Este projeto está sob a licença MIT.

---

## 👨‍💻 Autor

**Marlon Graciano Machado de Amorim**

---

## 📞 Suporte

Se você tiver alguma dúvida ou problema, abra uma [issue](https://github.com/marlonamorim/MGM.GoodHamburger/issues).

---

**Desenvolvido com ❤️ usando .NET 10 e Clean Architecture**
