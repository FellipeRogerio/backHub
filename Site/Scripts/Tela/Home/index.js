$(document).ready(function () {
    var dt = Biblioteca.Chamada.Get(("Home/Consultar").RetornaURL(), {}, false);

    Chart.defaults.line.spanGaps = true;

    GraficoFinanceiroSemestre(dt);
    GraficoCompraQuinzena(dt.filter(x=> x.Tipo == 2));
    GraficoVendaQuinzena(dt.filter(x => x.Tipo == 1));
    GraficoPedidoNota(dt);
    GraficoTempoProcVenda(dt);


 

})


function GraficoCompraQuinzena(dt) {
    var valor = dt.map(x => [x.Janeiro, x.Fevereiro, x.Março, x.Abril, x.Maio, x.Junho])[0];
  

    var vTot = valor.reduce((tot, i) => tot + i);

    $("#txtTotCompra").html(`R$ ${vTot.FormataDecimal(2)}`);
    var widgetlineChart1 = new Chartist.Line('#grafContaQuinzena', {
        labels: ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15"],
        series: [valor
        ]
    }, {
            axisX: {
                showGrid: true,
                showLabel: false,
                offset: 0,
            },
            axisY: {
                showGrid: false,
                low: 40,
                showLabel: false,
                offset: 0,
            },
            lineSmooth: Chartist.Interpolation.cardinal({
                tension: 0
            }),
            //low: 0,
            //high: 8,
            fullWidth: true,
            plugins: [
                Chartist.plugins.tooltip()
            ]
        });
}
function GraficoVendaQuinzena(dt) {
    var valor = dt.map(x => [x.Janeiro, x.Fevereiro, x.Março, x.Abril, x.Maio, x.Junho])[0];

    var vTot = valor.reduce((tot, i) => tot + i);
    $("#txtTotVenda").html(`R$ ${vTot.FormataDecimal(2)}`);
    var widgetlineChart1 = new Chartist.Line('#grafVendaQuinzena', {
        labels: ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15"],
        series: [valor
        ]
    }, {
            axisX: {
                showGrid: true,
                showLabel: false,
                offset: 0,
            },
            axisY: {
                showGrid: false,
                low: 40,
                showLabel: false,
                offset: 0,
            },
            lineSmooth: Chartist.Interpolation.cardinal({
                tension: 0
            }),
            //low: 0,
            //high: 8,
            fullWidth: true,
            plugins: [
                Chartist.plugins.tooltip()
            ]
        });
}

