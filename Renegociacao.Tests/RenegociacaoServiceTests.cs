using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

[TestFixture]
public class RenegociacaoServiceTests
{
    private RenegociacaoService _service;

    [SetUp]
    public void Setup()
    {
        _service = new RenegociacaoService();
    }

    [Test]
    public void Deve_Criar_Parcelas_Corretamente()
    {
        var request = CriarRequest();

        var result = _service.Renegociar(request);

        Assert.AreEqual(3, result.Parcelas.Count);
    }

    [Test]
    public void Deve_Aumentar_Valor_Com_Juros()
    {
        var request = CriarRequest();

        var result = _service.Renegociar(request);

        Assert.Greater(result.ValorTotalComJuros, result.ValorTotalOriginal);
    }

    [Test]
    public void Soma_Das_Parcelas_Deve_Bater_Com_Total()
    {
        var request = CriarRequest();

        var result = _service.Renegociar(request);

        var soma = result.Parcelas.Sum(p => p.Valor);

        Assert.AreEqual(result.ValorTotalComJuros, soma, 0.01);
    }

    [Test]
    public void Deve_Lancar_Excecao_Sem_Titulos()
    {
        var request = new RenegociacaoRequest();

        Assert.Throws<Exception>(() => _service.Renegociar(request));
    }

    private RenegociacaoRequest CriarRequest()
    {
        return new RenegociacaoRequest
        {
            Titulos = new List<TituloInput>
            {
                new TituloInput
                {
                    Parcela = 1,
                    ValorAberto = 1000,
                    DataVencimento = DateTimeOffset.Now.AddDays(-10)
                }
            },
            DataRenegociacao = DateTimeOffset.Now,
            DataPrimeiraParcela = DateTimeOffset.Now.AddDays(30),
            TaxaJurosMensal = 2,
            NumeroParcelas = 3,
            IntervaloDias = 30
        };
    }
}