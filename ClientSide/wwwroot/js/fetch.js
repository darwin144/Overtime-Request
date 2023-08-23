const token = $("#token").text();

// Parse token sebagai JSON
const decodedToken = JSON.parse(atob(token.split('.')[1]));

// Mengambil nilai primarysid
const primarysid = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid"];
document.getElementById("primarysid").textContent = primarysid;

console.log("primarysid: " + primarysid);

function fetchEmployeeData() {
    fetch(`https://localhost:7173/API-Overtimes/Overtime/ChartManagerByGuid/${primarysid}`, {
        headers: {
            Authorization: `Bearer ${token}`
        }
    })
        .then(response => response.json())
        .then(data => {
            const approved = data.data.approved;
            const rejected = data.data.rejected;
            const totalMale = data.data.totalMale;
            const totalFemale = data.data.totalFemale;
            const total = totalMale + totalFemale;

            console.log(total);
            console.log(approved);

            document.getElementById('profile-views').textContent = total;
            document.getElementById('followers').textContent = approved;
            document.getElementById('following').textContent = rejected;
        })
        .catch(error => console.log(error));
}

function fetchGenderChartData() {
    fetch(`https://localhost:7173/API-Overtimes/Overtime/ChartManagerByGuid/${primarysid}`, {
        headers: {
            Authorization: `Bearer ${token}`
        }
    })
        .then(response => response.json())
        .then(data => {
            const totalMale = data.data.totalMale;
            const totalFemale = data.data.totalFemale;

            const chartOptions = {
                series: [totalMale, totalFemale],
                labels: ['Male', 'Female'],
                chart: {
                    type: 'donut',
                },
                plotOptions: {
                    pie: {
                        donut: {
                            size: '50%',
                        },
                    },
                },
                colors: ["#FF7F00", "#0099C6"],
                responsive: [{
                    breakpoint: 480,
                    options: {
                        chart: {
                            width: 200,
                        },
                        legend: {
                            position: 'bottom',
                        },
                    },
                }],
            };

            const chart = new ApexCharts(document.querySelector("#chart-gender"), chartOptions);
            chart.render();
        })
        .catch(error => console.log(error));
}

function fetchRemainingOvertimeData() {
    fetch(`https://localhost:7173/API-Overtimes/Overtime/ListRemainingByManagerGuid/${primarysid}`, {
        headers: {
            Authorization: `Bearer ${token}`
        }
    })
        .then((response) => response.json())
        .then((data) => {
            const users = data.data;

            const categories = users.map((user) => user.fullname);
            const chartData = users.map((user) => user.remainingOvertime);
            console.log(users);
            var options = {
                series: [
                    {
                        data: chartData,
                    },
                ],
                chart: {
                    height: 350,
                    type: "bar",
                    events: {
                        click: function (chart, w, e) {
                            // console.log(chart, w, e)
                        },
                    },
                },
                plotOptions: {
                    bar: {
                        columnWidth: "50%",
                        distributed: true,
                    },
                },
                dataLabels: {
                    enabled: false,
                },
                legend: {
                    show: false,
                },
                xaxis: {
                    categories: categories,
                    labels: {
                        style: {
                            colors: ["#333"],
                            fontSize: "12px",
                        },
                        title: {
                            text: "Age (years)",
                        },
                    },
                },
            };

            var chart = new ApexCharts(
                document.querySelector("#charta"),
                options
            );
            chart.render();
        })
        .catch((error) => console.log(error));
}

// Panggil fungsi-fungsi untuk mengambil data dan merender grafik
fetchEmployeeData();
fetchGenderChartData();
fetchRemainingOvertimeData();
