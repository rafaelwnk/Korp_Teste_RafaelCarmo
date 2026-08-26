# Korp Teste - Rafael Carmo

Sistema de emissão de Notas Fiscais desenvolvido como parte do processo seletivo da Korp ERP. A solução é composta por dois microsserviços em .NET 10 (Estoque e Faturamento), um Gateway com proxy reverso, frontend em Angular 22 e banco de dados PostgreSQL.

## Sumário

- [Arquitetura](#arquitetura)
- [Como rodar o projeto](#como-rodar-o-projeto)
- [Funcionalidades implementadas](#funcionalidades-implementadas)
- [Detalhamento técnico](#detalhamento-técnico)
  - [Frontend (Angular)](#frontend-angular)
  - [Backend (.NET)](#backend-net)
- [Tratamento de falhas entre microsserviços](#tratamento-de-falhas-entre-microsserviços)
- [Requisitos opcionais](#requisitos-opcionais)

## Arquitetura

O sistema segue uma arquitetura de microsserviços com um gateway único como ponto de entrada para o frontend:

```
                          ┌───────────────────┐
                          │      Frontend     │
                          └──────────┬────────┘
                                     │
                          ┌──────────▼─────────┐
                          │   Gateway (YARP)   │
                          └───────┬────────┬───┘
                    /products/**  │        │  /invoices/**
                          ┌───────▼───┐ ┌──▼─────────┐
                          │ Inventory │ │  Billing   │
                          │  Api      │ │  Api       │
                          └─────┬─────┘ └─────┬──────┘
                                │             │  HTTP (HttpClient)
                                │             │
                                │◄────────────┘
                          ┌─────▼──────┐ ┌────────────┐
                          │ korp_      │ │ korp_      │
                          │ inventory  │ │ billing    │
                          │(PostgreSQL)│ │(PostgreSQL)│
                          └────────────┘ └────────────┘
```

- **Gateway.Api** — reverse proxy (YARP), único ponto de entrada exposto ao frontend, roteando `/products/**` para o serviço de Estoque e `/invoices/**` para o serviço de Faturamento. Também centraliza a política de CORS.
- **Inventory (Serviço de Estoque)** — CRUD de produtos e ajuste de saldo (`increase`/`decrease`).
- **Billing (Serviço de Faturamento)** — CRUD de notas fiscais, inclusão/remoção de itens e fechamento da nota, que consome o serviço de Estoque via HTTP para dar baixa nos produtos.
- Cada microsserviço tem seu **próprio banco PostgreSQL** (`korp_inventory` e `korp_billing`), reforçando o isolamento entre os serviços.
- Ambos os backends seguem **Clean Architecture** (`Api` → `Application` → `Domain` ← `Infrastructure`), com a camada de domínio livre de dependências externas.

## Como rodar o projeto

### Pré-requisitos
- [.NET SDK 10](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) 20+ e npm
- [Angular CLI](https://angular.dev/tools/cli) (`npm install -g @angular/cli`)
- PostgreSQL

### Backend

Cada serviço aplica suas próprias migrations via CLI:

```bash
cd backend/src/Inventory/Inventory.Api
dotnet ef database update -p backend/src/Inventory/Inventory.Infrastructure
dotnet run

cd backend/src/Billing/Billing.Api
dotnet ef database update -p backend/src/Billing/Billing.Infrastructure
dotnet run

cd backend/src/Gateway/Gateway.Api
dotnet run
```

Documentação interativa das APIs disponível via Scalar em `/scalar` em cada serviço.

### Frontend

```bash
cd frontend
npm install
ng serve
```

O frontend consome exclusivamente o Gateway (`environment.apiUrl`), nunca os serviços de Estoque/Faturamento diretamente.

## Funcionalidades implementadas

### Cadastro de Produtos
- Campos: código (único), descrição, saldo em estoque.
- Listagem paginada, criação, edição de descrição, ajuste de saldo (aumentar/diminuir) e exclusão.

### Cadastro de Notas Fiscais
- Numeração sequencial gerada pelo banco (`IDENTITY` no PostgreSQL).
- Status `Aberta`/`Fechada`, com regras de domínio que impedem alterar itens ou fechar uma nota que não esteja aberta.
- Inclusão e remoção de múltiplos produtos com suas respectivas quantidades enquanto a nota está aberta.

### Fechamento de Notas Fiscais
- Botão de fechamento na tela de edição da nota, com o formulário desabilitado durante o processamento.
- Ao fechar, a nota muda de status para `Fechada` e o serviço de Faturamento chama o serviço de Estoque para dar baixa no saldo de cada produto da nota (`PATCH /products/{id}/decrease`), refletindo o exemplo do enunciado (saldo 10 − 2 unidades = saldo 8).
- Não é possível fechar uma nota que não esteja `Aberta`, nem uma nota sem itens.

## Detalhamento técnico

### Frontend (Angular)

**Versão:** Angular 22

**Ciclos de vida do Angular:** o projeto não utiliza os hooks tradicionais(`ngOnInit`, `ngOnChanges`, etc.). Em vez disso, a aplicação foi construída inteiramente sobre as primitivas reativas mais recentes do Angular:
- `signal()` para estado local mutável (ex.: `page`, `pageSize`, `selectedProduct`);
- `input()` / `input.required()` e `output()` para comunicação entre componentes, substituindo `@Input`/`@Output`;
- `computed()` para valores derivados (ex.: `pages` no componente de paginação);
- `httpResource()` para buscar dados diretamente ligados a signals, dispensando a necessidade de disparar chamadas manuais em `ngOnInit` — a busca é refeita automaticamente sempre que os signals dos quais a URL depende (`page`, `pageSize`, `productId`) mudam.

**RxJS:** usado de forma pontual, apenas no `errorInterceptor` (`core/interceptors/error.interceptor.ts`), interceptando toda resposta HTTP com `catchError` para exibir uma notificação de erro (via `ngx-toastr`) com mensagem amigável por status HTTP, e repropagando o erro com `throwError`. As chamadas de mutação (`create`, `increaseStock`, `close`, etc.) usam `Observable` do `HttpClient` e são subscritas diretamente nos componentes.

**Outras bibliotecas:**
- `ngx-toastr` — notificações de sucesso/erro.
- `@angular/forms` (Reactive Forms) — formulários de criação/edição, com `FormGroup`/`FormControl` e `Validators`.

**Componentes visuais:** UIkit, carregado via CDN diretamente no index.html (CSS, JS e o pacote de ícones uikit-icons), fornecendo a base de estilo (grid, botões, cores) e o comportamento de UI (modais, altura de viewport). Os componentes de mais alto nível reutilizados entre as telas (modal, confirm-modal, pagination) foram desenvolvidos internamente em shared/components, compondo os elementos do UIkit em vez de usar um wrapper Angular específico da biblioteca.

**Roteamento:** lazy loading das páginas via `loadComponent()` em `app.routes.ts`, com um layout principal (`MainLayout` + `Sidebar`) envolvendo as páginas de Produtos e Notas Fiscais.

### Backend (.NET)

**Framework:** ASP.NET Core Minimal APIs (sem Controllers) em .NET 10, em todos os três serviços (`Gateway`, `Inventory.Api`, `Billing.Api`).

**Gateway:** implementado com YARP (Yarp.ReverseProxy), roteando por prefixo de path para os clusters `inventory-cluster` e `billing-cluster`, e centralizando a política de CORS liberada para a origem do frontend.

**Persistência:** Entity Framework Core com Npgsql (PostgreSQL), um `AppDbContext` por serviço, mapeamentos explícitos via `IEntityTypeConfiguration<T>` (Fluent API) em vez de Data Annotations.

**Tratamento de erros e exceções:**
- Uma hierarquia de exceções de domínio (`DomainException` e subtipos como `InsufficientStockException`, `InvalidInvoiceStatusException`) é lançada pelas entidades quando uma regra de negócio é violada.
- Um `ResultFactory.Try<T>` centraliza a captura dessas exceções e as converte num objeto `Result<T>` (`IsSuccess`/`Data`/`Message`), evitando `try/catch` espalhado pelos Services.
- Os endpoints nunca lidam com exceções diretamente: extensions (`ResultExtensions`) traduzem o `Result<T>` para o código HTTP apropriado (`Ok`, `NotFound`, `BadRequest`, `Created`, `NoContent`).
- Erros de comunicação entre microsserviços (indisponibilidade, timeout) são tratados explicitamente no `InventoryServiceClient` do serviço de Faturamento (ver seção seguinte).

**LINQ:** utilizado nas camadas de aplicação para projeção e composição de coleções, por exemplo:
- Mapeamento de listas de entidades para DTOs com `.Select(...)` dentro de *collection expressions* (`[.. produtos.Select(p => p.ToDto())]`) em `ProductMappingExtensions` e `InvoiceMappingExtensions`;
- Consultas com `Where`/`FirstOrDefault`/`AnyAsync`/`OrderBy`/`Skip`/`Take` no EF Core para paginação, busca por código único e checagem de existência (ex.: `ProductService.GetAsync`, `CreateAsync`).

**Gerenciamento de dependências:** `Microsoft.EntityFrameworkCore` + `Npgsql.EntityFrameworkCore.PostgreSQL`, `Yarp.ReverseProxy` (Gateway), `Scalar.AspNetCore` (documentação OpenAPI interativa em desenvolvimento).

## Tratamento de falhas entre microsserviços

O requisito obrigatório de simular a falha de um microsserviço e recuperar com feedback apropriado foi implementado no fluxo de fechamento de nota fiscal, que depende do serviço de Estoque:

1. `InvoiceService.CloseAsync` (Billing) fecha a nota localmente e, em seguida, para cada item, chama `IInventoryServiceClient.DecreaseStockAsync`, que faz uma requisição HTTP ao serviço de Estoque.
2. O `InventoryServiceClient` (`Billing.Infrastructure`) está preparado para dois cenários de falha:
   - **Serviço indisponível ou timeout** (`HttpRequestException`/`TaskCanceledException`, com `HttpClient.Timeout` configurado em 5s): a exceção é capturada, registrada em log, e convertida numa mensagem de erro amigável — *"The invoice could not be closed because the Inventory service is unavailable. Please try again."*
   - **Resposta de erro de negócio** (ex.: saldo insuficiente): o corpo da resposta é lido e a mensagem original do serviço de Estoque é repassada ao usuário.
3. Em ambos os casos, o `InvoiceService` interrompe o fechamento e retorna um `Result` de erro, que a API converte em `400 Bad Request` — o frontend captura isso no `errorInterceptor` e exibe a mensagem via toast, dando feedback claro ao usuário sobre a falha.

## Requisitos opcionais

### Tratamento de concorrência

Implementado no serviço de Estoque, usando concorrência otimista nativa do PostgreSQL:

- A coluna de sistema `xmin` (que o PostgreSQL atualiza automaticamente a cada `UPDATE` de uma linha) é mapeada no EF Core como *concurrency token* (`Property<uint>("xmin").IsRowVersion()`), sem exigir uma coluna própria de versionamento.
- Quando duas requisições tentam decrementar o mesmo produto simultaneamente, a segunda a chegar ao `SaveChangesAsync()` recebe um `DbUpdateConcurrencyException` do EF Core, pois seu `xmin` lido já está desatualizado.
- `ResultFactory.TryWithConcurrencyRetryAsync` encapsula a operação inteira (recarregar produto → aplicar a regra de domínio → salvar) num laço de retry (até 10 tentativas, com pequeno *delay* incremental entre elas), limpando o `ChangeTracker` a cada tentativa para garantir que o produto seja relido do banco com o saldo mais atual.
- Isso garante que, no cenário do enunciado (saldo 1, duas notas concorrentes), apenas uma operação seja bem-sucedida "de primeira" — a outra é automaticamente reprocessada contra o saldo já atualizado, podendo terminar em sucesso (se ainda houver saldo) ou em erro de negócio real (`InsufficientStockException`), nunca em uma leitura desatualizada silenciosa (*lost update*).
