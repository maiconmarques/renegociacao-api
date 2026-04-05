public class RenegociacaoRequest
{
    public List<TituloInput> Titulos { get; set; } = new();
    public DateTimeOffset DataRenegociacao { get; set; }
    public DateTimeOffset DataPrimeiraParcela { get; set; }
    public double TaxaJurosMensal { get; set; }
    public int NumeroParcelas { get; set; }
    public int IntervaloDias { get; set; }
}