function GraficoFinanceiroSemestre(dt) {


    //var a = dt.map(x => [x.Janeiro, x.Fevereiro, x.Março, x.Abril, x.Maio, x.Junho]);
    var myChart = new Chart($('#myChart'), {
        type: 'bar',
        data: {
            labels: ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dec"],
            datasets: [{
                label: 'Despesas',
                stack: 'Stack 0',
                data: dt.filter(x => x.Tipo == 2).map(x => [x.Janeiro, x.Fevereiro, x.Março, x.Abril, x.Maio, x.Junho])[0],
                backgroundColor: 'rgba(255, 99, 132, 0.7)',
                borderColor: 'rgba(255, 99, 132, 0.2)',
                borderWidth: 0.5
            },
            {
                label: 'Receita',
                stack: 'Stack 0',
                data: dt.filter(x => x.Tipo == 1).map(x => [x.Junho, x.Maio, x.Abril, x.Março, x.Fevereiro, x.Janeiro])[0],
                backgroundColor: 'rgba(75, 192, 192, 0.7)',
                borderColor: 'rgba(75, 192, 192, 1)',
                borderWidth: 0.5
            }
            ]



        },
        options: {
            tooltips: {
                mode: 'index',
                intersect: false
            },
            annotation: {
                annotations: [{
                    type: 'line',
                    mode: 'horizontal',
                    scaleID: 'y-axis-0',
                    value: '25',
                    borderColor: 'red',
                    borderWidth: 2
                }]
            },
            gridLines: {
                drawOnChartArea: false,
            },

            scales: {
                yAxes: [{
                    gridLines: {
                        display: false,
                        offsetGridLines: true
                    },
                    drawOnChartArea: false,
                    ticks: {
                        beginAtZero: true
                    }
                }],
                xAxes: [{
                    gridLines: {
                        display: true,
                        offsetGridLines: true
                    },
                    display: true,
                    scaleLabel: {
                        display: true,
                    },
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
            , elements: {
                line: {
                    fill: false
                }
            }

        }
    });
}

function GraficoPedidoNota(dt) {

    var lineArea2 = new Chartist.Line('#grafPedNF', {
        labels: ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dec"],
        series: [
            [80, 95, 87, 79, 73, 68, 57, 60, 73, 83, 81, 43],
            [75, 95, 82, 84, 80, 65, 60, 60, 80, 84, 83, 44],
        ]
    }, {
            showArea: true,
            fullWidth: true,
            lineSmooth: Chartist.Interpolation.none(),
            axisX: {
                showGrid: false,
            },
            axisY: {
                low: 0,
                scaleMinSpace: 50,
            }
        },
        [
            ['screen and (max-width: 640px) and (min-width: 381px)', {
                axisX: {
                    labelInterpolationFnc: function (value, index) {
                        return index % 2 === 0 ? value : null;
                    }
                }
            }],
            ['screen and (max-width: 380px)', {
                axisX: {
                    labelInterpolationFnc: function (value, index) {
                        return index % 3 === 0 ? value : null;
                    }
                }
            }]
        ]);

    lineArea2.on('created', function (data) {
        var defs = data.svg.elem('defs');
        defs.elem('linearGradient', {
            id: 'gradient2',
            x1: 0,
            y1: 1,
            x2: 0,
            y2: 0
        }).elem('stop', {
            offset: 0,
            'stop-opacity': '0.2',
            'stop-color': 'rgba(255, 255, 255, 1)'
        }).parent().elem('stop', {
            offset: 1,
            'stop-opacity': '0.2',
            'stop-color': 'rgba(0, 201, 255, 1)'
        });

        defs.elem('linearGradient', {
            id: 'gradient3',
            x1: 0,
            y1: 1,
            x2: 0,
            y2: 0
        }).elem('stop', {
            offset: 0.3,
            'stop-opacity': '0.2',
            'stop-color': 'rgba(255, 255, 255, 1)'
        }).parent().elem('stop', {
            offset: 1,
            'stop-opacity': '0.2',
            'stop-color': 'rgba(132, 60, 247, 1)'
        });
    });
    lineArea2.on('draw', function (data) {
        var circleRadius = 4;
        if (data.type === 'point') {

            var circle = new Chartist.Svg('circle', {
                cx: data.x,
                cy: data.y,
                r: circleRadius,
                class: 'ct-point-circle'
            });
            data.element.replace(circle);
        }
        else if (data.type === 'label') {
            // adjust label position for rotation
            const dX = data.width / 2 + (30 - data.width)
            data.element.attr({ x: data.element.attr('x') - dX })
        }
    });
    // Line with Area Chart 2 Ends
    


}

function GraficoTempoProcVenda(dt) {

    const label  = "lblCicloVenda";
    const txt = "txtCicloVenda";
    var Donutdata = {
        series: [
            {
                "name": "orcamento",
                "className": "ct-done",
                "label": "Orçamento",
                "value": 15
            },
            {
                "name": "analise",
                "className": "ct-progress",
                "label": "Análise",
                "value": 22
            },
            {
                "name": "estoque",
                "className": "ct-outstanding",
                "label": "Estoque",
                "value": 33
            },
            {
                "name": "NF",
                "className": "ct-started",
                "label": "Nota Fiscal",
                "value": 30
            }
        ]
    };

    var donut = new Chartist.Pie('#grafTempVenda', Donutdata, {
            donut: true,
            startAngle: 0,
            labelInterpolationFnc: function (value) {
                var total = Donutdata.series.reduce(function (prev, series) {
                    $(`#${label}${Donutdata.series.indexOf(series) + 1}`).html(`${series.value.FormataDecimal(2)}% ${series.label}`);
                    $(`#${txt}${Donutdata.series.indexOf(series) + 1}`).attr("aria-valuenow", series.value);
                    $(`#${txt}${Donutdata.series.indexOf(series) + 1}`).css('width', series.value + '%');
                    return prev + series.value;
                }, 0);
                return total + '%';
            }
        });

    donut.on('draw', function (data) {
        if (data.type === 'label') {
            if (data.index === 0) {
                data.element.attr({
                    dx: data.element.root().width() / 2,
                    dy: data.element.root().height() / 2
                });
            } else {
                data.element.remove();
            }
        }
    });
    // Donut Chart Ends
}

function CarregaCotacao() {
    const lblDia = "txtCotacaoDia";
    const icoDia = "icoCotacao";
    const valDia = "txtCotacao";


}