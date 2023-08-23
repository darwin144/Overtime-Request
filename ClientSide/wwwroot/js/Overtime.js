const token = $("#token").text();
const guid = $("#guidEmployee").text();


$.ajax({
    url: `https://localhost:7173/API-Overtimes/Overtime/ByEmployee/${guid}`,
    type: "GET",
    headers: {
        "Authorization": "Bearer " + token
    },
    success: (result) => {
        const data = result.data;
        const table = $('#tableOvertimeBody').DataTable({
            dom: 'Bfrtip',
            buttons: ['pdf'],
            columns: [
                { title: 'NO' },
                { title: 'Start Overtime' },
                { title: 'End Overtime' },
                { title: 'Submit Date' },
                { title: 'Paid' },
                { title: 'Status' }
                /*{ title: 'Actions' }*/
            ]
        });
        table.clear(); 
        $.each(data, (index, val) => {
            const row = [
                index + 1,
                moment(val.startOvertime).locale('id').format('DD MMMM YYYY [Pukul] HH:mm'),
                moment(val.endOvertime).locale('id').format('DD MMMM YYYY [Pukul] HH:mm'),
                moment(val.submitDate).locale('id').format('DD MMMM YYYY'),
                "Rp. " + val.paid.toString().replace(/\B(?=(\d{3})+(?!\d))/g, "."),
                getStatusBadge(val.status),
                `<button hidden class="btn btn-danger btn-delete" onclick="deleteRow(${val.id})">Delete</button>`,
                `<button class="btn btn-primary btn-print" onclick="printPayslip(${val.id})">Print Payslip</button>`

            ];

            table.row.add(row); 
        });

        table.draw();
    }
});

function getStatusBadge(status) {
    if (status === 0) {
        return `<span class="badge bg-primary">Approval</span>`;
    } else if (status === 1) {
        return `<span class="badge bg-success">Approved</span>`;
    } else {
        return `<span class="badge bg-danger">Rejected</span>`;
           }
}


