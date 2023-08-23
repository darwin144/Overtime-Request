function fetchData() {
    fetch('https://localhost:7044/API-Payroll/Overtime')
        .then(response => response.json())
        .then(data => {
            if (data.code === 200) {
                const dataTable = $('#tblOvertime').DataTable({
                    data: data.data,
                    columns: [
                        { data: 'id' },
                        { data: 'startOvertime' },
                        { data: 'endOvertime' },
                        { data: 'submitDate' },
                        { data: 'paid' },
                        { data: 'deskripsi' },
                        {
                            data: 'status',
                            render: function (data) {
                                return data === 1 ? 'Active' : 'Inactive';
                            }
                        },
                        {
                            data: 'id',
                            render: function (data) {
                                return `<button onclick="editData('${data}')">Edit</button>`;
                            }
                        }
                    ]
                });

                // Clear existing rows in the table
                dataTable.clear().draw();

                // Add new rows based on the fetched data
                dataTable.rows.add(data.data).draw();
            }
        })
        .catch(error => {
            console.error('Error:', error);
        });
}
