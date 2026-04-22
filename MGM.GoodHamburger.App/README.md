# 🍔 Good Hamburger - Aplicação Web (Blazor WebAssembly)

Aplicação web desenvolvida em **Blazor WebAssembly (.NET 10)** para visualização do cardápio, gerenciamento de carrinho e criação de pedidos da hamburgueria Good Hamburger.

## 🚀 Funcionalidades

- ✅ **Visualização do Cardápio** com abas separadas por categoria:
  - 🍔 **Hambúrgueres**
  - 🍟 **Acompanhamentos**
  - 🥤 **Bebidas**

- ✅ **Carrinho de Compras** com:
  - Adicionar itens do cardápio
  - Ajustar quantidade (aumentar/diminuir)
  - Remover itens
  - Cálculo automático de descontos (10%, 15%, 20%)
  - Visualização de subtotal, desconto e total

- ✅ **Criação de Pedidos** via integração com API REST

- ✅ **Listagem de Pedidos** com:
  - Histórico completo de pedidos
  - Ordenação por data (mais recente primeiro)
  - Detalhamento de itens, descontos e valores
  - Estatísticas (total de pedidos, economia total, valor total)
  - Atualização manual da lista

- ✅ **Informações de Descontos em tempo real**:
  - 10% OFF - Hambúrguer + Acompanhamento
  - 15% OFF - Hambúrguer + Bebida
  - 20% OFF - Combo Completo (Hambúrguer + Acompanhamento + Bebida)

- ✅ **Design Responsivo** com Bootstrap 5
- ✅ **Feedback Visual** com mensagens de sucesso e erro

---

## 📋 Pré-requisitos

Antes de executar, certifique-se de que:

1. ✅ A **API** (`MGM.GoodHamburger.Api`) está rodando em `https://localhost:7098`
2. ✅ O **banco de dados PostgreSQL** está ativo (via Docker Compose)
3. ✅ As **migrations** foram aplicadas

---

## ⚙️ Configuração

### 1️⃣ Configurar URL da API

Edite o arquivo `wwwroot/appsettings.json`:

```json
{
  "ApiSettings": {
    "BaseUrl": "https://localhost:7098"
  }
}
```

### 2️⃣ Verificar CORS na API

Certifique-se de que a API está configurada com CORS habilitado para a URL do Blazor App.

