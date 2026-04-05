public class RenegociacaoResponse
{
    public double ValorTotalOriginal { get; set; }
    public double ValorTotalComJuros { get; set; }
    public double TotalJuros { get; set; }
    public List<ParcelaOutput> Parcelas { get; set; } = new();
}