//payslip table
$.ajax({
    url: `https://localhost:7173/API-Overtimes/Payroll/GetByEmployeeId/${guid}`,
    type: "GET",
    headers: {
        "Authorization": "Bearer " + token
    },
    success: (result) => {
        const data = result.data;
        const table = $('#tablepayslip').DataTable({
            dom: 'Bfrtip',
            buttons: ['pdf'],
            columns: [
                { title: 'NO' },
                { title: 'Pay Date' },
                { title: 'Name' },
                { title: 'Department' },
                { title: 'Title' },
                { title: 'Allowence' },
                { title: 'Overtime' },
                { title: 'Salary Cuts' },
                { title: 'Total Salary' },
                { title: 'Actions' }
                
            ]
        });
        table.clear();
        $.each(data, (index, val) => {
            const row = [
                index + 1,
                moment(val.payDate).locale('id').format('DD MMMM YYYY'),
                (val.fullname),
                (val.department),
                (val.title),
                "Rp. " + val.allowence.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".") +"",
                "Rp. " + val.overtime.toString().replace(/\B(?=(\d{3})+(?!\d))/g, "."),
                "Rp. " + val.payrollCuts.toString().replace(/\B(?=(\d{3})+(?!\d))/g, "."),
                "Rp. " + val.totalSalary.toString().replace(/\B(?=(\d{3})+(?!\d))/g, "."),
                `<button class="btn btn-secondary btn-print btn-md" onclick="printPayslip()"><i class="bi bi-printer-fill p-2"></i></i></button>`
            ];

            table.row.add(row);
        });

        table.draw();
    }
});
function printPayslip() {
    const payslipTemplateFile = '/lib/file.html';

    // Retrieve data from the table
    const tableData = $('#tablepayslip').DataTable().data().toArray();

    // Create HTML elements to represent the payslip information
    let payslipInfoHTML = `<body
    style="
      box-sizing: border-box;
      margin: 0;
      font-family: Roboto, sans-serif, Helvetica Neue, Arial, Noto Sans,
        sans-serif;
      font-size: 0.875rem;
      font-weight: 400;
      line-height: 1.65;
      color: #526484;
      text-align: left;
      background-color: #fff !important;
      min-width: 520px;
      outline: none;
    "
  >
    <div class="card-inner" style="box-sizing: border-box; padding: 1.25rem">
      <table
        class="email-wraper"
        style="
          box-sizing: border-box;
          border-collapse: collapse;
          background: #f5f6fa;
          font-size: 14px;
          line-height: 22px;
          font-weight: 400;
          color: #8094ae;
          width: 100%;
        "
      >
        <tr style="box-sizing: border-box; page-break-inside: avoid">
          <td
            class="py-5"
            style="
              box-sizing: border-box;
              padding-top: 2.75rem !important;
              padding-bottom: 2.75rem !important;
            "
          >
            <table
              class="email-header"
              style="
                box-sizing: border-box;
                border-collapse: collapse;
                width: 100%;
                max-width: 620px;
                margin: 0 auto;
              "
            >
              <tbody style="box-sizing: border-box">
                <tr style="box-sizing: border-box; page-break-inside: avoid">
                  <td
                    class="text-center pb-4"
                    style="
                      box-sizing: border-box;
                      padding-bottom: 1.5rem !important;
                      text-align: center !important;
                    "
                  >
                    <p
                      class="email-title"
                      style="
                        box-sizing: border-box;
                        margin-top: 0;
                        margin-bottom: 0;
                        orphans: 3;
                        widows: 3;
                        font-size: 16px;
                        color: #754fa3;
                        padding-top: 12px;
                      "
                    >
                      Payslip Karyawan Metrodata Elecetronics Tbk.
                    </p>
                  </td>
                </tr>
              </tbody>
            </table>
            <table
              class="email-body"
              style="
                box-sizing: border-box;
                border-collapse: collapse;
                max-width: 720px;
                margin: 0 auto;
                background: #ffffff;
                width: 100%;
              "
            >`;

    tableData.forEach((row, index) => {
        payslipInfoHTML += `
              <tbody>
                <tr>
                  <td align="center" style="padding-top: 6px">
                    <table
                      role="presentation"
                      style="
                        width: 90%;
                        border-collapse: collapse;
                        border: none;
                        background-color: white;
                        border-radius: 1px;
                      "
                    >
                      <tr>
                        <td
                          style="
                            text-align: left;
                            display: flex;
                            flex-direction: column;
                            align-items: center;
                            padding-top: 1.5em;
                          "
                        >
                          <img
                            src="data:image/png;base64,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"
                            height="100"
                            style="background-size: cover"
                          />
                          <p
                            style="
                              color: #422f59;
                              font-size: 14px;
                              font-weight: 600;
                              padding-top: 1em;
                            "
                          >
                            Payslip Untuk Bulan ${row[1]}
                          </p>
                          
                        </td>
                      </tr>
                      <tr>
                        <td>
                          <hr />
                        </td>
                      </tr>
                      <tr>
                        <td
                          style="
                            padding-inline: 24px;
                            display: flex;
                            flex-direction: row;
                            justify-content: space-between;
                          "
                        >
                          <table style="width: 100%">
                            <tr>
                              <td class="cell">Name:</td>
                              <td class="cell cell-data" style="float: right">
                                <span class="data">${row[2]}</span>
                              </td>
                            </tr>
                            <tr>
                              <td class="cell">Title:</td>
                              <td class="cell cell-data" style="float: right">
                                <span class="data">${row[4]}</span>
                              </td>
                            </tr>
                            <tr>
                              <td class="cell">Department:</td>
                              <td class="cell cell-data" style="float: right">
                                <span class="data">${row[3]}</span>
                              </td>
                            </tr>
                            <tr>
                              <td class="cell">Currency :</td>
                              <td class="cell cell-data" style="float: right">
                                IDR
                              </td>
                            </tr>
                          </table>
                        </td>
                      </tr>
                      <tr>
                        <td style="padding-top: 24px">
                          <hr />
                        </td>
                      </tr>
                      <tr>
                        <td style="padding-inline: 24px">
                          <table style="width: 100%; border-collapse: collapse">
                            <tr style="border-bottom: solid 0.1px#b3b0b0">
                              <th class="cell cell-data" style="float: left">
                                Deskripsi
                              </th>
                              <th
                                class="cell cell-data"
                                style="text-align: start"
                              >
                                Penghasilan
                              </th>
                              <th class="cell cell-data" style="float: right">
                                Potongan
                              </th>
                            </tr>
                            <tr>
                              <td class="cell">Allowance</td>
                              <td class="cell">
                                <span class="data">${row[5]}</span>
                              </td>
                            </tr>
                            <tr>
                              <td class="cell">Pph &plus; Bpjs</td>
                              <td class="cell"></td>
                              <td class="cell" style="float: right">
                                <span class="data">${row[7]}</span>
                              </td>
                            </tr>
                            <tr
                              style="
                                border-top: solid 0.1px;
                                border-bottom: solid 0.1px;
                              "
                            >

                            <tr>
                              <td class="cell" colspan="2">Overtime</td>

                              <td class="cell" style="float: right">
                                <span class="data">${row[6]}</span>
                              </td>
                            </tr>
                            <tr
                              style="
                                border-top: solid 0.1px;
                                border-bottom: solid 0.1px;
                              "
                            >
                              <td class="cell cell-data" colspan="2">
                                Total Salary
                              </td>

                              <td class="cell cell-data" style="float: right">
                                <span class="data">${row[8]}</span>
                              </td>
                            </tr>
                          </table>
                        </td>
                      </tr>

                      <tr>
                        <td
                          style="
                            padding-inline: 24px;
                            padding-bottom: 14px;
                            padding-top: 34px;
                          "
                          class="cell"
                          colspan="2"
                        ></td>
                      </tr>

                      <tr>
                        <td
                          style="
                            padding-inline: 24px;
                            padding-bottom: 24px;
                            padding-top: 14px;
                          "
                          class="cell"
                          colspan="2"
                        >
                          Employee's signature: ___________
                        </td>
                      </tr>

                      <tr>
                        <td
                          style="
                            padding-inline: 24px;
                            padding-bottom: 14px;
                            padding-top: 34px;
                          "
                          class="cell"
                          colspan="2"
                        ></td>
                      </tr>

                      <tr>
                        <td
                          style="
                            text-align: left;
                            display: flex;
                            flex-direction: column;
                            align-items: center;
                            padding-bottom: 24px;
                          "
                        >
                          <p
                            style="
                              color: #210b82;
                              font-size: 10px;
                              font-weight: 600;
                            "
                          >
                            Thank You for your service
                          </p>
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
              </tbody>
              `;
    });

    payslipInfoHTML += `</table>
            <table
              class="email-footer"
              style="
                box-sizing: border-box;
                border-collapse: collapse;
                width: 100%;
                max-width: 620px;
                margin: 0 auto;
              "
            >
              <tbody style="box-sizing: border-box">
                <tr style="box-sizing: border-box; page-break-inside: avoid">
                  <td
                    class="text-center pt-4"
                    style="
                      box-sizing: border-box;
                      padding-top: 1.5rem !important;
                      text-align: center !important;
                    "
                  >
                    <p
                      class="email-copyright-text"
                      style="
                        box-sizing: border-box;
                        margin-top: 0;
                        margin-bottom: 1rem;
                        orphans: 3;
                        widows: 3;
                        font-size: 13px;
                      "
                    >
                      Powered by
                      <a
                        
                        style="
                          box-sizing: border-box;
                          color: #0971fe;
                          text-decoration: underline;
                          background-color: transparent;
                          transition: color 0.4s, background-color 0.4s,
                            border 0.4s, box-shadow 0.4s;
                          word-break: break-all;
                        "
                      >
                        Kelompok 4 MCC</a
                      >.
                    </p>
                  </td>
                </tr>
              </tbody>
            </table>
          </td>
        </tr>
      </table>
    </div>
    </body>`;


   
    // Open a new window or tab with the payslip HTML template
    const printWindow = window.open(payslipTemplateFile, '_blank');
    printWindow.onload = () => {
        // Inject the payslip information into the payslip template
        const payslipContainer = printWindow.document.getElementById('payslip');
        payslipContainer.innerHTML = payslipInfoHTML;
        // Print the payslip HTML content
        printWindow.print();
    };
}