No arquivo `MGM.GoodHamburger.Api/Program.cs`, deve conter:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorApp", policy =>
    {
        policy.WithOrigins("https://localhost:7242", "http://localhost:5046")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ...

app.UseCors("AllowBlazorApp");
```

---

## 🎯 Como Executar

### Via Visual Studio

1. Defina `MGM.GoodHamburger.App` como projeto de inicialização
2. Pressione **F5** ou clique em **Run**
3. O navegador abrirá automaticamente

### Via .NET CLI

```bash
cd MGM.GoodHamburger.App
dotnet run
```

A aplicação estará disponível em:
- **HTTPS**: `https://localhost:7242`
- **HTTP**: `http://localhost:5046`

---

## 📱 Navegação

- **Cardápio** (`/` ou `/menu`) - Página inicial com visualização do cardápio
- **Carrinho** (`/cart`) - Visualização e gerenciamento do carrinho de compras
- **Pedidos** (`/orders`) - Histórico de pedidos realizados
- **Home** (`/home`) - Página inicial alternativa

**Acesso Rápido:**
- Menu lateral: Links para Cardápio, Carrinho e Pedidos
- Topo da página: Ícone de carrinho com badge
- Badge do carrinho: Mostra quantidade de itens em tempo real

---

## 🛒 Como Usar a Aplicação

### 1️⃣ Navegar pelo Cardápio
1. Acesse a página de **Cardápio** (`/`)
2. Navegue pelas abas:
   - 🍔 **Hambúrgueres**
   - 🍟 **Acompanhamentos**
   - 🥤 **Bebidas**
3. Visualize os itens disponíveis com preços

### 2️⃣ Adicionar Itens ao Carrinho
1. Clique no botão **"Adicionar"** ao lado do item desejado
2. O badge do carrinho será atualizado automaticamente
3. Continue adicionando itens para montar seu pedido

### 3️⃣ Gerenciar Carrinho
- **Aumentar quantidade**: Botão `+`
- **Diminuir quantidade**: Botão `-`
- **Remover item**: Botão 🗑️ (lixeira)
- **Limpar tudo**: Botão "Limpar Carrinho"

### 4️⃣ Visualizar Descontos
Os descontos são aplicados **automaticamente** conforme você adiciona itens:

| Combinação | Desconto | Exibição |
|-----------|----------|----------|
| Hambúrguer + Acompanhamento | 10% | 💚 Badge verde |
| Hambúrguer + Bebida | 15% | 💚 Badge verde |
| Hambúrguer + Acompanhamento + Bebida | 20% | 💚 Badge verde |

### 5️⃣ Finalizar Pedido
1. Revise os itens no carrinho
2. Confira o desconto aplicado e o total
3. Clique em **"Finalizar Pedido"**
4. Aguarde o processamento (indicador de loading)
5. Veja a mensagem de sucesso com o ID do pedido
6. O carrinho será limpo automaticamente

### 6️⃣ Visualizar Histórico de Pedidos
1. Acesse a página de **Pedidos** (`/orders`)
2. Visualize todos os pedidos ordenados por data (mais recente primeiro)
3. Veja detalhes de cada pedido:
   - ID do pedido
   - Data e hora
   - Itens incluídos
   - Desconto aplicado
   - Valores (subtotal, desconto, total)
4. Confira as estatísticas:
   - Total de pedidos realizados
   - Economia total com descontos
   - Valor total gasto
5. Clique em **"Atualizar"** para recarregar a lista

---

## 🎨 Páginas da Aplicação

### 📋 Cardápio (`/` ou `/menu`)
- Visualização de todos os itens do cardápio
- 3 abas organizadas por categoria
- Botão "Adicionar" em cada item
- Badge mostrando quantidade de itens por categoria
- Informações sobre descontos disponíveis

### 🛒 Carrinho (`/cart`)
- Listagem de itens adicionados
- Controles de quantidade (+/-)
- Botão para remover itens
- Cálculo automático de subtotal, desconto e total
- Alert dinâmico mostrando desconto aplicado
- Sugestões para aumentar o desconto
- Botão "Finalizar Pedido" com feedback visual

### 📊 Pedidos (`/orders`)
- Tabela com histórico completo
- Ordenação por data (mais recente primeiro)
- Detalhamento completo de cada pedido
- Cards de estatísticas
- Botão de atualização manual
| **Acompanhamentos** | 🍟 | Batatas, onion rings, etc. | `SideDish` |
| **Bebidas** | 🥤 | Refrigerantes, sucos, etc. | `Drink` |

### Informações Exibidas

Para cada item:
- **ID** do produto
- **Nome** do item
- **Preço** formatado em moeda brasileira (R$)

### Badge de Contagem

Cada aba exibe um **badge** com a quantidade de itens disponíveis:
- Exemplo: `Hambúrgueres (3)`

---

## 🔗 Integração com a API

### Endpoint Utilizado

```
GET https://localhost:7098/api/menu
```

### Resposta Esperada

```json
[
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
  },
  {
    "id": 5,
    "name": "Refrigerante",
    "price": 2.50,
    "type": "Drink"
  }
]
```

---

## 🛠️ Estrutura do Projeto

```
MGM.GoodHamburger.App/
├── Models/
│   └── MenuItemDto.cs          # DTO para itens do menu
├── Services/
│   └── MenuService.cs          # Serviço para chamar a API
├── Pages/
│   ├── Menu.razor              # Página do cardápio com abas
│   ├── Home.razor
│   ├── Counter.razor
│   └── Weather.razor
├── Layout/
│   ├── MainLayout.razor        # Layout principal
│   └── NavMenu.razor           # Menu de navegação
└── wwwroot/
    ├── appsettings.json        # Configurações da aplicação
    └── index.html              # HTML principal
```

---

## 💡 Dicas de Desenvolvimento

### Modificar Estilos

Os estilos podem ser editados em:
- `wwwroot/css/app.css` - Estilos globais
- `Layout/NavMenu.razor.css` - Estilos do menu de navegação
- `Layout/MainLayout.razor.css` - Estilos do layout

### Adicionar Ícones

O projeto usa **Bootstrap Icons**. Veja a lista completa em:
https://icons.getbootstrap.com/

Exemplo:
```html
<i class="bi bi-card-list"></i>
```

---

## 🐛 Troubleshooting

### Erro: "Failed to fetch"

**Causa**: A API não está rodando ou o CORS não está configurado.

**Solução**:
1. Certifique-se de que a API está rodando
2. Verifique a configuração do CORS na API
3. Verifique a URL no `appsettings.json`

### Erro: "No menu items found"

**Causa**: O banco de dados pode estar vazio.

**Solução**:
1. Execute as migrations: `dotnet ef database update`
2. Verifique se o PostgreSQL está rodando
3. Verifique os logs da API

### Página em branco

**Causa**: Erro de JavaScript ou problema de roteamento.

**Solução**:
1. Abra o console do navegador (F12)
2. Verifique erros no console
3. Limpe o cache do navegador (Ctrl+Shift+Del)

---

## 📚 Tecnologias Utilizadas

- **Blazor WebAssembly** (.NET 10)
- **Bootstrap 5** (UI Framework)
- **Bootstrap Icons** (Ícones)
- **HttpClient** (Chamadas HTTP)

---

## 🤝 Contribuindo

1. Faça alterações apenas nos arquivos necessários
2. Teste as mudanças localmente
3. Commit com mensagens descritivas
4. Abra um Pull Request

---

**Desenvolvido com ❤️ usando Blazor WebAssembly e .NET 10**
