
// Creates a doughnut chart. passLabels, passId, PassData, needs to be created in js beforehand. Also canvas with correct id
new Chart("doughnut_" + passId, {
    type: "doughnut",
    data: {
        labels: passLabels,
        datasets: [{
            // 10 colors, assuming no more than 10 categories will exist
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
            hoverBorderColor: '#00000033',
            borderAlign: 'inner',
            borderWidth: 2,
            
        }]
    },
    options: {
        legend: { display: true },
        scales: { display: false },
    }
});