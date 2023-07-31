new Chart("doughnut_" + passId, {
    type: "doughnut",
    data: {
        labels: passLabels,
        datasets: [{
            backgroundColor: [
                '#1e87e680'
                ,'#42f58180'
                ,'#e042f580'
                ,'#cbf54280'
                ,'#f5425180'
                ,'#4251f580'
                ,'#4bf54280'
                ,'#f542b080'
                ,'#f5964280'
                ,'#42f5f280'],
            data: passData,
            barThickness: 50
        }]
    },
    options: {
        legend: { display: true },
        scales: { display: false },
        yAxes: [{
            // Set label width. Will keep the charts the same size but will truncate long labels
            afterFit: function (scaleInstance) {
                scaleInstance.height = 150;
            }
        }]
    }
});