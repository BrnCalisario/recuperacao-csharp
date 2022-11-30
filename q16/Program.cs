using System.IO;
using System.Linq;
using static System.Console;
using System.Collections.Generic;

var days = getDays();
var bikes = getSharings();


// Q1
var mediaAlugueis = (
    bikes.Select(b => b.Casual).Sum() + 
    bikes.Select(b => b.Registred).Sum()) / bikes.Count();


// Q2
var mediaMes = 
    bikes
        .Select(b => new {
            totalDia = b.Casual + b.Registred,
            mes = (int) (b.Day / 30)
        })
        .GroupBy(x => x.mes)
        .Select(g => new {
            mes = g.Key,
            mediaMes = g.Select(g => g.totalDia).Sum() / g.Count()
        });




// Q3
var mediaPorEstacoes = 
    bikes
        .Join(days,
            b => b.Day,
            d => d.Day,
            (b, d) => new {
                estacao = d.Season,
                alugueisDia = b.Casual + b.Registred
            })
        .GroupBy(x => x.estacao)
        .Select(g => new {
            estacao = (Estacao) g.Key,
            media = g.Select(g => g.alugueisDia).Sum() / g.Count()
        });

var porWeather = 
    bikes  
        .Join(days,
            b => b.Day,
            d => d.Day,
            (b, d) => new {
                condicaoTempo = d.Weather,
                alugueisDia = b.Casual + b.Registred,
            })
        .GroupBy(x => x.condicaoTempo)
        .Select(g => new {
            condicaoTempo = g.Key,
            media = g.Select(g => g.alugueisDia).Sum() + g.Count()
        });

var porTemp = 
    bikes
        .Join(days,
            b => b.Day,
            d => d.Day,
            (b, d) => new {
                tempDiv = (int) (d.Temp * 100) / 25,
                alugueisDia = b.Casual + b.Registred
            })
        .GroupBy(x => x.tempDiv)
        .Select(g => new {
            temperatura = (Temperatura) g.Key,
            media = g.Select(g => g.alugueisDia).Sum() + g.Count()
        });

// Q4
var porDiaTrabalho =
    bikes
        .Join(days, 
            b => b.Day,
            d => d.Day,
            (b, d) => new {
                diaDeTrab = d.IsWorkingDay,
                alugueisDia = b.Casual + b.Registred
            })
        .GroupBy(x => x.diaDeTrab)
        .Select(g => new {
            diaDeTrab = g.Key ? "Dia de Trabalho" : "Dia de Folga",
            media = g.Select(g => g.alugueisDia).Sum() / g.Count()
        });

// Q5 
var picos = bikes
    .Join(days,
        b => b.Day,
        d => d.Day,
        (b, d) => new { 
            Day = b.Day,
            Casual = b.Casual,
            Registred = b.Registred,
            Temp = d.Temp,
            WorkDay = d.IsWorkingDay,
            Weather = d.Weather
        })
    .GroupBy(b => b.Day)
    .Select(g => new {
        dia = g.Key,
        alugueisDia = g.Sum( g => g.Casual + g.Registred),
        temperatura = g.Select(g => g.Temp).First() * 100,
        diaDeTrabalho = g.Select(g => g.WorkDay).First(),
        tempo = g.Select(g => g.Weather).First()
    });

foreach(var mes in mediaMes)
    WriteLine("Média mês " + mes.mes + ": " + mes.mediaMes);

foreach(var q in porDiaTrabalho)
    WriteLine(q.diaDeTrab + " " + q.media);

//  Q4 - Estações com mais alugueis: 
var melhorEstacao = mediaPorEstacoes.OrderByDescending(g => g.media).First();
    WriteLine(melhorEstacao);


// Q5 Alugueis por Qualidade de tempo
foreach(var g in porWeather.OrderBy(g => g.condicaoTempo))
   WriteLine(g);
// TODO

foreach(var g in porTemp.OrderByDescending(g => g.media))
    WriteLine(g);



// Q5 - Pico e Baixa, Observações: O dia de pico não era dia de trabalho e estava um clima agradavel
// Já o dia de baixa estava com a temperatura baixissima
var maxima = picos.OrderByDescending(b => b.alugueisDia).First();
var minima = picos.OrderBy(b => b.alugueisDia).First();
WriteLine(maxima + "\n " + minima);



IEnumerable<DayInfo> getDays()
{
    StreamReader reader = new StreamReader("dayInfo.csv");
    reader.ReadLine();

    while (!reader.EndOfStream)
    {
        var data = reader.ReadLine().Split(',');
        DayInfo info = new DayInfo();

        info.Day = int.Parse(data[0]);
        info.Season = int.Parse(data[1]);
        info.IsWorkingDay = int.Parse(data[2]) == 1;
        info.Weather = int.Parse(data[3]);
        info.Temp = float.Parse(data[4].Replace('.', ','));
        
        if (info.Temp  > 1) {
            info.Temp /= 1000;
        }

        yield return info;
    }

    reader.Close();
}

IEnumerable<BikeSharing> getSharings()
{

    StreamReader reader = new StreamReader("bikeSharing.csv");
    reader.ReadLine();

    while(!reader.EndOfStream)
    {
        var data = reader.ReadLine().Split(',');
        BikeSharing bikeSharing = new BikeSharing();

        bikeSharing.Day = int.Parse(data[0]);
        bikeSharing.Casual = int.Parse(data[1]);
        bikeSharing.Registred = int.Parse(data[2]);

        yield return bikeSharing;
    }

    reader.Close();
}

public class DayInfo
{
    public int Day { get; set; }
    public int Season { get; set; }
    public bool IsWorkingDay { get; set; }
    public int Weather { get; set; }
    public float Temp { get; set; }
}

public class BikeSharing
{
    public int Day { get; set; }
    public int Casual { get; set; }
    public int Registred { get; set; }
}

public enum Estacao {
    Inverno =  1,
    Primavera = 2,
    Verao = 3,
    Outono = 4,
}

public enum Temperatura {
    Frio = 0,
    Bom = 1,
    Calor = 2,
    INFERNO = 3,
}