using CalculadoraDescontos.App;
using Xunit;

namespace CalculadoraDescontos.Tests;

/// <summary>
/// Testes parametrizados do <see cref="DescontoService"/> utilizando
/// [Theory] + [InlineData]: cada [InlineData] executa como um teste individual.
/// </summary>
public class DescontoServiceTests
{
    private readonly DescontoService _descontoService = new();

    // ---------------------------------------------------------------
    // Teste 1 (retorno string): valida todas as categorias de cliente
    // ---------------------------------------------------------------
    [Theory]
    [InlineData(2, "BRONZE")]   // menos de 5 compras  -> BRONZE
    [InlineData(7, "PRATA")]    // entre 5 e 10        -> PRATA
    [InlineData(15, "OURO")]    // mais de 10 compras  -> OURO
    public void ObterCategoriaCliente_DeveRetornarCategoriaCorreta(int totalCompras, string categoriaEsperada)
    {
        // Act
        var resultado = _descontoService.ObterCategoriaCliente(totalCompras);

        // Assert
        Assert.Equal(categoriaEsperada, resultado);
    }

    // ---------------------------------------------------------------
    // Teste 2 (retorno int): valida o cálculo do desconto percentual
    // ---------------------------------------------------------------
    [Theory]
    [InlineData(100, 10, 90)]   // 100 com 10% de desconto -> 90
    [InlineData(200, 20, 160)]  // 200 com 20% de desconto -> 160
    [InlineData(50, 0, 50)]     // 50 sem desconto         -> 50
    public void CalcularDescontoPorPercentual_DeveAplicarDescontoCorretamente(int valorOriginal, int percentualDesconto, int valorFinalEsperado)
    {
        // Act
        var resultado = _descontoService.CalcularDescontoPorPercentual(valorOriginal, percentualDesconto);

        // Assert
        Assert.Equal(valorFinalEsperado, resultado);
    }

    // ---------------------------------------------------------------
    // Teste 3 (retorno bool): valida a elegibilidade para o cupom
    // ---------------------------------------------------------------
    [Theory]
    [InlineData(20, false, true)]   // maior de idade, não é primeira compra -> true
    [InlineData(16, true, true)]    // menor de idade, primeira compra       -> true
    [InlineData(17, false, false)]  // menor de idade, não é primeira compra -> false
    public void EValidoParaCupom_DeveValidarElegibilidadeCorretamente(int idade, bool primeiraCompra, bool resultadoEsperado)
    {
        // Act
        var resultado = _descontoService.EValidoParaCupom(idade, primeiraCompra);

        // Assert
        Assert.Equal(resultadoEsperado, resultado);
    }
}
