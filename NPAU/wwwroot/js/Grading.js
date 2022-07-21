var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Student/Grading/GetAll"
        },
        "columns": [
            {
                "data": "courseEnrollment.courseSession.courseName", "width": "15%",
                
            },
            {
                "data": "assessment.name", "width": "15%"
            },
            {
                "data": "courseEnrollment.student.fullName", "width": "15%"
            },
            {
                "data": "score", "width": "15%"
            },
            {
                "data": "assessment.maxScore", "width": "15%"
            },
            
        ]
    });
}

