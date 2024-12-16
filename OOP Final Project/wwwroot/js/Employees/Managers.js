$(document).ready(function () {
  // Handle sidebar link clicks
  $(".side-link").on("click", function (e) {
    e.preventDefault(); // Prevent the default behavior

    const page = $(this).data("page"); // Get the data-page attribute

    // Send an AJAX request to load the selected partial view
    $("#main-content").load(
      "/Employees/TestManager?handler=LoadPartial&section=" + page,
      function (response, status, xhr) {
        if (status === "error") {
          console.error("Error loading content:", xhr.status, xhr.statusText);
          $("#main-content").html(
            '<div class="text-danger text-center">Error loading content.</div>',
          );
        }
      },
    );
  });
  // Initialize the chart
  initializeChart();
});

function initializeChart() {
  const ctx = document.getElementById("performanceChart").getContext("2d");
  new Chart(ctx, {
    type: "line",
    data: {
      labels: ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday"],
      datasets: [
        {
          label: "Appointments",
          data: [12, 19, 3, 5, 2],
          borderColor: "#195478",
          borderWidth: 2,
          fill: false,
          tension: 0.1,
        },
      ],
    },
    options: {
      responsive: true,
      plugins: {
        legend: {
          display: true,
        },
      },
    },
  });
}
