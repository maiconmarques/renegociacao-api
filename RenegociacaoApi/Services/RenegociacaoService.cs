public class RenegociacaoService
{
    private const int FinancialDaysPerMonth = 30;

    public RenegociacaoResponse Renegociar(RenegociacaoRequest request)
    {
        if (request.Titulos == null || !request.Titulos.Any())
            throw new Exception("Nenhum título informado");

        var totalOriginal = request.Titulos.Sum(t => t.ValorAberto);

        var parcelas = GerarParcelas(
            totalOriginal,
            request.NumeroParcelas,
            request.DataPrimeiraParcela,
            request.IntervaloDias
        );

        var totalComJuros = CalcularJuros(
            totalOriginal,
            request.TaxaJurosMensal,
            request.DataRenegociacao,
            parcelas
        );

        AjustarParcelas(parcelas, totalComJuros);

        return new RenegociacaoResponse
        {
            ValorTotalOriginal = totalOriginal,
            ValorTotalComJuros = totalComJuros,
            TotalJuros = Math.Round(totalComJuros - totalOriginal, 2),
            Parcelas = parcelas
        };
    }

    private List<ParcelaOutput> GerarParcelas(double valorTotal, int numeroParcelas, DateTimeOffset dataInicial, int intervaloDias)
    {
        var parcelas = new List<ParcelaOutput>();

        long totalCentavos = (long)Math.Round(valorTotal * 100);
        long baseParcela = totalCentavos / numeroParcelas;
        long resto = totalCentavos % numeroParcelas;

        for (int i = 0; i < numeroParcelas; i++)
        {
            long valorCentavos = baseParcela + (i < resto ? 1 : 0);

            parcelas.Add(new ParcelaOutput
            {
                Parcela = i + 1,
                Valor = valorCentavos / 100.0,
                DataVencimento = dataInicial.AddDays(i * intervaloDias)
            });
        }

        return parcelas;
    }

    private double CalcularJuros(double valorTotal, double taxaMensal, DateTimeOffset dataBase, List<ParcelaOutput> parcelas)
    {
        var totalDias = parcelas.Sum(p => (p.DataVencimento - dataBase).Days);

        var taxaDia = taxaMensal / FinancialDaysPerMonth;
        var taxaReal = taxaDia * totalDias;

        var valorComJuros = valorTotal + (valorTotal * taxaReal / 100);

        return Math.Floor(valorComJuros * 100) / 100;
    }

    private void AjustarParcelas(List<ParcelaOutput> parcelas, double novoTotal)
    {
        int qtd = parcelas.Count;

        long totalCentavos = (long)Math.Round(novoTotal * 100);
        long baseParcela = totalCentavos / qtd;
        long resto = totalCentavos % qtd;

        for (int i = 0; i < qtd; i++)
        {
            long valorCentavos = baseParcela + (i < resto ? 1 : 0);
            parcelas[i].Valor = valorCentavos / 100.0;
        }
    }
}