const token = $("#token").text();
const guid = $("#guidEmployee").text();


$.ajax({
    url: `https://localhost:7173/API-Overtimes/Overtime/ByManager/${guid}`,
    type: "GET",
    headers: {
        "Authorization": "Bearer " + token
    },
    success: (result) => {
        const data = result.data;
        const table = $('#tableOvertimeApproval').DataTable({
            data: data,
            columns: [
                { title: 'NO', data: null },
                { title: 'Name', data: 'fullname' },
                { title: 'Start Overtime', data: 'startOvertime' },
                { title: 'End Overtime', data: 'endOvertime' },
                { title: 'Submit Date', data: 'submitDate' },
                { title: 'Description', data: 'deskripsi' },
                { title: 'Paid', data: 'paid' },
                /*{ title: 'Actions', data: null }*/
            ],
            columnDefs: [
                {
                    targets: 0,
                    render: function (data, type, row, meta) {
                        return meta.row + 1;
                    }
                },
                {
                    targets: 2,
                    render: function (data, type, row, meta) {
                        return moment(data).locale('id').format('DD MMMM YYYY [Pukul] HH:mm');
                    }
                },
                {
                    targets: 3,
                    render: function (data, type, row, meta) {
                        return moment(data).locale('id').format('DD MMMM YYYY [Pukul] HH:mm');
                    }
                },
                {
                    targets: 4,
                    render: function (data, type, row, meta) {
                        return moment(data).locale('id').format('DD MMMM YYYY');
                    }
                },
                {
                    targets: 6,
                    render: function (data, type, row, meta) {
                        return "Rp. " + data.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
                    }
                },
                {
                    targets: 7,
                    render: function (data, type, row, meta) {
                        return `<button class="btn btn-success btn-delete" id="approved" onclick="Approved(${meta.row})"><i class="fas fa-check"></i>Approved</button>` +
                            `<button class="btn btn-danger btn-delete" id="rejected" onclick="Rejected(${meta.row})"><i class="fas fa-times"></i> Rejected</button>`;
                    }
                }
            ]
        });
    }
});

function Approved(index) {
    const data = $('#tableOvertimeApproval').DataTable().row(index).data();
    const overtime = {
        id: data.id,
        fullname: data.fullname,
        startOvertime: data.startOvertime,
        endOvertime: data.endOvertime,
        submitDate: data.submitDate,
        deskripsi: data.deskripsi,
        paid: data.paid,
        status: 1,
        employee_id: data.employee_id
    };
    console.log(overtime);
    $.ajax({
        headers: {
            "Authorization": "Bearer " + token,
            "Accept": "application/json",
            "Content-Type": "application/json"
        },
        type: "PUT",
        url: `https://localhost:7173/API-Overtimes/Overtime/OvertimeApproval/${guid}`,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(overtime)
    }).done((result) => {
        Swal.fire({
            title: 'Updated!',
            icon: 'success',
            showConfirmButton: false,
            timer: 1500
        })
        $('#tblOvertime').DataTable().ajax.reload();
   
    }).fail((error) => {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: `Something went wrong! ${error.responseJSON.message}`,
        })
        $('#tblOvertime').DataTable().ajax.reload();
    })
}


function Rejected(index) {
    const data = $('#tableOvertimeApproval').DataTable().row(index).data();
    const overtime = {
        id: data.id,
        fullName: data.fullName,
        startOvertime: data.startOvertime,
        endOvertime: data.endOvertime,
        submitDate: data.submitDate,
        deskripsi: data.deskripsi,
        paid: data.paid,
        status: 3,
        employee_id: data.employee_id
    };
    $.ajax({
        headers: {
            "Authorization": "Bearer " + token,
            "Accept": "application/json",
            "Content-Type": "application/json"
        },
        type: "PUT",
        url: `https://localhost:7173/API-Overtimes/Overtime/OvertimeApproval/${guid}`,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(overtime)
    }).done((result) => {
        Swal.fire({
            title: 'Updated!',
            icon: 'success',
            showConfirmButton: false,
            timer: 1500
        })
        $('#tblOvertime').DataTable().ajax.reload();

    }).fail((error) => {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: `Something went wrong! ${error.responseJSON.message}`,
        })
        $('#tblOvertime').DataTable().ajax.reload();
    })
}