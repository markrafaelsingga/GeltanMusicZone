document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('showEditModalBtn').addEventListener('click', function () {
        document.getElementById('editModal').style.display = 'block';
    });

    document.getElementById('showDetailModalBtn').addEventListener('click', function () {
        document.getElementById('detailModal').style.display = 'block';
    });

    document.getElementById('closeBtn').addEventListener('click', function () {
        document.getElementById('editModal').style.display = 'none';
        document.getElementById('detailModal').style.display = 'none';
    });

    document.getElementById('mainCloseBtn').addEventListener('click', function () {
        document.getElementById('editModal').style.display = 'none';
        document.getElementById('detailModal').style.display = 'none';
    });

    window.addEventListener('click', function (event) {
        if (event.target == document.getElementById('editModal')) {
            document.getElementById('editModal').style.display = 'none';
        }
        if (event.target == document.getElementById('detailModal')) {
            document.getElementById('detailModal').style.display = 'none';
        }
    });
});

function openEditModal(userId, fname, mi, lname, dep, dob, phone, address, email) {
    // Set the content dynamically
    document.getElementById('editFirstname').value = fname;
    // Set other fields as needed

    // Show the modal
    document.getElementById('editModal').style.display = 'block';
}

function openDetailModal(userId, fname, mi, lname, department, status) {
    // Set the content dynamically
    document.getElementById('detailFirstname').innerText = fname;
    // Set other fields as needed

    // Show the modal
    document.getElementById('detailModal').style.display = 'block';
}

function closeModal(modalId) {
    // Hide the specified modal
    document.getElementById(modalId).style.display = 'none';
}
