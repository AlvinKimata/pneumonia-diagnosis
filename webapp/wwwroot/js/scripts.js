window.addEventListener('DOMContentLoaded', event => {

    // Toggle the side navigation
    const sidebarToggle = document.body.querySelector('#sidebarToggle');
    if (sidebarToggle) {
        // Uncomment Below to persist sidebar toggle between refreshes
        // if (localStorage.getItem('sb|sidebar-toggle') === 'true') {
        //     document.body.classList.toggle('sb-sidenav-toggled');
        // }
        sidebarToggle.addEventListener('click', event => {
            event.preventDefault();
            document.body.classList.toggle('sb-sidenav-toggled');
            localStorage.setItem('sb|sidebar-toggle', document.body.classList.contains('sb-sidenav-toggled'));
        });
    }

});

//Regular expression function to parse a list of float stored as a string.
function extractFloatsFromString(inputString) {
    var regex = /[-+]?[0-9]*\.?[0-9]+/g;
    var matches = inputString.match(regex);
  
    if (matches) {
      return matches.map(function (match) {
        return parseFloat(match);
      });
    } else {
      return [];
    }
}