// Set new default font family and font color to mimic Bootstrap's default styling
Chart.defaults.global.defaultFontFamily = '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#292b2c';

function plotBarChart(data) {

  // Bar Chart Example
  var ctx = document.getElementById("myBarChart");
  var myLineChart = new Chart(ctx, {
    type: 'bar',
    data: {
      labels: ["Prescence of Pneumonia", "Abscence of Pneumonia"],
      datasets: [{
        label: "Number of images diagnosed",
        backgroundColor: "rgba(2,117,216,1)",
        borderColor: "rgba(2,117,216,1)",
        data: data,
      }],
    },
    options: {
      scales: {
        xAxes: [{
          gridLines: {
            display: true
          },
          ticks: {
            maxTicksLimit: 2
          }
        }],
        yAxes: [{
          ticks: {
            min: 0,
            max: 100,
            maxTicksLimit: 5
          },
          gridLines: {
            display: true
          }
        }],
      },
      legend: {
        display: false
      }
    }
  });
}