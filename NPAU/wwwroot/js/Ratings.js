var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Ratings/GetAll"
        },
        "columns": [
            {
                "data": "student.fullname", "width": "15%",
            },
            {
                "data": "age", "width": "15%"
            },
            {
                "data": "academics", "width": "15%"
            },
            {
                "data": "finances", "width": "15%"
            },
            {
                "data": "support", "width": "15%"
            },
            {
                "data": "distance", "width": "15%"
            },

        ]
    });
}