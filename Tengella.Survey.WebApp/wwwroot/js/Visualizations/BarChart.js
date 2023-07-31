new Chart("bar_" + passId, {
    type: "horizontalBar",
    data: {
        labels: passLabels,
        datasets: [{
            backgroundColor: '#1e87e680',
            data: passData,
            barThickness: 50
        }]
    },
    options: {
        legend: { display: false },
        scales: {
            xAxes: [{
                display: true,
                ticks: {
                    beginAtZero: true,
                    stepSize: 1,
                    precision: 0
                },
            }],
            yAxes: [{
                // Set label width. Will keep the charts the same size but will truncate long labels
                afterFit: function (scaleInstance) {
                    scaleInstance.width = 150;
                }
            }]
        }
    }
});