# calculadora-descontos-xunit

Solução **.NET 10** criada via linha de comando (.NET CLI) com **testes unitários parametrizados em xUnit**, demonstrando na prática a diferença entre **`[Fact]`** e **`[Theory]`** com **`[InlineData]`** para cenários com entradas e retornos dos tipos `string`, `int` e `bool`.

> Atividade da disciplina **Garantia da Qualidade de Software** — Prof. Daniel (não é o vorcaro) Henrique Matos de Paiva (Lista de Exercícios 23).

## diferença entre `[Fact]` e `[Theory]`?

| Atributo | O que é | Como executa |
|----------|---------|--------------|
| **`[Fact]`** | Teste **único**, sem parâmetros. Representa um único cenário com valores fixos escritos dentro do próprio método de teste. | Executa **1 vez**. |
| **`[Theory]`** | Teste **parametrizado**. O método recebe parâmetros e os dados vêm de atributos como `[InlineData(...)]` (também existem `[MemberData]` e `[ClassData]`). | Executa **uma vez para cada `[InlineData]`** — cada linha aparece como um teste individual no resultado. |

**Vantagem do `[Theory]`:** em vez de duplicar métodos de teste para cada cenário (como seria necessário com `[Fact]`), escrevemos **um único método** e adicionamos vários `[InlineData(...)]`. Neste projeto, 3 métodos `[Theory]` com 3 `[InlineData]` cada geram **9 testes individuais**.

Exemplo:

```csharp
[Theory]
[InlineData(2, "BRONZE")]
[InlineData(7, "PRATA")]
[InlineData(15, "OURO")]
public void ObterCategoriaCliente_DeveRetornarCategoriaCorreta(int totalCompras, string categoriaEsperada)
{
    var resultado = _descontoService.ObterCategoriaCliente(totalCompras);
    Assert.Equal(categoriaEsperada, resultado);
}
```

## Estrutura da solução

```
calculadora-descontos-xunit/
├── CalculadoraDescontos.slnx           # Solução (gerada por dotnet new sln)
├── CalculadoraDescontos.App/           # Projeto console (código de produção)
│   ├── CalculadoraDescontos.App.csproj
│   ├── DescontoService.cs              # Regras de negócio de descontos
│   └── Program.cs
└── CalculadoraDescontos.Tests/         # Projeto de testes unitários (xUnit)
    ├── CalculadoraDescontos.Tests.csproj
    └── DescontoServiceTests.cs         # Testes com [Theory] + [InlineData]
```

##  Métodos implementados (`DescontoService.cs`)

| # | Assinatura | Retorno | Regra de negócio | Exemplos |
|---|------------|---------|------------------|----------|
| 1 | `ObterCategoriaCliente(int totalCompras)` | `string` | `"BRONZE"` para menos de 5 compras; `"PRATA"` entre 5 e 10 (inclusive); `"OURO"` para mais de 10 | `2` → `"BRONZE"` · `7` → `"PRATA"` · `15` → `"OURO"` |
| 2 | `CalcularDescontoPorPercentual(int valorOriginal, int percentualDesconto)` | `int` | Retorna o valor final com o desconto percentual aplicado | `(100, 10)` → `90` |
| 3 | `EValidoParaCupom(int idade, bool primeiraCompra)` | `bool` | `true` se tiver **18 anos ou mais** OU se for a **primeira compra**; caso contrário, `false` | `(20, false)` → `true` · `(17, false)` → `false` |

##  Cobertura dos testes (`DescontoServiceTests.cs`)

Todos os testes utilizam **`[Theory]`** com múltiplos **`[InlineData]`**:

**Teste 1 (string) — `ObterCategoriaCliente`:**

| `[InlineData]` | Entrada | Saída esperada |
|---------------|---------|----------------|
| 1 | `totalCompras = 2` | `"BRONZE"` |
| 2 | `totalCompras = 7` | `"PRATA"` |
| 3 | `totalCompras = 15` | `"OURO"` |

**Teste 2 (int) — `CalcularDescontoPorPercentual`:**

| `[InlineData]` | valorOriginal | percentualDesconto | valorFinal esperado |
|---------------|---------------|--------------------|---------------------|
| 1 | `100` | `10` | `90` |
| 2 | `200` | `20` | `160` |
| 3 | `50` | `0` | `50` |

**Teste 3 (bool) — `EValidoParaCupom`:**

| `[InlineData]` | idade | primeiraCompra | resultado esperado | cenário |
|---------------|-------|----------------|--------------------|---------|
| 1 | `20` | `false` | `true` | Maior de idade, não é primeira compra |
| 2 | `16` | `true` | `true` | Menor de idade, primeira compra |
| 3 | `17` | `false` | `false` | Menor de idade, não é primeira compra |

**Resultado esperado: 9 testes aprovados (Passed! - 9)** — cada `[InlineData]` roda como um teste individual.

##  Como executar

Pré-requisito: [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) instalado.

```bash
# Na raiz do repositório, execute a suíte de testes:
dotnet test
```

Saída esperada:

```
Passed!  - Failed: 0, Passed: 9, Skipped: 0, Total: 9
```

##  Como a solução foi criada (.NET CLI)

```bash
# 1. Cria a Solução
dotnet new sln -n CalculadoraDescontos

# 2. Cria o projeto da aplicação (Código de Produção)
dotnet new console -n CalculadoraDescontos.App -f net10.0

# 3. Cria o projeto de Testes Unitários com xUnit
dotnet new xunit -n CalculadoraDescontos.Tests -f net10.0

# 4. Adiciona ambos os projetos à Solução
dotnet sln add CalculadoraDescontos.App/CalculadoraDescontos.App.csproj
dotnet sln add CalculadoraDescontos.Tests/CalculadoraDescontos.Tests.csproj

# 5. Adiciona a referência do projeto de Produção no projeto de Testes
dotnet add CalculadoraDescontos.Tests/CalculadoraDescontos.Tests.csproj reference CalculadoraDescontos.App/CalculadoraDescontos.App.csproj
```

## Licença

Este projeto está licenciado sob a [MIT License](LICENSE).
