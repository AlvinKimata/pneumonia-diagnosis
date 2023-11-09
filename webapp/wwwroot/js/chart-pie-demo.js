// Set new default font family and font color to mimic Bootstrap's default styling
Chart.defaults.global.defaultFontFamily = '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#292b2c';

function plotPieChart(data) {
  // Pie Chart Example
  var ctx = document.getElementById("myPieChart");
  var myPieChart = new Chart(ctx, {
    type: 'doughnut',
    data: {
      labels: ["Blue", "Red"],
      datasets: [{
        data: data,
        backgroundColor: ['#007bff', '#dc3545'],
      }],
    },
    options: {
      scales: {
        xAxes: [{
          gridLines: {
            display: false
          },
        }],
        yAxes: [{
          gridLines: {
            display: false
          }
        }],
      }
    }
  });
}