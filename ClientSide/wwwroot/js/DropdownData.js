    $(document).ready(function () {
        $.ajax({
            type: "GET",
            url: "https://localhost:7173/API-Overtimes/Department",
            data: "",
            success: function (data) {
                console.log(data)
                console.log(data.data.length)
                var s = '<option value="">Pilih department</option>';
                for (var i = 0; i < data.data.length; i++) {
                    console.log(data.data[i])
                    s += `<option value="${data.data[i].id}">${data.data[i].name}</option>`;
                }
                $("#departmentdropdown").html(s);
            }
        });
    });


    $(document).ready(function () {

        $.ajax({
            type: "GET",
            url: "https://localhost:7173/API-Payroll/EmployeeLevel",
            data: "",
            success: function (data) {
                console.log(data)
                console.log(data.data.length)
                var s = '<option value="">Pilih Employee Level</option>';
                for (var i = 0; i < data.data.length; i++) {
                    console.log(data.data[i])
                    s += `<option value="${data.data[i].id}">${data.data[i].title} - ${data.data[i].level}</option>`;
                }
                $("#employeeleveldropdown").html(s);
            }
        });
    });

    $(document).ready(function () {
        $.ajax({
            type: "GET",
            url: "https://localhost:7173/API-Overtimes/AccountRole/GetAllMasterAccountRole",
            data: "",
            success: function (data) {
                console.log(data)
                console.log(data.data.length)
                var s = '<option value="">Pilih Manager</option>';
                var addReportTo = [];

                for (var i = 0; i < data.data.length; i++) {
                    if (data.data[i].name == "Employee") {
                        s += `<option value="${data.data[i].employee_id}">${data.data[i].firstName} ${data.data[i].lastName}</option>`;
                        addReportTo.push(data.data[i].reportTo);
                    }
                    console.log(data.data[i])

                }
                $("#managerdropdown").html(s);
            }
        });
    });


