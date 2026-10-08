namespace CalculadoraDescontos.App;

/// <summary>
/// Serviço com as regras de negócio da calculadora de descontos:
/// categoria do cliente, desconto percentual e elegibilidade para cupom.
/// </summary>
public class DescontoService
{
    /// <summary>
    /// Retorna a categoria do cliente com base no total de compras:
    /// "BRONZE" para menos de 5 compras,
    /// "PRATA" para compras entre 5 e 10 (inclusive) e
    /// "OURO" para mais de 10 compras.
    /// </summary>
    /// <example>
    /// ObterCategoriaCliente(2) retorna "BRONZE";
    /// ObterCategoriaCliente(7) retorna "PRATA";
    /// ObterCategoriaCliente(15) retorna "OURO".
    /// </example>
    public string ObterCategoriaCliente(int totalCompras)
    {
        if (totalCompras < 5)
        {
            return "BRONZE";
        }

        if (totalCompras <= 10)
        {
            return "PRATA";
        }

        return "OURO";
    }

    /// <summary>
    /// Retorna o valor final com o desconto percentual aplicado.
    /// </summary>
    /// <example>
    /// CalcularDescontoPorPercentual(100, 10) retorna 90.
    /// </example>
    public int CalcularDescontoPorPercentual(int valorOriginal, int percentualDesconto)
    {
        return valorOriginal - (valorOriginal * percentualDesconto / 100);
    }

    /// <summary>
    /// Verifica se o cliente é válido para usar o cupom.
    /// Regra: ter 18 anos ou mais OU ser a primeira compra.
    /// </summary>
    /// <example>
    /// EValidoParaCupom(20, false) retorna true;
    /// EValidoParaCupom(16, true) retorna true;
    /// EValidoParaCupom(17, false) retorna false.
    /// </example>
    public bool EValidoParaCupom(int idade, bool primeiraCompra)
    {
        return idade >= 18 || primeiraCompra;
    }
}
